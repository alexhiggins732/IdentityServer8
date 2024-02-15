/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Stores;

/// <summary>
/// Interface for reference token storage
/// </summary>
public interface IReferenceTokenStore
{
    /// <summary>
    /// Stores the reference token.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<string> StoreReferenceTokenAsync(Token token);

    /// <summary>
    /// Gets the reference token.
    /// </summary>
    /// <param name="handle">The handle.</param>
    /// <returns></returns>
    Task<Token> GetReferenceTokenAsync(string handle);

    /// <summary>
    /// Removes the reference token.
    /// </summary>
    /// <param name="handle">The handle.</param>
    /// <returns></returns>
    Task RemoveReferenceTokenAsync(string handle);

    /// <summary>
    /// Removes the reference tokens.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <returns></returns>
    Task RemoveReferenceTokensAsync(string subjectId, string clientId);
}
