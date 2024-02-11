/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Threading.Tasks;

namespace IdentityServer8.Validation
{
    /// <summary>
    /// Allows inserting custom token validation logic
    /// </summary>
    public interface ICustomTokenValidator
    {
        /// <summary>
        /// Custom validation logic for access tokens.
        /// </summary>
        /// <param name="result">The validation result so far.</param>
        /// <returns>The validation result</returns>
        Task<TokenValidationResult> ValidateAccessTokenAsync(TokenValidationResult result);

        /// <summary>
        /// Custom validation logic for identity tokens.
        /// </summary>
        /// <param name="result">The validation result so far.</param>
        /// <returns>The validation result</returns>
        Task<TokenValidationResult> ValidateIdentityTokenAsync(TokenValidationResult result);
    }
}