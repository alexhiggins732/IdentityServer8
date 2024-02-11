/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Collections.Specialized;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer8.Validation;

namespace IdentityServer.UnitTests.Endpoints.EndSession
{
    class StubEndSessionRequestValidator : IEndSessionRequestValidator
    {
        public EndSessionValidationResult EndSessionValidationResult { get; set; } = new EndSessionValidationResult();
        public EndSessionCallbackValidationResult EndSessionCallbackValidationResult { get; set; } = new EndSessionCallbackValidationResult();

        public Task<EndSessionValidationResult> ValidateAsync(NameValueCollection parameters, ClaimsPrincipal subject)
        {
            return Task.FromResult(EndSessionValidationResult);
        }

        public Task<EndSessionCallbackValidationResult> ValidateCallbackAsync(NameValueCollection parameters)
        {
            return Task.FromResult(EndSessionCallbackValidationResult);
        }
    }
}
