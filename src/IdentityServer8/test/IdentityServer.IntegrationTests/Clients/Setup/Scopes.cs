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

namespace IdentityServer.IntegrationTests.Clients.Setup;

internal class Scopes
{
    public static IEnumerable<IdentityResource> GetIdentityScopes()
    {
        return new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Address(),
            new IdentityResource("roles", new[] { "role" })
        };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>
        {
            new ApiResource
            {
                Name = "api",
                ApiSecrets =
                {
                    new Secret("secret".Sha256())
                },
                Scopes = { "api1", "api2", "api3", "api4.with.roles" }
            },
            new ApiResource("other_api")
            {
                Scopes = { "other_api" }
            }
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new ApiScope[]
        {
            new ApiScope
            {
                Name = "api1"
            },
            new ApiScope
            {
                Name = "api2"
            },
            new ApiScope
            {
                Name = "api3"
            },
            new ApiScope
            {
                Name = "api4.with.roles",
                UserClaims = { "role" }
            },
            new ApiScope
            {
                Name = "other_api",
            },
        };
    }
}
