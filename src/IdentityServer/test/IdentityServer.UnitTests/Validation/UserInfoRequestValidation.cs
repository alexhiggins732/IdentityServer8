/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using FluentAssertions;
using IdentityModel;
using IdentityServer.UnitTests.Validation.Setup;
using IdentityServer.Stores;
using IdentityServer.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.UnitTests.Common;
using Xunit;

namespace IdentityServer.UnitTests.Validation
{
    public class UserInfoRequestValidation
    {
        private const string Category = "UserInfo Request Validation Tests";
        private IClientStore _clients = new InMemoryClientStore(TestClients.Get());

        [Fact]
        [Trait("Category", Category)]
        public async Task token_without_sub_should_fail()
        {
            var tokenResult = new TokenValidationResult
            {
                IsError = false,
                Client = await _clients.FindEnabledClientByIdAsync("codeclient"),
                Claims = new List<Claim>()
            };

            var validator = new UserInfoRequestValidator(
                new TestTokenValidator(tokenResult),
                new TestProfileService(shouldBeActive: true),
                TestLogger.Create<UserInfoRequestValidator>());

            var result = await validator.ValidateRequestAsync("token");

            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.ProtectedResourceErrors.InvalidToken);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task active_user_should_succeed()
        {
            var tokenResult = new TokenValidationResult
            {
                IsError = false,
                Client = await _clients.FindEnabledClientByIdAsync("codeclient"),
                Claims = new List<Claim>
                {
                    new Claim("sub", "123")
                },
            };

            var validator = new UserInfoRequestValidator(
                new TestTokenValidator(tokenResult),
                new TestProfileService(shouldBeActive: true),
                TestLogger.Create<UserInfoRequestValidator>());

            var result = await validator.ValidateRequestAsync("token");

            result.IsError.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task inactive_user_should_fail()
        {
            var tokenResult = new TokenValidationResult
            {
                IsError = false,
                Client = await _clients.FindEnabledClientByIdAsync("codeclient"),
                Claims = new List<Claim>
                {
                    new Claim("sub", "123")
                },
            };

            var validator = new UserInfoRequestValidator(
                new TestTokenValidator(tokenResult),
                new TestProfileService(shouldBeActive: false),
                TestLogger.Create<UserInfoRequestValidator>());

            var result = await validator.ValidateRequestAsync("token");

            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.ProtectedResourceErrors.InvalidToken);
        }
    }
}
