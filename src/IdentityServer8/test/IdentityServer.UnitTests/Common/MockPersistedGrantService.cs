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
using System.Threading.Tasks;
using IdentityServer8.Models;
using IdentityServer8.Services;

namespace IdentityServer.UnitTests.Common
{
    public class MockPersistedGrantService : IPersistedGrantService
    {
        public IEnumerable<Grant> GetAllGrantsResult { get; set; }
        public bool RemoveAllGrantsWasCalled { get; set; }

        public Task<IEnumerable<Grant>> GetAllGrantsAsync(string subjectId)
        {
            return Task.FromResult(GetAllGrantsResult ?? Enumerable.Empty<Grant>());
        }

        public Task RemoveAllGrantsAsync(string subjectId, string clientId, string sessionId = null)
        {
            RemoveAllGrantsWasCalled = true;
            return Task.CompletedTask;
        }
    }
}
