/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel;
using IdentityServer.UnitTests.Common;
using IdentityServer.UnitTests.Services.Default;
using IdentityServer.UnitTests.Validation.Setup;
using IdentityServer8;
using IdentityServer8.Configuration;
using IdentityServer8.Models;
using IdentityServer8.Services;
using IdentityServer8.Stores;
using IdentityServer8.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace IdentityServer.UnitTests.Validation.Secrets
{
    public class PrivateKeyJwtSecretValidation
    {
        private readonly ISecretValidator _validator;
        private readonly IClientStore _clients;

        public PrivateKeyJwtSecretValidation()
        {
            _validator = new PrivateKeyJwtSecretValidator(
                new MockHttpContextAccessor(
                    new IdentityServerOptions()
                    {
                        IssuerUri = "https://idsrv3.com"
                    }
                    ),
                    new DefaultReplayCache(new TestCache()),
                    new LoggerFactory().CreateLogger<PrivateKeyJwtSecretValidator>()
                );
            _clients = new InMemoryClientStore(ClientValidationTestClients.Get());
        }

        private JsonWebToken CreateToken(string clientId, DateTime? nowOverride = null)
        {
            var certificate = TestCert.Load();
            var now = nowOverride ?? DateTime.UtcNow;

            var tokenHandler = new JsonWebTokenHandler();

            var claims = new Dictionary<string, object>
            {
                { "jti", Guid.NewGuid().ToString() },
                { JwtClaimTypes.Subject, clientId },
                { JwtClaimTypes.IssuedAt, new DateTimeOffset(now).ToUnixTimeSeconds() }
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = clientId,
                Audience = "https://idsrv3.com/connect/token",
                Claims = claims,
                Expires = now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(
                    new X509SecurityKey(certificate),
                    SecurityAlgorithms.RsaSha256
                )
            };

            // Token string olarak oluşturuluyor
            var tokenString = tokenHandler.CreateToken(tokenDescriptor);

            // String token'ı JsonWebToken nesnesine dönüştürüp döndür
            return tokenHandler.ReadJsonWebToken(tokenString);
        }

        [Fact]
        public async Task Invalid_Certificate_X5t_Only_Requires_Full_Certificate()
        {
            var clientId = "certificate_valid";
            var client = await _clients.FindEnabledClientByIdAsync(clientId);

            var token = CreateToken(clientId);
            var secret = new ParsedSecret
            {
                Id = clientId,
                Credential = $"{token.EncodedHeader}.{token.EncodedPayload}.{token.EncodedSignature}",
                Type = IdentityServerConstants.ParsedSecretTypes.JwtBearer
            };

            var result = await _validator.ValidateAsync(client.ClientSecrets, secret);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Invalid_Certificate_Thumbprint()
        {
            var clientId = "certificate_invalid";
            var client = await _clients.FindEnabledClientByIdAsync(clientId);

            var token = CreateToken(clientId);
            var secret = new ParsedSecret
            {
                Id = clientId,
                Credential = $"{token.EncodedHeader}.{token.EncodedPayload}.{token.EncodedSignature}",
                Type = IdentityServerConstants.ParsedSecretTypes.JwtBearer
            };

            var result = await _validator.ValidateAsync(client.ClientSecrets, secret);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Valid_Certificate_Base64()
        {
            var clientId = "certificate_base64_valid";
            var client = await _clients.FindEnabledClientByIdAsync(clientId);

            var token = CreateToken(clientId);
            var secret = new ParsedSecret
            {
                Id = clientId,
                Credential = $"{token.EncodedHeader}.{token.EncodedPayload}.{token.EncodedSignature}",
                Type = IdentityServerConstants.ParsedSecretTypes.JwtBearer
            };

            var result = await _validator.ValidateAsync(client.ClientSecrets, secret);

            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Invalid_Replay()
        {
            var clientId = "certificate_base64_valid";
            var client = await _clients.FindEnabledClientByIdAsync(clientId);

            var token = CreateToken(clientId);
            var secret = new ParsedSecret
            {
                Id = clientId,
                Credential = $"{token.EncodedHeader}.{token.EncodedPayload}.{token.EncodedSignature}",
                Type = IdentityServerConstants.ParsedSecretTypes.JwtBearer
            };

            var result = await _validator.ValidateAsync(client.ClientSecrets, secret);
            result.Success.Should().BeTrue();

            result = await _validator.ValidateAsync(client.ClientSecrets, secret);
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Invalid_Certificate_Base64()
        {
            var clientId = "certificate_base64_invalid";
            var client = await _clients.FindEnabledClientByIdAsync(clientId);

            var token = CreateToken(clientId);
            var secret = new ParsedSecret
            {
                Id = clientId,
                Credential = $"{token.EncodedHeader}.{token.EncodedPayload}.{token.EncodedSignature}",
                Type = IdentityServerConstants.ParsedSecretTypes.JwtBearer
            };

            var result = await _validator.ValidateAsync(client.ClientSecrets, secret);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Invalid_Issuer()
        {
            var clientId = "certificate_valid";
            var client = await _clients.FindEnabledClientByIdAsync(clientId);

            var token = CreateToken(clientId);

            // "iss" claim'ini manipüle et
            var claims = token.Claims.ToList();
            claims.RemoveAll(c => c.Type == JwtClaimTypes.Issuer);
            claims.Add(new Claim(JwtClaimTypes.Issuer, "invalid"));

            var modifiedToken = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
            {
                Issuer = clientId,
                Audience = "https://idsrv3.com/connect/token",
                Claims = claims.ToDictionary(c => c.Type, c => (object) c.Value),
                SigningCredentials = new SigningCredentials(
                    new X509SecurityKey(TestCert.Load()),
                    SecurityAlgorithms.RsaSha256
                )
            });

            var secret = new ParsedSecret
            {
                Id = clientId,
                Credential = modifiedToken,
                Type = IdentityServerConstants.ParsedSecretTypes.JwtBearer
            };

            var result = await _validator.ValidateAsync(client.ClientSecrets, secret);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Invalid_Subject()
        {
            var clientId = "certificate_valid";
            var client = await _clients.FindEnabledClientByIdAsync(clientId);

            var token = CreateToken(clientId);

            // "sub" claim'ini manipüle et
            var claims = token.Claims.ToList();
            claims.RemoveAll(c => c.Type == JwtClaimTypes.Subject);
            claims.Add(new Claim(JwtClaimTypes.Subject, "invalid"));

            var modifiedToken = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
            {
                Issuer = clientId,
                Audience = "https://idsrv3.com/connect/token",
                Claims = claims.ToDictionary(c => c.Type, c => (object) c.Value),
                SigningCredentials = new SigningCredentials(
                    new X509SecurityKey(TestCert.Load()),
                    SecurityAlgorithms.RsaSha256
                )
            });

            var secret = new ParsedSecret
            {
                Id = clientId,
                Credential = modifiedToken,
                Type = IdentityServerConstants.ParsedSecretTypes.JwtBearer
            };

            var result = await _validator.ValidateAsync(client.ClientSecrets, secret);

            result.Success.Should().BeFalse();
        }


        [Fact]
        public async Task Invalid_Expired_Token()
        {
            var clientId = "certificate_valid";
            var client = await _clients.FindEnabledClientByIdAsync(clientId);

            var token = CreateToken(clientId, DateTime.UtcNow.AddHours(-1));
            var secret = new ParsedSecret
            {
                Id = clientId,
                Credential = $"{token.EncodedHeader}.{token.EncodedPayload}.{token.EncodedSignature}",
                Type = IdentityServerConstants.ParsedSecretTypes.JwtBearer
            };

            var result = await _validator.ValidateAsync(client.ClientSecrets, secret);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Invalid_Unsigned_Token()
        {
            var clientId = "certificate_valid";
            var client = await _clients.FindEnabledClientByIdAsync(clientId);

            // Unsigned token için header ve claims oluştur
            var header = new Dictionary<string, object>
            {
                { "alg", "none" }
            };

            var claims = new Dictionary<string, object>
            {
                { "jti", Guid.NewGuid().ToString() },
                { JwtClaimTypes.Subject, clientId },
                { JwtClaimTypes.Issuer, clientId },
                { JwtClaimTypes.IssuedAt, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() }
            };

            // Header ve Claims JSON string'e dönüştürülüyor
            var headerJson = System.Text.Json.JsonSerializer.Serialize(header);
            var claimsJson = System.Text.Json.JsonSerializer.Serialize(claims);

            // JSON string Base64Url formatına dönüştürülüyor
            var encodedHeader = Base64Url.Encode(System.Text.Encoding.UTF8.GetBytes(headerJson));
            var encodedClaims = Base64Url.Encode(System.Text.Encoding.UTF8.GetBytes(claimsJson));

            // İmzasız JWT oluşturuluyor
            var token = $"{encodedHeader}.{encodedClaims}.";

            var secret = new ParsedSecret
            {
                Id = clientId,
                Credential = token,
                Type = IdentityServerConstants.ParsedSecretTypes.JwtBearer
            };

            var result = await _validator.ValidateAsync(client.ClientSecrets, secret);

            result.Success.Should().BeFalse();
        }


    }
}
