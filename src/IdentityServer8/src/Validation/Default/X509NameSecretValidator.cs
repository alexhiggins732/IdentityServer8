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
/// Validator for an X.509 certificate based client secret using the common name
/// </summary>
public class X509NameSecretValidator : ISecretValidator
{
    private readonly ILogger<X509NameSecretValidator> _logger;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    public X509NameSecretValidator(ILogger<X509NameSecretValidator> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
    {
        var fail = Task.FromResult(new SecretValidationResult { Success = false });

        if (parsedSecret.Type != ParsedSecretTypes.X509Certificate)
        {
            _logger.LogDebug("X509 name secret validator cannot process {type}", parsedSecret.Type ?? "null");
            return fail;
        }

        if (!(parsedSecret.Credential is X509Certificate2 cert))
        {
            throw new InvalidOperationException("Credential is not a x509 certificate.");
        }

        var name = cert.Subject;
        if (name == null)
        {
            _logger.LogWarning("No subject/name found in X509 certificate.");
            return fail;
        }

        var nameSecrets = secrets.Where(s => s.Type == SecretTypes.X509CertificateName);
        if (!nameSecrets.Any())
        {
            _logger.LogDebug("No x509 name secrets configured for client.");
            return fail;
        }

        foreach (var nameSecret in nameSecrets)
        {
            if (name.Equals(nameSecret.Value, StringComparison.Ordinal))
            {
                var result = new SecretValidationResult
                {
                    Success = true,
                    Confirmation = cert.CreateThumbprintCnf()
                };

                return Task.FromResult(result);
            }
        }

        _logger.LogDebug("No matching x509 name secret found.");
        return fail;
    }
}
