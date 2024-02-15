/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer8.Models;
using IdentityServer8.Services;

namespace IdentityServer.UnitTests.Common
{
    public class MockProfileService : IProfileService
    {
        public ICollection<Claim> ProfileClaims { get; set; } = new HashSet<Claim>();
        public bool IsActive { get; set; } = true;

        public bool GetProfileWasCalled => ProfileContext != null;
        public ProfileDataRequestContext ProfileContext { get; set; }

        public bool IsActiveWasCalled => ActiveContext != null;
        public IsActiveContext ActiveContext { get; set; }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            ProfileContext = context;
            context.IssuedClaims = ProfileClaims.ToList();
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            ActiveContext = context;
            context.IsActive = IsActive;
            return Task.CompletedTask;
        }
    }
}
