/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Validation;

/// <summary>
/// Uses the registered secret parsers to parse a secret on the current request
/// </summary>
public class SecretParser : ISecretsListParser
{
    private readonly ILogger _logger;
    private readonly IEnumerable<ISecretParser> _parsers;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecretParser"/> class.
    /// </summary>
    /// <param name="parsers">The parsers.</param>
    /// <param name="logger">The logger.</param>
    public SecretParser(IEnumerable<ISecretParser> parsers, ILogger<ISecretsListParser> logger)
    {
        _parsers = parsers;
        _logger = logger;
    }

    /// <summary>
    /// Checks the context to find a secret.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns></returns>
    public async Task<ParsedSecret> ParseAsync(HttpContext context)
    {
        // see if a registered parser finds a secret on the request
        ParsedSecret bestSecret = null;
        foreach (var parser in _parsers)
        {
            var parsedSecret = await parser.ParseAsync(context);
            if (parsedSecret != null)
            {
                _logger.LogDebug("Parser found secret: {type}", parser.GetType().Name);

                bestSecret = parsedSecret;

                if (parsedSecret.Type != IdentityServerConstants.ParsedSecretTypes.NoSecret)
                {
                    break;
                }
            }
        }

        if (bestSecret != null)
        {
            _logger.LogDebug("Secret id found: {id}", Ioc.Sanitizer.Log.Sanitize(bestSecret.Id));
            return bestSecret;
        }

        _logger.LogDebug("Parser found no secret");
        return null;
    }

    /// <summary>
    /// Gets all available authentication methods.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetAvailableAuthenticationMethods()
    {
        return _parsers.Select(p => p.AuthenticationMethod).Where(p => !String.IsNullOrWhiteSpace(p));
    }
}
