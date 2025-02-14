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
using System.Collections.Specialized;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel;
using  IdentityServer.Common;
using  IdentityServer.Validation.Setup;
using IdentityServer8.Configuration;
using IdentityServer8.Models;
using IdentityServer8.Stores;
using Xunit;

namespace  IdentityServer.Validation.TokenRequest_Validation
{
    public class TokenRequestValidation_RefreshToken_Invalid
    {
        private const string Category = "TokenRequest Validation - RefreshToken - Invalid";

        private IClientStore _clients = Factory.CreateClientStore();

        [Fact]
        [Trait("Category", Category)]
        public async Task Non_existing_RefreshToken()
        {
            var client = await _clients.FindEnabledClientByIdAsync("roclient");

            var validator = Factory.CreateTokenRequestValidator();

            var parameters = new NameValueCollection();
            parameters.Add(OidcConstants.TokenRequest.GrantType, "refresh_token");
            parameters.Add(OidcConstants.TokenRequest.RefreshToken, "nonexistent");

            var result = await validator.ValidateRequestAsync(parameters, client.ToValidationResult());

            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.TokenErrors.InvalidGrant);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task RefreshTokenTooLong()
        {
            var client = await _clients.FindEnabledClientByIdAsync("roclient");
            var options = new IdentityServerOptions();

            var validator = Factory.CreateTokenRequestValidator();
            var longRefreshToken = "x".Repeat(options.InputLengthRestrictions.RefreshToken + 1);

            var parameters = new NameValueCollection();
            parameters.Add(OidcConstants.TokenRequest.GrantType, "refresh_token");
            parameters.Add(OidcConstants.TokenRequest.RefreshToken, longRefreshToken);

            var result = await validator.ValidateRequestAsync(parameters, client.ToValidationResult());

            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.TokenErrors.InvalidGrant);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task Expired_RefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                AccessToken = new Token("access_token") { ClientId = "roclient" },
                Lifetime = 10,
                CreationTime = DateTime.UtcNow.AddSeconds(-15)
            };

            var grants = Factory.CreateRefreshTokenStore();
            var handle = await grants.StoreRefreshTokenAsync(refreshToken);

            var client = await _clients.FindEnabledClientByIdAsync("roclient");

            var validator = Factory.CreateTokenRequestValidator(
                refreshTokenStore: grants);

            var parameters = new NameValueCollection();
            parameters.Add(OidcConstants.TokenRequest.GrantType, "refresh_token");
            parameters.Add(OidcConstants.TokenRequest.RefreshToken, handle);

            var result = await validator.ValidateRequestAsync(parameters, client.ToValidationResult());

            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.TokenErrors.InvalidGrant);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task Wrong_Client_Binding_RefreshToken_Request()
        {
            var refreshToken = new RefreshToken
            {
                AccessToken = new Token("access_token")
                {
                    ClientId = "otherclient",
                    Lifetime = 600,
                    CreationTime = DateTime.UtcNow
                }
            };

            var grants = Factory.CreateRefreshTokenStore();
            var handle = await grants.StoreRefreshTokenAsync(refreshToken);

            var client = await _clients.FindEnabledClientByIdAsync("roclient");

            var validator = Factory.CreateTokenRequestValidator(
                refreshTokenStore: grants);

            var parameters = new NameValueCollection();
            parameters.Add(OidcConstants.TokenRequest.GrantType, "refresh_token");
            parameters.Add(OidcConstants.TokenRequest.RefreshToken, handle);

            var result = await validator.ValidateRequestAsync(parameters, client.ToValidationResult());

            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.TokenErrors.InvalidGrant);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task Client_has_no_OfflineAccess_Scope_anymore_at_RefreshToken_Request()
        {
            var refreshToken = new RefreshToken
            {
                AccessToken = new Token("access_token")
                {
                    ClientId = "roclient_restricted"
                },
                Lifetime = 600,
                CreationTime = DateTime.UtcNow
            };

            var grants = Factory.CreateRefreshTokenStore();
            var handle = await grants.StoreRefreshTokenAsync(refreshToken);

            var client = await _clients.FindEnabledClientByIdAsync("roclient_restricted");

            var validator = Factory.CreateTokenRequestValidator(
                refreshTokenStore: grants);

            var parameters = new NameValueCollection();
            parameters.Add(OidcConstants.TokenRequest.GrantType, "refresh_token");
            parameters.Add(OidcConstants.TokenRequest.RefreshToken, handle);

            var result = await validator.ValidateRequestAsync(parameters, client.ToValidationResult());

            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.TokenErrors.InvalidGrant);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task RefreshToken_Request_with_disabled_User()
        {
            var subjectClaim = new Claim(JwtClaimTypes.Subject, "foo");

            var refreshToken = new RefreshToken
            {
                AccessToken = new Token("access_token")
                {
                    Claims = new List<Claim> { subjectClaim },
                    ClientId = "roclient"
                },
                Lifetime = 600,
                CreationTime = DateTime.UtcNow
            };

            var grants = Factory.CreateRefreshTokenStore();
            var handle = await grants.StoreRefreshTokenAsync(refreshToken);

            var client = await _clients.FindEnabledClientByIdAsync("roclient");

            var validator = Factory.CreateTokenRequestValidator(
                refreshTokenStore: grants,
                profile: new TestProfileService(shouldBeActive: false));

            var parameters = new NameValueCollection();
            parameters.Add(OidcConstants.TokenRequest.GrantType, "refresh_token");
            parameters.Add(OidcConstants.TokenRequest.RefreshToken, handle);

            var result = await validator.ValidateRequestAsync(parameters, client.ToValidationResult());

            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.TokenErrors.InvalidGrant);
        }
    }
}
