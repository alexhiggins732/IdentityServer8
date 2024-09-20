/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer.IntegrationTests.Common;
using IdentityServer.Models;
using IdentityServer.Test;
using Xunit;

namespace IdentityServer.IntegrationTests.Endpoints.Authorize;

public class SessionIdTests
{
    private const string Category = "SessionIdTests";

    private IdentityServerPipeline _mockPipeline = new IdentityServerPipeline();

    public SessionIdTests()
    {
        _mockPipeline.Clients.AddRange(new Client[] {
            new Client
            {
                ClientId = "client1",
                AllowedGrantTypes = GrantTypes.Implicit,
                RequireConsent = false,
                AllowedScopes = new List<string> { "openid", "profile" },
                RedirectUris = new List<string> { "https://client1/callback" },
                AllowAccessTokensViaBrowser = true
            },
            new Client
            {
                ClientId = "client2",
                AllowedGrantTypes = GrantTypes.Implicit,
                RequireConsent = true,
                AllowedScopes = new List<string> { "openid", "profile", "api1", "api2" },
                RedirectUris = new List<string> { "https://client2/callback" },
                AllowAccessTokensViaBrowser = true
            }
        });

        _mockPipeline.Users.Add(new TestUser
        {
            SubjectId = "bob",
            Username = "bob",
            Claims = new Claim[]
            {
                new Claim("name", "Bob Loblaw"),
                new Claim("email", "bob@loblaw.com"),
                new Claim("role", "Attorney")
            }
        });

        _mockPipeline.IdentityScopes.AddRange(new IdentityResource[] {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        });
        _mockPipeline.ApiResources.AddRange(new ApiResource[] {
            new ApiResource
            {
                Name = "api",
            }
        });
        _mockPipeline.ApiScopes.AddRange(new ApiScope[] {
            new ApiScope
            {
                Name = "api1"
            },
            new ApiScope
            {
                Name = "api2"
            }
        });

        _mockPipeline.Initialize();
    }

    [Fact]
    public async Task session_id_should_be_reissued_if_session_cookie_absent()
    {
        await _mockPipeline.LoginAsync("bob");
        var sid1 = _mockPipeline.GetSessionCookie().Value;
        sid1.Should().NotBeNull();

        _mockPipeline.RemoveSessionCookie();

        await _mockPipeline.BrowserClient.GetAsync(IdentityServerPipeline.DiscoveryEndpoint);

        var sid2 = _mockPipeline.GetSessionCookie().Value;
        sid2.Should().Be(sid1);
    }
}
