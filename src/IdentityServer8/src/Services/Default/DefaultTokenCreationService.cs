/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer8.Services;

/// <summary>
/// Default token creation service
/// </summary>
public class DefaultTokenCreationService : ITokenCreationService
{
    /// <summary>
    /// The key service
    /// </summary>
    protected readonly IKeyMaterialService Keys;

    /// <summary>
    /// The logger
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    ///  The clock
    /// </summary>
    protected readonly ISystemClock Clock;

    /// <summary>
    /// The options
    /// </summary>
    protected readonly IdentityServerOptions Options;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultTokenCreationService"/> class.
    /// </summary>
    /// <param name="clock">The options.</param>
    /// <param name="keys">The keys.</param>
    /// <param name="options">The options.</param>
    /// <param name="logger">The logger.</param>
    public DefaultTokenCreationService(
        ISystemClock clock,
        IKeyMaterialService keys,
        IdentityServerOptions options,
        ILogger<DefaultTokenCreationService> logger)
    {
        Clock = clock;
        Keys = keys;
        Options = options;
        Logger = logger;
    }

    /// <summary>
    /// Creates the token.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <returns>
    /// A protected and serialized security token
    /// </returns>
    public virtual async Task<string> CreateTokenAsync(Token token)
    {
        var header = await CreateHeaderAsync(token);
        var payload = await CreatePayloadAsync(token);

        return await CreateJwtAsync(header.Header, header.Credentials, payload);
    }

    /// <summary>
    /// Creates the JWT header
    /// </summary>
    /// <param name="token">The token.</param>
    /// <returns>The JWT header</returns>
    protected virtual async Task<(Dictionary<string, object> Header, SigningCredentials Credentials)> CreateHeaderAsync(Token token)
    {
        var credential = await Keys.GetSigningCredentialsAsync(token.AllowedSigningAlgorithms);

        if (credential == null)
        {
            throw new InvalidOperationException("No signing credential is configured. Can't create JWT token");
        }

        var header = new Dictionary<string, object>
        {
            { "alg", credential.Algorithm },
            { "kid", credential.Key.KeyId }
        };

        // emit x5t claim for backwards compatibility with v4 of MS JWT library
        if (credential.Key is X509SecurityKey x509Key)
        {
            var cert = x509Key.Certificate;
            if (Clock.UtcNow.UtcDateTime > cert.NotAfter)
            {
                Logger.LogWarning("Certificate {subjectName} has expired on {expiration}", cert.Subject, cert.NotAfter.ToString(CultureInfo.InvariantCulture));
            }

            header["x5t"] = Base64Url.Encode(cert.GetCertHash());
        }

        if (token.Type == TokenTypes.AccessToken)
        {
            if (Options.AccessTokenJwtType.IsPresent())
            {
                header["typ"] = Options.AccessTokenJwtType;
            }
        }

        return (header, credential);
    }

    /// <summary>
    /// Creates the JWT payload
    /// </summary>
    /// <param name="token">The token.</param>
    /// <returns>The JWT payload</returns>
    protected virtual Task<JwtPayload> CreatePayloadAsync(Token token)
    {
        var payload = token.CreateJwtPayload(Clock, Options, Logger);
        return Task.FromResult(payload);
    }

    /// <summary>
    /// Applies the signature to the JWT
    /// </summary>
    /// <param name="header">The JWT header.</param>
    /// <param name="payload">The JWT payload.</param>
    /// <returns>The signed JWT</returns>
    protected virtual Task<string> CreateJwtAsync(Dictionary<string, object> header, SigningCredentials signingCredentials, JwtPayload payload)
    {
        var handler = new JsonWebTokenHandler();
        var descriptor = new SecurityTokenDescriptor
        {
            Claims = payload,            
            Issuer = payload.TryGetValue("iss", out var issuer) ? issuer.ToString() : null,
            Expires = payload.TryGetValue("exp", out var exp) ? DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp)).UtcDateTime : null,
            SigningCredentials = signingCredentials
        };
        if (header.TryGetValue("typ", out var typ))
            descriptor.TokenType = typ.ToString();        

        if (payload.TryGetValue("aud", out var audObj) && audObj is List<string> audiences)
            audiences.ForEach(descriptor.Audiences.Add);

        var jwt = handler.CreateToken(descriptor);

        return Task.FromResult(jwt);
    }


}
