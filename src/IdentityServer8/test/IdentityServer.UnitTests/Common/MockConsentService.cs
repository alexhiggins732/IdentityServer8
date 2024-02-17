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
using IdentityServer8.Validation;

namespace IdentityServer.UnitTests.Common
{
    public class MockConsentService : IConsentService
    {
        public bool RequiresConsentResult { get; set; }

        public Task<bool> RequiresConsentAsync(ClaimsPrincipal subject, Client client, IEnumerable<ParsedScopeValue> parsedScopes)
        {
            return Task.FromResult(RequiresConsentResult);
        }

        public ClaimsPrincipal ConsentSubject { get; set; }
        public Client ConsentClient { get; set; }
        public IEnumerable<string> ConsentScopes { get; set; }

        public Task UpdateConsentAsync(ClaimsPrincipal subject, Client client, IEnumerable<ParsedScopeValue> parsedScopes)
        {
            ConsentSubject = subject;
            ConsentClient = client;
            ConsentScopes = parsedScopes?.Select(x => x.RawValue);

            return Task.CompletedTask;
        }
    }
}
