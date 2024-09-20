/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.UnitTests.Common
{
    public class MockUserSession : IUserSession
    {
        public List<string> Clients = new List<string>();

        public bool EnsureSessionIdCookieWasCalled { get; set; }
        public bool RemoveSessionIdCookieWasCalled { get; set; }
        public bool CreateSessionIdWasCalled { get; set; }

        public ClaimsPrincipal User { get; set; }
        public string SessionId { get; set; }
        public AuthenticationProperties Properties { get; set; }


        public Task<string> CreateSessionIdAsync(ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            CreateSessionIdWasCalled = true;
            User = principal;
            SessionId = Guid.NewGuid().ToString();
            return Task.FromResult(SessionId);
        }

        public Task<ClaimsPrincipal> GetUserAsync()
        {
            return Task.FromResult(User);
        }

        Task<string> IUserSession.GetSessionIdAsync()
        {
            return Task.FromResult(SessionId);
        }

        public Task EnsureSessionIdCookieAsync()
        {
            EnsureSessionIdCookieWasCalled = true;
            return Task.CompletedTask;
        }

        public Task RemoveSessionIdCookieAsync()
        {
            RemoveSessionIdCookieWasCalled = true;
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetClientListAsync()
        {
            return Task.FromResult<IEnumerable<string>>(Clients);
        }

        public Task AddClientIdAsync(string clientId)
        {
            Clients.Add(clientId);
            return Task.CompletedTask;
        }
    }
}
