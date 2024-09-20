/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer.UnitTests.Common;
using IdentityServer8;
using IdentityServer8.Configuration;
using IdentityServer8.Models;
using IdentityServer8.ResponseHandling;
using IdentityServer8.Services;
using IdentityServer8.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Xunit;
using static IdentityModel.OidcConstants;

namespace IdentityServer.UnitTests.ResponseHandling.AuthorizeInteractionResponseGenerator
{
    public class CustomAuthorizeInteractionResponseGenerator : IdentityServer8.ResponseHandling.AuthorizeInteractionResponseGenerator
    {
        public CustomAuthorizeInteractionResponseGenerator(ISystemClock clock, ILogger<IdentityServer8.ResponseHandling.AuthorizeInteractionResponseGenerator> logger, IConsentService consent, IProfileService profile) : base(clock, logger, consent, profile)
        {
        }

        public InteractionResponse ProcessLoginResponse { get; set; }
        protected internal override Task<InteractionResponse> ProcessLoginAsync(ValidatedAuthorizeRequest request)
        {
            if (ProcessLoginResponse != null)
            {
                return Task.FromResult(ProcessLoginResponse);
            }

            return base.ProcessLoginAsync(request);
        }

        public InteractionResponse ProcessConsentResponse { get; set; }
        protected internal override Task<InteractionResponse> ProcessConsentAsync(ValidatedAuthorizeRequest request, ConsentResponse consent = null)
        {
            if (ProcessConsentResponse != null)
            {
                return Task.FromResult(ProcessConsentResponse);
            }
            return base.ProcessConsentAsync(request, consent);
        }
    }

    public class AuthorizeInteractionResponseGeneratorTests_Custom
    {
        private IdentityServerOptions _options = new IdentityServerOptions();
        private CustomAuthorizeInteractionResponseGenerator _subject;
        private MockConsentService _mockConsentService = new MockConsentService();
        private StubClock _clock = new StubClock();

        public AuthorizeInteractionResponseGeneratorTests_Custom()
        {
            _subject = new CustomAuthorizeInteractionResponseGenerator(
                _clock,
                TestLogger.Create<IdentityServer8.ResponseHandling.AuthorizeInteractionResponseGenerator>(),
                _mockConsentService,
                new MockProfileService());
        }


        [Fact]
        public async Task ProcessInteractionAsync_with_overridden_login_returns_redirect_should_return_redirect()
        {
            var request = new ValidatedAuthorizeRequest
            {
                ClientId = "foo",
                Subject = new IdentityServerUser("123")
                {
                    IdentityProvider = IdentityServerConstants.LocalIdentityProvider
                }.CreatePrincipal(),
                Client = new Client
                {
                },
            };

            _subject.ProcessLoginResponse = new InteractionResponse
            {
                RedirectUrl = "/custom"
            };

            var result = await _subject.ProcessInteractionAsync(request);

            result.IsRedirect.Should().BeTrue();
            result.RedirectUrl.Should().Be("/custom");
        }

        [Fact]
        public async Task ProcessInteractionAsync_with_prompt_none_and_login_returns_login_should_return_error()
        {
            var request = new ValidatedAuthorizeRequest
            {
                ClientId = "foo",
                Subject = new IdentityServerUser("123")
                {
                    IdentityProvider = IdentityServerConstants.LocalIdentityProvider
                }.CreatePrincipal(),
                Client = new Client
                {
                },
                PromptModes = new[] { PromptModes.None },
            };

            _subject.ProcessLoginResponse = new InteractionResponse
            {
                IsLogin = true
            };

            var result = await _subject.ProcessInteractionAsync(request);

            result.IsError.Should().BeTrue();
            result.Error.Should().Be("login_required");
        }

        [Fact]
        public async Task ProcessInteractionAsync_with_prompt_none_and_login_returns_redirect_should_return_error()
        {
            var request = new ValidatedAuthorizeRequest
            {
                ClientId = "foo",
                Subject = new IdentityServerUser("123")
                {
                    IdentityProvider = IdentityServerConstants.LocalIdentityProvider
                }.CreatePrincipal(),
                Client = new Client
                {
                },
                PromptModes = new[] { PromptModes.None },
            };

            _subject.ProcessLoginResponse = new InteractionResponse
            {
                RedirectUrl = "/custom"
            };

            var result = await _subject.ProcessInteractionAsync(request);

            result.IsError.Should().BeTrue();
            result.Error.Should().Be("interaction_required");
            result.RedirectUrl.Should().BeNull();
        }

        [Fact]
        public async Task ProcessInteractionAsync_with_prompt_none_and_consent_returns_consent_should_return_error()
        {
            var request = new ValidatedAuthorizeRequest
            {
                ClientId = "foo",
                Subject = new IdentityServerUser("123")
                {
                    IdentityProvider = IdentityServerConstants.LocalIdentityProvider
                }.CreatePrincipal(),
                Client = new Client
                {
                },
                PromptModes = new[] { PromptModes.None },
            };

            _subject.ProcessConsentResponse = new InteractionResponse
            {
                IsConsent = true
            };

            var result = await _subject.ProcessInteractionAsync(request);

            result.IsError.Should().BeTrue();
            result.Error.Should().Be("consent_required");
        }
    }
}
