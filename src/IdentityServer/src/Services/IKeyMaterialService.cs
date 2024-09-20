/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Microsoft.IdentityModel.Tokens;

namespace IdentityServer8.Services;

/// <summary>
/// Interface for the key material service
/// </summary>
public interface IKeyMaterialService
{
    /// <summary>
    /// Gets all validation keys.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync();

    /// <summary>
    /// Gets the signing credentials.
    /// </summary>
    /// <param name="allowedAlgorithms">Collection of algorithms used to filter the server supported algorithms. 
    /// A value of null or empty indicates that the server default should be returned.</param>
    /// <returns></returns>
    Task<SigningCredentials> GetSigningCredentialsAsync(IEnumerable<string> allowedAlgorithms = null);

    /// <summary>
    /// Gets all signing credentials.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SigningCredentials>> GetAllSigningCredentialsAsync();
}
