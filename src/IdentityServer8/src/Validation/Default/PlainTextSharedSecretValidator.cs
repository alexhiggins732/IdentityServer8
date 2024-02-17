/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Validation;

/// <summary>
/// Validates a secret stored in plain text
/// </summary>
public class PlainTextSharedSecretValidator : ISecretValidator
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainTextSharedSecretValidator"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public PlainTextSharedSecretValidator(ILogger<PlainTextSharedSecretValidator> logger)
    {
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
    /// <exception cref="System.ArgumentException">id or credential is missing.</exception>
    public Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
    {
        var fail = Task.FromResult(new SecretValidationResult { Success = false });
        var success = Task.FromResult(new SecretValidationResult { Success = true });

        if (parsedSecret.Type != IdentityServerConstants.ParsedSecretTypes.SharedSecret)
        {
            _logger.LogError("Parsed secret should not be of type: {type}", parsedSecret.Type ?? "null");
            return fail;
        }

        var sharedSecrets = secrets.Where(s => s.Type == IdentityServerConstants.SecretTypes.SharedSecret);
        if (!sharedSecrets.Any())
        {
            _logger.LogDebug("No shared secret configured for client.");
            return fail;
        }

        var sharedSecret = parsedSecret.Credential as string;

        if (parsedSecret.Id.IsMissing() || sharedSecret.IsMissing())
        {
            throw new ArgumentException("Id or Credential is missing.");
        }

        foreach (var secret in sharedSecrets)
        {
            // use time constant string comparison
            var isValid = TimeConstantComparer.IsEqual(sharedSecret, secret.Value);

            if (isValid)
            {
                return success;
            }
        }

        _logger.LogDebug("No matching plain text secret found.");
        return fail;
    }
}
