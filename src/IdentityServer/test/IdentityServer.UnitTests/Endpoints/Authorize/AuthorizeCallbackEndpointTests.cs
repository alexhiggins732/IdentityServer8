/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Collections.Specialized;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer.UnitTests.Common;
using IdentityServer8;
using IdentityServer8.Configuration;
using IdentityServer8.Endpoints;
using IdentityServer8.Endpoints.Results;
using IdentityServer8.Extensions;
using IdentityServer8.Models;
using IdentityServer8.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Xunit;

namespace IdentityServer.UnitTests.Endpoints.Authorize
{
    public class AuthorizeCallbackEndpointTests
    {
        private const string Category = "Authorize Endpoint";

        private HttpContext _context;

        private TestEventService _fakeEventService = new TestEventService();

        private ILogger<AuthorizeCallbackEndpoint> _fakeLogger = TestLogger.Create<AuthorizeCallbackEndpoint>();

        private IdentityServerOptions _options = new IdentityServerOptions();

        private MockConsentMessageStore _mockUserConsentResponseMessageStore = new MockConsentMessageStore();

        private MockUserSession _mockUserSession = new MockUserSession();

        private NameValueCollection _params = new NameValueCollection();

        private StubAuthorizeRequestValidator _stubAuthorizeRequestValidator = new StubAuthorizeRequestValidator();

        private StubAuthorizeResponseGenerator _stubAuthorizeResponseGenerator = new StubAuthorizeResponseGenerator();

        private StubAuthorizeInteractionResponseGenerator _stubInteractionGenerator = new StubAuthorizeInteractionResponseGenerator();

        private AuthorizeCallbackEndpoint _subject;

        private ClaimsPrincipal _user = new IdentityServerUser("bob").CreatePrincipal();

        private ValidatedAuthorizeRequest _validatedAuthorizeRequest;

        public AuthorizeCallbackEndpointTests()
        {
            Init();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task ProcessAsync_authorize_after_consent_path_should_return_authorization_result()
        {
            var parameters = new NameValueCollection()
            {
                { "client_id", "client" },
                { "nonce", "some_nonce" },
                { "scope", "api1 api2" }
            };
            var request = new ConsentRequest(parameters, _user.GetSubjectId());
            _mockUserConsentResponseMessageStore.Messages.Add(request.Id, new Message<ConsentResponse>(new ConsentResponse()));

            _mockUserSession.User = _user;

            _context.Request.Method = "GET";
            _context.Request.Path = new PathString("/connect/authorize/callback");
            _context.Request.QueryString = new QueryString("?" + parameters.ToQueryString());

            var result = await _subject.ProcessAsync(_context);

            result.Should().BeOfType<AuthorizeResult>();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task ProcessAsync_authorize_after_login_path_should_return_authorization_result()
        {
            _context.Request.Method = "GET";
            _context.Request.Path = new PathString("/connect/authorize/callback");
            _mockUserSession.User = _user;

            var result = await _subject.ProcessAsync(_context);

            result.Should().BeOfType<AuthorizeResult>();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task ProcessAsync_consent_missing_consent_data_should_return_error_page()
        {
            var parameters = new NameValueCollection()
            {
                { "client_id", "client" },
                { "nonce", "some_nonce" },
                { "scope", "api1 api2" }
            };
            var request = new ConsentRequest(parameters, _user.GetSubjectId());
            _mockUserConsentResponseMessageStore.Messages.Add(request.Id, new Message<ConsentResponse>(null));

            _mockUserSession.User = _user;

            _context.Request.Method = "GET";
            _context.Request.Path = new PathString("/connect/authorize/callback");
            _context.Request.QueryString = new QueryString("?" + parameters.ToQueryString());

            var result = await _subject.ProcessAsync(_context);

            result.Should().BeOfType<AuthorizeResult>();
            ((AuthorizeResult)result).Response.IsError.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task ProcessAsync_no_consent_message_should_return_redirect_for_consent()
        {
            _stubInteractionGenerator.Response.IsConsent = true;

            var parameters = new NameValueCollection()
            {
                { "client_id", "client" },
                { "nonce", "some_nonce" },
                { "scope", "api1 api2" }
            };
            var request = new ConsentRequest(parameters, _user.GetSubjectId());
            _mockUserConsentResponseMessageStore.Messages.Add(request.Id, null);

            _mockUserSession.User = _user;

            _context.Request.Method = "GET";
            _context.Request.Path = new PathString("/connect/authorize/callback");
            _context.Request.QueryString = new QueryString("?" + parameters.ToQueryString());

            var result = await _subject.ProcessAsync(_context);

            result.Should().BeOfType<ConsentPageResult>();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task ProcessAsync_post_to_entry_point_should_return_405()
        {
            _context.Request.Method = "POST";

            var result = await _subject.ProcessAsync(_context);

            var statusCode = result as StatusCodeResult;
            statusCode.Should().NotBeNull();
            statusCode.StatusCode.Should().Be(405);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task ProcessAsync_valid_consent_message_should_cleanup_consent_cookie()
        {
            var parameters = new NameValueCollection()
            {
                { "client_id", "client" },
                { "nonce", "some_nonce" },
                { "scope", "api1 api2" }
            };
            var request = new ConsentRequest(parameters, _user.GetSubjectId());
            _mockUserConsentResponseMessageStore.Messages.Add(request.Id, new Message<ConsentResponse>(new ConsentResponse() { ScopesValuesConsented = new string[] { "api1", "api2" } }));

            _mockUserSession.User = _user;

            _context.Request.Method = "GET";
            _context.Request.Path = new PathString("/connect/authorize/callback");
            _context.Request.QueryString = new QueryString("?" + parameters.ToQueryString());

            var result = await _subject.ProcessAsync(_context);

            _mockUserConsentResponseMessageStore.Messages.Count.Should().Be(0);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task ProcessAsync_valid_consent_message_should_return_authorize_result()
        {
            var parameters = new NameValueCollection()
            {
                { "client_id", "client" },
                { "nonce", "some_nonce" },
                { "scope", "api1 api2" }
            };
            var request = new ConsentRequest(parameters, _user.GetSubjectId());
            _mockUserConsentResponseMessageStore.Messages.Add(request.Id, new Message<ConsentResponse>(new ConsentResponse() { ScopesValuesConsented = new string[] { "api1", "api2" } }));

            _mockUserSession.User = _user;

            _context.Request.Method = "GET";
            _context.Request.Path = new PathString("/connect/authorize/callback");
            _context.Request.QueryString = new QueryString("?" + parameters.ToQueryString());

            var result = await _subject.ProcessAsync(_context);

            result.Should().BeOfType<AuthorizeResult>();
        }

        internal void Init()
        {
            _context = new MockHttpContextAccessor().HttpContext;

            _validatedAuthorizeRequest = new ValidatedAuthorizeRequest()
            {
                RedirectUri = "http://client/callback",
                State = "123",
                ResponseMode = "fragment",
                ClientId = "client",
                Client = new Client
                {
                    ClientId = "client",
                    ClientName = "Test Client"
                },
                Raw = _params,
                Subject = _user
            };
            _stubAuthorizeResponseGenerator.Response.Request = _validatedAuthorizeRequest;

            _stubAuthorizeRequestValidator.Result = new AuthorizeRequestValidationResult(_validatedAuthorizeRequest);

            _subject = new AuthorizeCallbackEndpoint(
                _fakeEventService,
                _fakeLogger,
                _options,
                _stubAuthorizeRequestValidator,
                _stubInteractionGenerator,
                _stubAuthorizeResponseGenerator,
                _mockUserSession,
                _mockUserConsentResponseMessageStore);
        }
    }
}
