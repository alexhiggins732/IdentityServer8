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
/// No-op client configuration validator (for backwards-compatibility).
/// </summary>
/// <seealso cref="IdentityServer8.Validation.IClientConfigurationValidator" />
public class NopClientConfigurationValidator : IClientConfigurationValidator
{
    /// <summary>
    /// Determines whether the configuration of a client is valid.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public Task ValidateAsync(ClientConfigurationValidationContext context)
    {
        context.IsValid = true;
        return Task.CompletedTask;
    }
}
