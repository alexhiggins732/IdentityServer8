/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.Services;
using IdentityServer8.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.UnitTests.Common
{
    class MockClaimsService : IClaimsService
    {
        public List<Claim> IdentityTokenClaims { get; set; } = new List<Claim>();
        public List<Claim> AccessTokenClaims { get; set; } = new List<Claim>();

        public Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ClaimsPrincipal subject, ResourceValidationResult resources, bool includeAllIdentityClaims, ValidatedRequest request)
        {
            return Task.FromResult(IdentityTokenClaims.AsEnumerable());
        }

        public Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, ResourceValidationResult resources, ValidatedRequest request)
        {
            return Task.FromResult(AccessTokenClaims.AsEnumerable());
        }
    }
}
