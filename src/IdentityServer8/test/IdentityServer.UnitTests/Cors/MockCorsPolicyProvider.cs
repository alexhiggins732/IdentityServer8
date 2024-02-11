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
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.UnitTests.Cors
{
    public class MockCorsPolicyProvider : ICorsPolicyProvider
    {
        public bool WasCalled { get; set; }
        public CorsPolicy Response { get; set; }

        public Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
        {
            WasCalled = true;
            return Task.FromResult(Response);
        }
    }
}
