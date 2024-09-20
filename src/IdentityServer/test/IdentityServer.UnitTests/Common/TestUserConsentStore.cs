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
using IdentityServer8.Stores;
using IdentityServer8.Stores.Serialization;

namespace IdentityServer.UnitTests.Common
{
    public class TestUserConsentStore : IUserConsentStore
    {
        private DefaultUserConsentStore _userConsentStore;
        private InMemoryPersistedGrantStore _grantStore = new InMemoryPersistedGrantStore();

        public TestUserConsentStore()
        {
            _userConsentStore = new DefaultUserConsentStore(
               _grantStore,
               new PersistentGrantSerializer(),
                new DefaultHandleGenerationService(),
               TestLogger.Create<DefaultUserConsentStore>());
        }

        public Task StoreUserConsentAsync(Consent consent)
        {
            return _userConsentStore.StoreUserConsentAsync(consent);
        }

        public Task<Consent> GetUserConsentAsync(string subjectId, string clientId)
        {
            return _userConsentStore.GetUserConsentAsync(subjectId, clientId);
        }

        public Task RemoveUserConsentAsync(string subjectId, string clientId)
        {
            return _userConsentStore.RemoveUserConsentAsync(subjectId, clientId);
        }
    }
}
