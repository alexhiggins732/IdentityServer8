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
using FluentAssertions;
using IdentityServer8.Configuration;
using IdentityServer8.Models;
using IdentityServer8.Stores;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IdentityServer.UnitTests.Extensions
{
    public class IdentityServerBuilderExtensionsCacheStoreTests
    {
        private class CustomClientStore: IClientStore
        {
            public Task<Client> FindClientByIdAsync(string clientId)
            {
                throw new System.NotImplementedException();
            }
        }

        private class CustomResourceStore : IResourceStore
        {
            public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
            {
                throw new System.NotImplementedException();
            }

            public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
            {
                throw new System.NotImplementedException();
            }

            public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> names)
            {
                throw new System.NotImplementedException();
            }

            public Task<Resources> GetAllResourcesAsync()
            {
                throw new System.NotImplementedException();
            }

            public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
            {
                throw new System.NotImplementedException();
            }
        }

        [Fact]
        public void AddClientStoreCache_should_add_concrete_iclientstore_implementation()
        {
            var services = new ServiceCollection();
            var identityServerBuilder = new IdentityServerBuilder(services);

            identityServerBuilder.AddClientStoreCache<CustomClientStore>();

            services.Any(x => x.ImplementationType == typeof(CustomClientStore)).Should().BeTrue();
        }

        [Fact]
        public void AddResourceStoreCache_should_attempt_to_register_iresourcestore_implementation()
        {
            var services = new ServiceCollection();
            var identityServerBuilder = new IdentityServerBuilder(services);

            identityServerBuilder.AddResourceStoreCache<CustomResourceStore>();

            services.Any(x => x.ImplementationType == typeof(CustomResourceStore)).Should().BeTrue();
        }
    }
}
