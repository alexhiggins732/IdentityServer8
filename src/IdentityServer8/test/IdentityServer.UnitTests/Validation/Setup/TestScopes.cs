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

namespace IdentityServer.UnitTests.Validation.Setup
{
    internal class TestScopes
    {
        public static IEnumerable<IdentityResource> GetIdentity()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource
                {
                    Name = "api",
                    Scopes =  { "resource", "resource2" }
                }
            };
        }

        public static IEnumerable<ApiScope> GetScopes()
        {
            return new ApiScope[]
            {
                new ApiScope
                {
                    Name = "resource",
                    Description = "resource scope"
                },
                new ApiScope
                {
                    Name = "resource2",
                    Description = "resource scope"
                }
            };
        }
    }
}
