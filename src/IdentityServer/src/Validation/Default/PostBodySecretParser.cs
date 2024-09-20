/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Validation;

/// <summary>
/// Parses a POST body for secrets
/// </summary>
public class PostBodySecretParser : ISecretParser
{
    private readonly ILogger _logger;
    private readonly IdentityServerOptions _options;

    /// <summary>
    /// Creates the parser with options
    /// </summary>
    /// <param name="options">IdentityServer options</param>
    /// <param name="logger">Logger</param>
    public PostBodySecretParser(IdentityServerOptions options, ILogger<PostBodySecretParser> logger)
    {
        _logger = logger;
        _options = options;
    }

    /// <summary>
    /// Returns the authentication method name that this parser implements
    /// </summary>
    /// <value>
    /// The authentication method.
    /// </value>
    public string AuthenticationMethod => OidcConstants.EndpointAuthenticationMethods.PostBody;

    /// <summary>
    /// Tries to find a secret on the context that can be used for authentication
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>
    /// A parsed secret
    /// </returns>
    public async Task<ParsedSecret> ParseAsync(HttpContext context)
    {
        _logger.LogDebug("Start parsing for secret in post body");

        if (!context.Request.HasApplicationFormContentType())
        {
            _logger.LogDebug("Content type is not a form");
            return null;
        }

        var body = await context.Request.ReadFormAsync();

        if (body != null)
        {
            var id = body["client_id"].FirstOrDefault();
            var secret = body["client_secret"].FirstOrDefault();

            // client id must be present
            if (id.IsPresent())
            {
                if (id.Length > _options.InputLengthRestrictions.ClientId)
                {
                    _logger.LogError("Client ID exceeds maximum length.");
                    return null;
                }

                if (secret.IsPresent())
                {
                    if (secret.Length > _options.InputLengthRestrictions.ClientSecret)
                    {
                        _logger.LogError("Client secret exceeds maximum length.");
                        return null;
                    }

                    return new ParsedSecret
                    {
                        Id = id,
                        Credential = secret,
                        Type = IdentityServerConstants.ParsedSecretTypes.SharedSecret
                    };
                }
                else
                {
                    // client secret is optional
                    _logger.LogDebug("client id without secret found");

                    return new ParsedSecret
                    {
                        Id = id,
                        Type = IdentityServerConstants.ParsedSecretTypes.NoSecret
                    };
                }
            }
        }

        _logger.LogDebug("No secret in post body found");
        return null;
    }
}
