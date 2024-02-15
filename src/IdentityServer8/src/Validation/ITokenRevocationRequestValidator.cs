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
/// Interface for the token revocation request validator
/// </summary>
public interface ITokenRevocationRequestValidator
{
    /// <summary>
    /// Validates the request.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="client">The client.</param>
    /// <returns></returns>
    Task<TokenRevocationRequestValidationResult> ValidateRequestAsync(NameValueCollection parameters, Client client);
}
