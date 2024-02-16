/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using FluentAssertions;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer.IntegrationTests.Clients.Setup;
using IdentityServer.IntegrationTests.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Xunit;


namespace IdentityServer.IntegrationTests.Clients;

public class ClientAssertionClient
{
    private const string TokenEndpoint = "https://idsvr8/connect/token";
    private const string ClientId = "certificate_base64_valid";

    private readonly HttpClient _client;

    public ClientAssertionClient()
    {
        var builder = new WebHostBuilder()
            .UseStartup<Startup>();
        var server = new TestServer(builder);

        _client = server.CreateClient();
    }

    [Fact]
    public async Task Valid_client_with_manual_payload_should_succeed()
    {
        var token = CreateToken(ClientId);
        var requestBody = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer" },
                { "client_assertion", token },
                { "grant_type", "client_credentials" },
                { "scope", "api1" }
            });

        var response = await GetToken(requestBody);

        AssertValidToken(response);
    }

    [Fact]
    public async Task Valid_client_should_succeed()
    {
        var token = CreateToken(ClientId);

        var response = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = TokenEndpoint,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            GrantType = "client_credentials",
            ClientId = ClientId,
            ClientAssertion =
            {
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                Value = token
            },

            Scope = "api1"
        });

        AssertValidToken(response);
    }

    [Fact]
    public async Task Valid_client_with_implicit_clientId_should_succeed()
    {
        var token = CreateToken(ClientId);

        var response = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = TokenEndpoint,
            ClientId = "client",
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            GrantType = "client_credentials",
            ClientAssertion =
            {
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                Value = token
            },

            Scope = "api1"
        });

        AssertValidToken(response);
    }

    [Fact]
    public async Task Valid_client_with_token_replay_should_fail()
    {
        var token = CreateToken(ClientId);

        var response = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = TokenEndpoint,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            GrantType = "client_credentials",
            ClientId = ClientId,
            ClientAssertion =
            {
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                Value = token
            },

            Scope = "api1"
        });

        AssertValidToken(response);

        // replay
        response = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = TokenEndpoint,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            GrantType = "client_credentials",
            ClientId = ClientId,
            ClientAssertion =
            {
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                Value = token
            },

            Scope = "api1"
        });

        response.IsError.Should().BeTrue();
        response.Error.Should().Be("invalid_client");
    }

    [Fact]
    public async Task Client_with_invalid_secret_should_fail()
    {
        var response = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = TokenEndpoint,
            GrantType = "client_credentials",
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            ClientId = ClientId,

            ClientAssertion =
            {
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                Value = "invalid"
            },

            Scope = "api1"
        });

        response.IsError.Should().Be(true);
        response.Error.Should().Be(OidcConstants.TokenErrors.InvalidClient);
        response.ErrorType.Should().Be(ResponseErrorType.Protocol);
    }

    [Fact]
    public async Task Invalid_client_should_fail()
    {
        const string clientId = "certificate_base64_invalid";
        var token = CreateToken(clientId);

        var tokenRequest = new ClientCredentialsTokenRequest();
        tokenRequest.Address = TokenEndpoint;
        tokenRequest.ClientId = clientId;
        tokenRequest.GrantType = "client_credentials";
        tokenRequest.ClientAssertion.Type = OidcConstants.ClientAssertionTypes.JwtBearer;
        tokenRequest.ClientAssertion.Value = token;
        tokenRequest.Scope = "api1";
        tokenRequest.ClientCredentialStyle = ClientCredentialStyle.PostBody;
        var response = await _client.RequestTokenAsync(tokenRequest);


        response.IsError.Should().Be(true);
        response.Error.Should().Be(OidcConstants.TokenErrors.InvalidClient);
        response.ErrorType.Should().Be(ResponseErrorType.Protocol);
    }

    private async Task<TokenResponse> GetToken(FormUrlEncodedContent body)
    {
        var response = await _client.PostAsync(TokenEndpoint, body);
        return await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response);
    }

    private void AssertValidToken(TokenResponse response)
    {
        response.IsError.Should().Be(false);
        response.ExpiresIn.Should().Be(3600);
        response.TokenType.Should().Be("Bearer");
        response.IdentityToken.Should().BeNull();
        response.RefreshToken.Should().BeNull();

        var payload = GetPayload(response);

        payload.Count().Should().Be(8);

        payload.Keys.Should().Contain("iss");
        payload.Keys.Should().Contain("client_id");

        payload["iss"].ValueKind.Should().Be(JsonValueKind.String);
        payload["client_id"].ValueKind.Should().Be(JsonValueKind.String);
        payload["iss"].ToString().Should().Be("https://idsvr8");
        payload["client_id"].ToString().Should().Be(ClientId);



        payload["scope"].ValueKind.Should().Be(JsonValueKind.Array);


        var scopes = payload["scope"].EnumerateArray();
        scopes.First().ToString().Should().Be("api1");

        payload["aud"].ToString().Should().Be("api");
    }

    private Dictionary<string, JsonElement> GetPayload(TokenResponse response)
    {
        var token = response.AccessToken.Split('.').Skip(1).Take(1).First();
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            Encoding.UTF8.GetString(Base64Url.Decode(token)));

        return dictionary;
    }

    private string CreateToken(string clientId, DateTime? nowOverride = null)
    {
        var certificate = TestCert.Load();
        var now = nowOverride ?? DateTime.UtcNow;

        var token = new JwtSecurityToken(
                clientId,
                TokenEndpoint,
                new List<Claim>()
                {
                    new Claim("jti", Guid.NewGuid().ToString()),
                    new Claim(JwtClaimTypes.Subject, clientId),
                    new Claim(JwtClaimTypes.IssuedAt, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                },
                now,
                now.AddMinutes(1),
                new SigningCredentials(
                    new X509SecurityKey(certificate),
                    SecurityAlgorithms.RsaSha256
                )
            );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}
