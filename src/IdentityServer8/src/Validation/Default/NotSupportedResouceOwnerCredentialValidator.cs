/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using IdentityServer8.Models;

namespace IdentityServer8.Validation
{
    /// <summary>
    /// Default resource owner password validator (no implementation == not supported)
    /// </summary>
    /// <seealso cref="IdentityServer8.Validation.IResourceOwnerPasswordValidator" />
    public class NotSupportedResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSupportedResourceOwnerPasswordValidator"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public NotSupportedResourceOwnerPasswordValidator(ILogger<NotSupportedResourceOwnerPasswordValidator> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.UnsupportedGrantType);

            _logger.LogInformation("Resource owner password credential type not supported. Configure an IResourceOwnerPasswordValidator.");
            return Task.CompletedTask;
        }
    }
}