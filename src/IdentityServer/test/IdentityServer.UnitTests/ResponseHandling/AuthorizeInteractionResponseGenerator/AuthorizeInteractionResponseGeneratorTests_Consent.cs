/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel;
using IdentityServer.UnitTests.Common;
using IdentityServer8.Configuration;
using IdentityServer8.Models;
using IdentityServer8.Validation;
using Xunit;

namespace IdentityServer.UnitTests.ResponseHandling.AuthorizeInteractionResponseGenerator
{
    public class AuthorizeInteractionResponseGeneratorTests_Consent
    {
        private IdentityServer8.ResponseHandling.AuthorizeInteractionResponseGenerator _subject;
        private IdentityServerOptions _options = new IdentityServerOptions();
        private MockConsentService _mockConsent = new MockConsentService();
        private MockProfileService _fakeUserService = new MockProfileService();

        private void RequiresConsent(bool value)
        {
            _mockConsent.RequiresConsentResult = value;
        }

        private void AssertUpdateConsentNotCalled()
        {
            _mockConsent.ConsentClient.Should().BeNull();
            _mockConsent.ConsentSubject.Should().BeNull();
            _mockConsent.ConsentScopes.Should().BeNull();
        }

        private void AssertUpdateConsentCalled(Client client, ClaimsPrincipal user, params string[] scopes)
        {
            _mockConsent.ConsentClient.Should().BeSameAs(client);
            _mockConsent.ConsentSubject.Should().BeSameAs(user);
            _mockConsent.ConsentScopes.Should().BeEquivalentTo(scopes);
        }

        private static IEnumerable<IdentityResource> GetIdentityScopes()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        private static IEnumerable<ApiResource> GetApiResources()
        {
            return new ApiResource[]
            {
                new ApiResource
                {
                    Name = "api",
                    Scopes = { "read", "write", "forbidden" }
                }
             };
        }

        private static IEnumerable<ApiScope> GetScopes()
        {
            return new ApiScope[]
            {
                new ApiScope
                {
                    Name = "read",
                    DisplayName = "Read data",
                    Emphasize = false
                },
                new ApiScope
                {
                    Name = "write",
                    DisplayName = "Write data",
                    Emphasize = true
                },
                new ApiScope
                {
                    Name = "forbidden",
                    DisplayName = "Forbidden scope",
                    Emphasize = true
                }
             };
        }

        public AuthorizeInteractionResponseGeneratorTests_Consent()
        {
            _subject = new IdentityServer8.ResponseHandling.AuthorizeInteractionResponseGenerator(
                new StubClock(),
                TestLogger.Create<IdentityServer8.ResponseHandling.AuthorizeInteractionResponseGenerator>(),
                _mockConsent,
                _fakeUserService);
        }

        private static ResourceValidationResult GetValidatedResources(params string[] scopes)
        {
            var resources = new Resources(GetIdentityScopes(), GetApiResources(), GetScopes());
            return new ResourceValidationResult(resources).Filter(scopes);
        }


        [Fact]
        public async Task ProcessConsentAsync_NullRequest_Throws()
        {
            Func<Task> act = async () => await _subject.ProcessConsentAsync(null, new ConsentResponse());

            (await act.Should().ThrowAsync<ArgumentNullException>())
                .And.ParamName.Should().Be("request");
        }

        [Fact]
        public async Task ProcessConsentAsync_AllowsNullConsent()
        {
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                PromptModes = new[] { OidcConstants.PromptModes.Consent },
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };
            await _subject.ProcessConsentAsync(request, null);
        }

        [Fact]
        public async Task ProcessConsentAsync_PromptModeIsLogin_Throws()
        {
            RequiresConsent(true);
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                PromptModes = new[] { OidcConstants.PromptModes.Login },
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };

            Func<Task> act = async () => await _subject.ProcessConsentAsync(request);

            (await act.Should().ThrowAsync<ArgumentException>())
                .And.Message.Should().Contain("PromptMode");
        }

        [Fact]
        public async Task ProcessConsentAsync_PromptModeIsSelectAccount_Throws()
        {
            RequiresConsent(true);
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                PromptModes = new[] { OidcConstants.PromptModes.SelectAccount },
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };

            Func<Task> act = async () => await _subject.ProcessConsentAsync(request);

            (await act.Should().ThrowAsync<ArgumentException>())
                 .And.Message.Should().Contain("PromptMode");
        }


        [Fact]
        public async Task ProcessConsentAsync_RequiresConsentButPromptModeIsNone_ReturnsErrorResult()
        {
            RequiresConsent(true);
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                PromptModes = new[] { OidcConstants.PromptModes.None },
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };
            var result = await _subject.ProcessConsentAsync(request);

            request.WasConsentShown.Should().BeFalse();
            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.AuthorizeErrors.ConsentRequired);
            AssertUpdateConsentNotCalled();
        }

        [Fact]
        public async Task ProcessConsentAsync_PromptModeIsConsent_NoPriorConsent_ReturnsConsentResult()
        {
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                PromptModes = new[] { OidcConstants.PromptModes.Consent },
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };
            var result = await _subject.ProcessConsentAsync(request);
            request.WasConsentShown.Should().BeFalse();
            result.IsConsent.Should().BeTrue();
            AssertUpdateConsentNotCalled();
        }

        [Fact]
        public async Task ProcessConsentAsync_NoPromptMode_ConsentServiceRequiresConsent_NoPriorConsent_ReturnsConsentResult()
        {
            RequiresConsent(true);
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                PromptModes = new[] { OidcConstants.PromptModes.Consent },
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };
            var result = await _subject.ProcessConsentAsync(request);
            request.WasConsentShown.Should().BeFalse();
            result.IsConsent.Should().BeTrue();
            AssertUpdateConsentNotCalled();
        }

        [Fact]
        public async Task ProcessConsentAsync_PromptModeIsConsent_ConsentNotGranted_ReturnsErrorResult()
        {
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                PromptModes = new[] { OidcConstants.PromptModes.Consent },
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };

            var consent = new ConsentResponse
            {
                RememberConsent = false,
                ScopesValuesConsented = new string[] { }
            };
            var result = await _subject.ProcessConsentAsync(request, consent);
            request.WasConsentShown.Should().BeTrue();
            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.AuthorizeErrors.AccessDenied);
            AssertUpdateConsentNotCalled();
        }

        [Fact]
        public async Task ProcessConsentAsync_NoPromptMode_ConsentServiceRequiresConsent_ConsentNotGranted_ReturnsErrorResult()
        {
            RequiresConsent(true);
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };
            var consent = new ConsentResponse
            {
                RememberConsent = false,
                ScopesValuesConsented = new string[] { }
            };
            var result = await _subject.ProcessConsentAsync(request, consent);
            request.WasConsentShown.Should().BeTrue();
            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.AuthorizeErrors.AccessDenied);
            AssertUpdateConsentNotCalled();
        }

        [Fact]
        public async Task ProcessConsentAsync_NoPromptMode_ConsentServiceRequiresConsent_ConsentGrantedButMissingRequiredScopes_ReturnsErrorResult()
        {
            RequiresConsent(true);
            var client = new Client { };
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
                Client = client
            };

            var consent = new ConsentResponse
            {
                RememberConsent = false,
                ScopesValuesConsented = new string[] { "read" }
            };

            var result = await _subject.ProcessConsentAsync(request, consent);
            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.AuthorizeErrors.AccessDenied);
            AssertUpdateConsentNotCalled();
        }

        [Fact]
        public async Task ProcessConsentAsync_NoPromptMode_ConsentServiceRequiresConsent_ConsentGranted_ScopesSelected_ReturnsConsentResult()
        {
            RequiresConsent(true);
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                Client = new Client
                {
                    AllowRememberConsent = false
                },
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };
            var consent = new ConsentResponse
            {
                RememberConsent = false,
                ScopesValuesConsented = new string[] { "openid", "read" }
            };
            var result = await _subject.ProcessConsentAsync(request, consent);
            request.ValidatedResources.Resources.IdentityResources.Count().Should().Be(1);
            request.ValidatedResources.Resources.ApiScopes.Count().Should().Be(1);
            "openid".Should().Be(request.ValidatedResources.Resources.IdentityResources.Select(x => x.Name).First());
            "read".Should().Be(request.ValidatedResources.Resources.ApiScopes.First().Name);
            request.WasConsentShown.Should().BeTrue();
            result.IsConsent.Should().BeFalse();
            AssertUpdateConsentNotCalled();
        }

        [Fact]
        public async Task ProcessConsentAsync_PromptModeConsent_ConsentGranted_ScopesSelected_ReturnsConsentResult()
        {
            RequiresConsent(true);
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                Client = new Client
                {
                    AllowRememberConsent = false
                },
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };
            var consent = new ConsentResponse
            {
                RememberConsent = false,
                ScopesValuesConsented = new string[] { "openid", "read" }
            };
            var result = await _subject.ProcessConsentAsync(request, consent);
            request.ValidatedResources.Resources.IdentityResources.Count().Should().Be(1);
            request.ValidatedResources.Resources.ApiScopes.Count().Should().Be(1);
            "read".Should().Be(request.ValidatedResources.Resources.ApiScopes.First().Name);
            request.WasConsentShown.Should().BeTrue();
            result.IsConsent.Should().BeFalse();
            AssertUpdateConsentNotCalled();
        }

        [Fact]
        public async Task ProcessConsentAsync_AllowConsentSelected_SavesConsent()
        {
            RequiresConsent(true);
            var client = new Client { AllowRememberConsent = true };
            var user = new ClaimsPrincipal();
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                Client = client,
                Subject = user,
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };
            var consent = new ConsentResponse
            {
                RememberConsent = true,
                ScopesValuesConsented = new string[] { "openid", "read" }
            };
            var result = await _subject.ProcessConsentAsync(request, consent);
            AssertUpdateConsentCalled(client, user, "openid", "read");
        }

        [Fact]
        public async Task ProcessConsentAsync_NotRememberingConsent_DoesNotSaveConsent()
        {
            RequiresConsent(true);
            var client = new Client { AllowRememberConsent = true };
            var user = new ClaimsPrincipal();
            var request = new ValidatedAuthorizeRequest()
            {
                ResponseMode = OidcConstants.ResponseModes.Fragment,
                State = "12345",
                RedirectUri = "https://client.com/callback",
                Client = client,
                Subject = user,
                RequestedScopes = new List<string> { "openid", "read", "write" },
                ValidatedResources = GetValidatedResources("openid", "read", "write"),
            };
            var consent = new ConsentResponse
            {
                RememberConsent = false,
                ScopesValuesConsented = new string[] { "openid", "read" }
            };
            var result = await _subject.ProcessConsentAsync(request, consent);
            AssertUpdateConsentCalled(client, user);
        }
    }
}
