/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using FluentAssertions;
using IdentityModel.Client;
using IdentityServer.IntegrationTests.Clients.Setup;
using IdentityServer8.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Text.Json;
using Xunit;

namespace IdentityServer.IntegrationTests.Clients;

public class CustomTokenRequestValidatorClient
{
    private const string TokenEndpoint = "https://server/connect/token";

    private readonly HttpClient _client;

    public CustomTokenRequestValidatorClient()
    {
        var val = new TestCustomTokenRequestValidator();
        Startup.CustomTokenRequestValidator = val;

        var builder = new WebHostBuilder()
            .UseStartup<Startup>();
        var server = new TestServer(builder);

        _client = server.CreateClient();
    }

    [Fact]
    public async Task Client_credentials_request_should_contain_custom_response()
    {
        var response = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = TokenEndpoint,

            ClientId = "client",
            ClientSecret = "secret",
            Scope = "api1"
        });
        ValidateCustomFields(response);
    }

    [Fact]
    public async Task Resource_owner_credentials_request_should_contain_custom_response()
    {
        var response = await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = TokenEndpoint,

            ClientId = "roclient",
            ClientSecret = "secret",
            Scope = "api1",

            UserName = "bob",
            Password = "bob"
        });

        ValidateCustomFields(response);
    }

    [Fact]
    public async Task Refreshing_a_token_should_contain_custom_response()
    {
        var response = await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = TokenEndpoint,

            ClientId = "roclient",
            ClientSecret = "secret",
            Scope = "api1 offline_access",

            UserName = "bob",
            Password = "bob"
        });

        response = await _client.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = TokenEndpoint,
            ClientId = "roclient",
            ClientSecret = "secret",

            RefreshToken = response.RefreshToken
        });

        ValidateCustomFields(response);
    }

    [Fact]
    public async Task Extension_grant_request_should_contain_custom_response()
    {
        var response = await _client.RequestTokenAsync(new TokenRequest
        {
            Address = TokenEndpoint,
            GrantType = "custom",

            ClientId = "client.custom",
            ClientSecret = "secret",

            Parameters =
            {
                { "scope", "api1" },
                { "custom_credential", "custom credential"}
            }
        });

        ValidateCustomFields(response);
    }

     private Dictionary<string, JsonElement> GetFields(JsonElement json)
    {
        return json.ToObject<Dictionary<string, JsonElement>>();
    }
    private void ValidateCustomFields(TokenResponse response)
    {
        var fields = GetFields(response.Json);
        fields["custom"].ToString().Should().Be("custom");

    }
}
