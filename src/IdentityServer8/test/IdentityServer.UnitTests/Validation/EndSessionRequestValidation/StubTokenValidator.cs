/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Threading.Tasks;
using IdentityServer8.Models;
using IdentityServer8.Validation;

namespace IdentityServer.UnitTests.Validation.EndSessionRequestValidation
{
    public class StubTokenValidator : ITokenValidator
    {
        public TokenValidationResult AccessTokenValidationResult { get; set; } = new TokenValidationResult();
        public TokenValidationResult IdentityTokenValidationResult { get; set; } = new TokenValidationResult();

        public Task<TokenValidationResult> ValidateAccessTokenAsync(string token, string expectedScope = null)
        {
            return Task.FromResult(AccessTokenValidationResult);
        }

        public Task<TokenValidationResult> ValidateIdentityTokenAsync(string token, string clientId = null, bool validateLifetime = true)
        {
            return Task.FromResult(IdentityTokenValidationResult);
        }

        public Task<TokenValidationResult> ValidateRefreshTokenAsync(string token, Client client)
        {
            throw new System.NotImplementedException();
        }
    }
}
