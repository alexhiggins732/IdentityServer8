/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using FluentAssertions;
using IdentityServer.UnitTests.Common;
using IdentityServer8.Models;
using IdentityServer8.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.UnitTests.Validation
{
    public class StrictRedirectUriValidatorAppAuthValidation
    {
        private const string Category = "Strict Redirect Uri Validator AppAuth Validation Tests";

        private Client clientWithValidLoopbackRedirectUri = new Client
        {
            RequirePkce = true,
            RedirectUris = new List<string>
            {
                "http://127.0.0.1"
            }
        };

        private Client clientWithNoRedirectUris = new Client
        {
            RequirePkce = true
        };

        [Theory]
        [Trait("Category", Category)]
        [InlineData("http://127.0.0.1")] // This is in the clients redirect URIs
        [InlineData("http://127.0.0.1:0")]
        [InlineData("http://127.0.0.1:80")]
        [InlineData("http://127.0.0.1:65535")]
        [InlineData("http://127.0.0.1:123/a/b")]
        [InlineData("http://127.0.0.1:123?q=123")]
        [InlineData("http://127.0.0.1:443/?q=123")]
        [InlineData("http://127.0.0.1:443/a/b?q=123")]
        [InlineData("http://127.0.0.1:443#abc")]
        [InlineData("http://127.0.0.1:443/a/b?q=123#abc")]
        public async Task Loopback_Redirect_URIs_Should_Be_AllowedAsync(string requestedUri)
        {
            var strictRedirectUriValidatorAppAuthValidator = new StrictRedirectUriValidatorAppAuth(TestLogger.Create<StrictRedirectUriValidatorAppAuth>());

            var result = await strictRedirectUriValidatorAppAuthValidator.IsRedirectUriValidAsync(requestedUri, clientWithValidLoopbackRedirectUri);

            result.Should().BeTrue();
        }

        [Theory]
        [Trait("Category", Category)]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("http:")]
        [InlineData("127.0.0.1")]
        [InlineData("//127.0.0.1")]
        [InlineData("https://127.0.0.1:123")]
        [InlineData("http://127.0.0.1:")]
        [InlineData("http://127.0.0.1:-1")]
        [InlineData("http://127.0.0.2:65536")]
        [InlineData("http://127.0.0.1:443a")]
        [InlineData("http://127.0.0.1:a443")]
        [InlineData("http://127.0.0.1:443a/")]
        [InlineData("http://127.0.0.1:443a?")]
        [InlineData("http://127.0.0.2:443")]
        [InlineData("http://127.0.0.1#abc")]
        [InlineData("http://127.0.0.1:#abc")]
        public async Task Loopback_Redirect_URIs_Should_Not_Be_AllowedAsync(string requestedUri)
        {
            var strictRedirectUriValidatorAppAuthValidator = new StrictRedirectUriValidatorAppAuth(TestLogger.Create<StrictRedirectUriValidatorAppAuth>());

            var result = await strictRedirectUriValidatorAppAuthValidator.IsRedirectUriValidAsync(requestedUri, clientWithValidLoopbackRedirectUri);

            result.Should().BeFalse();
        }
    }
}
