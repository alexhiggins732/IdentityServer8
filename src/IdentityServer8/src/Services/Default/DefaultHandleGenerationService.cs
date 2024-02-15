/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Services;

/// <summary>
/// Default handle generation service
/// </summary>
/// <seealso cref="IdentityServer8.Services.IHandleGenerationService" />
public class DefaultHandleGenerationService : IHandleGenerationService
{
    /// <summary>
    /// Generates a handle.
    /// </summary>
    /// <param name="length">The length.</param>
    /// <returns></returns>
    public Task<string> GenerateAsync(int length)
    {
        return Task.FromResult(CryptoRandom.CreateUniqueId(length, CryptoRandom.OutputFormat.Hex));
    }
}
