/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer.IntegrationTests.Common;
using IdentityServer.Models;
using IdentityServer.Services;
using IdentityServer.Test;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IdentityServer.IntegrationTests.Pipeline;

public class CorsTests
{
    private const string Category = "CORS Integration";

    private IdentityServerPipeline _pipeline = new IdentityServerPipeline();

    public CorsTests()
    {
        _pipeline.Clients.AddRange(new Client[] {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.Implicit,
                RequireConsent = true,
                AllowedScopes = new List<string> { "openid", "profile", "api1", "api2" },
                RedirectUris = new List<string> { "https://client/callback" },
                AllowedCorsOrigins = new List<string> { "https://client" }
            }
        });

        _pipeline.Users.Add(new TestUser
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

        _pipeline.IdentityScopes.AddRange(new IdentityResource[] {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        });
        _pipeline.ApiResources.AddRange(new ApiResource[] {
            new ApiResource
            {
                Name = "api",
                Scopes = { "api1", "api2" }
            }
        });
        _pipeline.ApiScopes.AddRange(new[] {
            new ApiScope
            {
                Name = "api1"
            },
            new ApiScope
            {
                Name = "api2"
            }
        });

        _pipeline.Initialize();
    }

    [Theory]
    [InlineData(IdentityServerPipeline.DiscoveryEndpoint)]
    [InlineData(IdentityServerPipeline.DiscoveryKeysEndpoint)]
    [InlineData(IdentityServerPipeline.TokenEndpoint)]
    [InlineData(IdentityServerPipeline.UserInfoEndpoint)]
    [InlineData(IdentityServerPipeline.RevocationEndpoint)]
    [Trait("Category", Category)]
    public async Task cors_request_to_allowed_endpoints_should_succeed(string url)
    {
        _pipeline.BackChannelClient.DefaultRequestHeaders.Add("Origin", "https://client");
        _pipeline.BackChannelClient.DefaultRequestHeaders.Add("Access-Control-Request-Method", "GET");
        
        var message = new HttpRequestMessage(HttpMethod.Options, url);
        var response = await _pipeline.BackChannelClient.SendAsync(message);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response.Headers.Contains("Access-Control-Allow-Origin").Should().BeTrue();
    }

    [Theory]
    [InlineData(IdentityServerPipeline.AuthorizeEndpoint)]
    [InlineData(IdentityServerPipeline.EndSessionEndpoint)]
    [InlineData(IdentityServerPipeline.CheckSessionEndpoint)]
    [InlineData(IdentityServerPipeline.LoginPage)]
    [InlineData(IdentityServerPipeline.ConsentPage)]
    [InlineData(IdentityServerPipeline.ErrorPage)]
    [Trait("Category", Category)]
    public async Task cors_request_to_restricted_endpoints_should_not_succeed(string url)
    {
        _pipeline.BackChannelClient.DefaultRequestHeaders.Add("Origin", "https://client");
        _pipeline.BackChannelClient.DefaultRequestHeaders.Add("Access-Control-Request-Method", "GET");

        var message = new HttpRequestMessage(HttpMethod.Options, url);
        var response = await _pipeline.BackChannelClient.SendAsync(message);

        response.Headers.Contains("Access-Control-Allow-Origin").Should().BeFalse();
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task custom_cors_policy_provider_should_be_used()
    {
        var policy = new StubCorePolicyProvider();
        _pipeline.OnPreConfigureServices += services =>
        {
            services.AddSingleton<ICorsPolicyService>(policy);
        };
        _pipeline.Initialize();

        _pipeline.BackChannelClient.DefaultRequestHeaders.Add("Origin", "https://client");
        _pipeline.BackChannelClient.DefaultRequestHeaders.Add("Access-Control-Request-Method", "GET");

        var message = new HttpRequestMessage(HttpMethod.Options, IdentityServerPipeline.DiscoveryEndpoint);
        var response = await _pipeline.BackChannelClient.SendAsync(message);

        policy.WasCalled.Should().BeTrue();
    }
}

public class StubCorePolicyProvider : ICorsPolicyService
{
    public bool Result;
    public bool WasCalled;

    public Task<bool> IsOriginAllowedAsync(string origin)
    {
        WasCalled = true;
        return Task.FromResult(Result);
    }
}
