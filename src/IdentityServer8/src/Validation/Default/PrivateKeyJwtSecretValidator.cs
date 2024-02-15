/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Microsoft.IdentityModel.Tokens;

namespace IdentityServer8.Validation;

/// <summary>
/// Validates a secret based on RS256 signed JWT token
/// </summary>
public class PrivateKeyJwtSecretValidator : ISecretValidator
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IReplayCache _replayCache;
    private readonly ILogger _logger;

    private const string Purpose = nameof(PrivateKeyJwtSecretValidator);
    
    /// <summary>
    /// Instantiates an instance of private_key_jwt secret validator
    /// </summary>
    public PrivateKeyJwtSecretValidator(IHttpContextAccessor contextAccessor, IReplayCache replayCache, ILogger<PrivateKeyJwtSecretValidator> logger)
    {
        _contextAccessor = contextAccessor;
        _replayCache = replayCache;
        _logger = logger;
    }

    /// <summary>
    /// Validates a secret
    /// </summary>
    /// <param name="secrets">The stored secrets.</param>
    /// <param name="parsedSecret">The received secret.</param>
    /// <returns>
    /// A validation result
    /// </returns>
    /// <exception cref="System.ArgumentException">ParsedSecret.Credential is not a JWT token</exception>
    public async Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
    {
        var fail = new SecretValidationResult { Success = false };
        var success = new SecretValidationResult { Success = true };

        if (parsedSecret.Type != IdentityServerConstants.ParsedSecretTypes.JwtBearer)
        {
            return fail;
        }

        if (!(parsedSecret.Credential is string jwtTokenString))
        {
            _logger.LogError("ParsedSecret.Credential is not a string.");
            return fail;
        }

        List<SecurityKey> trustedKeys;
        try
        {
            trustedKeys = await secrets.GetKeysAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not parse secrets");
            return fail;
        }

        if (!trustedKeys.Any())
        {
            _logger.LogError("There are no keys available to validate client assertion.");
            return fail;
        }

        var validAudiences = new[]
        {
            // issuer URI (tbd)
            //_contextAccessor.HttpContext.GetIdentityServerIssuerUri(),
            
            // token endpoint URL
            string.Concat(_contextAccessor.HttpContext.GetIdentityServerIssuerUri().EnsureTrailingSlash(),
                Constants.ProtocolRoutePaths.Token)
        };
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKeys = trustedKeys,
            ValidateIssuerSigningKey = true,

            ValidIssuer = parsedSecret.Id,
            ValidateIssuer = true,

            ValidAudiences = validAudiences,
            ValidateAudience = true,

            RequireSignedTokens = true,
            RequireExpirationTime = true,
            
            ClockSkew = TimeSpan.FromMinutes(5)
        };
        try
        {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(jwtTokenString, tokenValidationParameters, out var token);

            var jwtToken = (JwtSecurityToken)token;
            if (jwtToken.Subject != jwtToken.Issuer)
            {
                _logger.LogError("Both 'sub' and 'iss' in the client assertion token must have a value of client_id.");
                return fail;
            }
            
            var exp = jwtToken.Payload.Exp;
            if (!exp.HasValue)
            {
                _logger.LogError("exp is missing.");
                return fail;
            }
            
            var jti = jwtToken.Payload.Jti;
            if (jti.IsMissing())
            {
                _logger.LogError("jti is missing.");
                return fail;
            }

            if (await _replayCache.ExistsAsync(Purpose, jti))
            {
                _logger.LogError("jti is found in replay cache. Possible replay attack.");
                return fail;
            }
            else
            {
                await _replayCache.AddAsync(Purpose, jti, DateTimeOffset.FromUnixTimeSeconds(exp.Value).AddMinutes(5));
            }

            return success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "JWT token validation error");
            return fail;
        }
    }
}
