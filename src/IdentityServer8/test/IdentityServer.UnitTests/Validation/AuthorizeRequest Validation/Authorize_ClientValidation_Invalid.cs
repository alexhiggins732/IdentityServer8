/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Collections.Specialized;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel;
using IdentityServer.UnitTests.Common;
using IdentityServer.UnitTests.Validation.Setup;
using IdentityServer8.Configuration;
using Xunit;

namespace IdentityServer.UnitTests.Validation.AuthorizeRequest_Validation
{
    public class Authorize_ClientValidation_Invalid
    {
        private const string Category = "AuthorizeRequest Client Validation - Invalid";

        private IdentityServerOptions _options = TestIdentityServerOptions.Create();

        [Fact]
        [Trait("Category", Category)]
        public async Task Invalid_Protocol_Client()
        {
            var parameters = new NameValueCollection();
            parameters.Add(OidcConstants.AuthorizeRequest.ClientId, "wsfed");
            parameters.Add(OidcConstants.AuthorizeRequest.Scope, "openid");
            parameters.Add(OidcConstants.AuthorizeRequest.RedirectUri, "https://wsfed/callback");
            parameters.Add(OidcConstants.AuthorizeRequest.ResponseType, OidcConstants.ResponseTypes.IdToken);

            var validator = Factory.CreateAuthorizeRequestValidator();
            var result = await validator.ValidateAsync(parameters);

            result.IsError.Should().BeTrue();
            result.Error.Should().Be(OidcConstants.AuthorizeErrors.UnauthorizedClient);
        }
    }
}
