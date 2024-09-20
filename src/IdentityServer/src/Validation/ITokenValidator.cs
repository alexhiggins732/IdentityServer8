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
/// Interface for the token validator
/// </summary>
public interface ITokenValidator
{
    /// <summary>
    /// Validates an access token.
    /// </summary>
    /// <param name="token">The access token.</param>
    /// <param name="expectedScope">The expected scope.</param>
    /// <returns></returns>
    Task<TokenValidationResult> ValidateAccessTokenAsync(string token, string expectedScope = null);
    
    /// <summary>
    /// Validates an identity token.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="validateLifetime">if set to <c>true</c> the lifetime gets validated. Otherwise not.</param>
    /// <returns></returns>
    Task<TokenValidationResult> ValidateIdentityTokenAsync(string token, string clientId = null, bool validateLifetime = true);
}
