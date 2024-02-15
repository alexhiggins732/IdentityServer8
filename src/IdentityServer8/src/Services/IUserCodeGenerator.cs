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
/// Implements device flow user code generation
/// </summary>
public interface IUserCodeGenerator
{
    /// <summary>
    /// Gets the type of the user code.
    /// </summary>
    /// <value>
    /// The type of the user code.
    /// </value>
    string UserCodeType { get; }

    /// <summary>
    /// Gets the retry limit.
    /// </summary>
    /// <value>
    /// The retry limit for getting a unique value.
    /// </value>
    int RetryLimit { get; }

    /// <summary>
    /// Generates the user code.
    /// </summary>
    /// <returns></returns>
    Task<string> GenerateAsync();
}
