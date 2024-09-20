/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Collections.Generic;
using IdentityServer8.Models;

namespace IdentityServer.IntegrationTests.Endpoints.Introspection.Setup;

internal class Clients
{
    public static IEnumerable<Client> Get()
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "client1",
                ClientSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api1", "api2", "api3-a", "api3-b" },
                AccessTokenType = AccessTokenType.Reference
            },
            new Client
            {
                ClientId = "client2",
                ClientSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api1", "api2", "api3-a", "api3-b" },
                AccessTokenType = AccessTokenType.Reference
            },
            new Client
            {
                ClientId = "client3",
                ClientSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api1", "api2", "api3-a", "api3-b" },
                AccessTokenType = AccessTokenType.Reference
            },
            new Client
            {
                ClientId = "ro.client",
                ClientSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = { "api1", "api2", "api3-a", "api3-b" },
                AccessTokenType = AccessTokenType.Reference
            }
        };
    }
}
