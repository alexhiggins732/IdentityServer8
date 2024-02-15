/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Secret = IdentityServer8.Models.Secret;


namespace IdentityServer8.Validation;

/// <summary>
/// Validates a shared secret stored in SHA256 or SHA512
/// </summary>
public class HashedSharedSecretValidator : ISecretValidator
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HashedSharedSecretValidator"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public HashedSharedSecretValidator(ILogger<HashedSharedSecretValidator> logger)
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
    /// <exception cref="System.ArgumentNullException">Id or cedential</exception>
    public Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
    {
        var fail = Task.FromResult(new SecretValidationResult { Success = false });
        var success = Task.FromResult(new SecretValidationResult { Success = true });

        if (parsedSecret.Type != IdentityServerConstants.ParsedSecretTypes.SharedSecret)
        {
            _logger.LogDebug("Hashed shared secret validator cannot process {type}", parsedSecret.Type ?? "null");
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

        var secretSha256 = sharedSecret.Sha256();
        var secretSha512 = sharedSecret.Sha512();

        foreach (var secret in sharedSecrets)
        {
            var secretDescription = string.IsNullOrEmpty(secret.Description) ? "no description" : secret.Description;

            bool isValid = false;
            byte[] secretBytes;

            try
            {
                secretBytes = Convert.FromBase64String(secret.Value);
            }
            catch (FormatException)
            {
                _logger.LogInformation("Secret: {description} uses invalid hashing algorithm.", secretDescription);
                return fail;
            }
            catch (ArgumentNullException)
            {
                _logger.LogInformation("Secret: {description} is null.", secretDescription);
                return fail;
            }

            if (secretBytes.Length == 32)
            {
                isValid = TimeConstantComparer.IsEqual(secret.Value, secretSha256);
            }
            else if (secretBytes.Length == 64)
            {
                isValid = TimeConstantComparer.IsEqual(secret.Value, secretSha512);
            }
            else
            {
                _logger.LogInformation("Secret: {description} uses invalid hashing algorithm.", secretDescription);
                return fail;
            }

            if (isValid)
            {
                return success;
            }
        }

        _logger.LogDebug("No matching hashed secret found.");
        return fail;
    }
}
