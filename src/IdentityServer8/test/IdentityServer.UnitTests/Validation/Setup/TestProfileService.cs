/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Threading.Tasks;
using IdentityServer8.Models;
using IdentityServer8.Services;

namespace IdentityServer.UnitTests.Validation.Setup
{
    internal class TestProfileService : IProfileService
    {
        private bool _shouldBeActive;

        public TestProfileService(bool shouldBeActive = true)
        {
            _shouldBeActive = shouldBeActive;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = _shouldBeActive;
            return Task.CompletedTask;
        }
    }
}
