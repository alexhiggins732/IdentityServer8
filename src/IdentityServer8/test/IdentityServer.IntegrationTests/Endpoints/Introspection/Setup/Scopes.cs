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

internal class Scopes
{
    public static IEnumerable<ApiResource> GetApis()
    {
        return new ApiResource[]
        {
            new ApiResource
            {
                Name = "api1",
                ApiSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },
                Scopes = { "api1" }
            },
            new ApiResource
            {
                Name = "api2",
                ApiSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },
                Scopes = { "api2" }
            },
             new ApiResource
            {
                Name = "api3",
                ApiSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },
                Scopes = { "api3-a", "api3-b" }
            }
        };
    }
    public static IEnumerable<ApiScope> GetScopes()
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
                Name = "api3-a"
            },
            new ApiScope
            {
                Name = "api3-b"
            }
        };
    }
}
