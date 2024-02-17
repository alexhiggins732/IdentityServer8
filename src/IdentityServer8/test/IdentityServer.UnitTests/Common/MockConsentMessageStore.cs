/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer8.Models;
using IdentityServer8.Stores;

namespace IdentityServer.UnitTests.Common
{
    public class MockConsentMessageStore : IConsentMessageStore
    {
        public Dictionary<string, Message<ConsentResponse>> Messages { get; set; } = new Dictionary<string, Message<ConsentResponse>>();

        public Task DeleteAsync(string id)
        {
            if (id != null && Messages.ContainsKey(id))
            {
                Messages.Remove(id);
            }
            return Task.CompletedTask;
        }

        public Task<Message<ConsentResponse>> ReadAsync(string id)
        {
            Message<ConsentResponse> val = null;
            if (id != null)
            {
                Messages.TryGetValue(id, out val);
            }
            return Task.FromResult(val);
        }

        public Task WriteAsync(string id, Message<ConsentResponse> message)
        {
            Messages[id] = message;
            return Task.CompletedTask;
        }
    }
}
