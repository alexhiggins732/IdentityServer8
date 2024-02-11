/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer8.Extensions;
using IdentityServer8.Models;
using Microsoft.Extensions.Logging;
using static IdentityServer8.IdentityServerConstants;

namespace IdentityServer8.Validation
{
    /// <summary>
    /// Validator for an X.509 certificate based client secret using the thumbprint
    /// </summary>
    public class X509ThumbprintSecretValidator : ISecretValidator
    {
        private readonly ILogger<X509ThumbprintSecretValidator> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        public X509ThumbprintSecretValidator(ILogger<X509ThumbprintSecretValidator> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
        {
            var fail = Task.FromResult(new SecretValidationResult { Success = false });

            if (parsedSecret.Type != ParsedSecretTypes.X509Certificate)
            {
                _logger.LogDebug("X509 thumbprint secret validator cannot process {type}", parsedSecret.Type ?? "null");
                return fail;
            }

            if (!(parsedSecret.Credential is X509Certificate2 cert))
            {
                throw new InvalidOperationException("Credential is not a x509 certificate.");
            }

            var thumbprint = cert.Thumbprint;
            if (thumbprint == null)
            {
                _logger.LogWarning("No thumbprint found in X509 certificate.");
                return fail;
            }

            var thumbprintSecrets = secrets.Where(s => s.Type == SecretTypes.X509CertificateThumbprint);
            if (!thumbprintSecrets.Any())
            {
                _logger.LogDebug("No thumbprint secrets configured for client.");
                return fail;
            }

            foreach (var thumbprintSecret in thumbprintSecrets)
            {
                if (thumbprint.Equals(thumbprintSecret.Value, StringComparison.OrdinalIgnoreCase))
                {
                    var result = new SecretValidationResult
                    {
                        Success = true,
                        Confirmation = cert.CreateThumbprintCnf()
                    };

                    return Task.FromResult(result);
                }
            }

            _logger.LogDebug("No matching x509 thumbprint secret found.");
            return fail;
        }
    }
}