/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Security.Claims;

namespace IdentityServer8.Validation
{
    /// <summary>
    /// Validation result for userinfo requests
    /// </summary>
    public class UserInfoRequestValidationResult : ValidationResult
    {
        /// <summary>
        /// Gets or sets the token validation result.
        /// </summary>
        /// <value>
        /// The token validation result.
        /// </value>
        public TokenValidationResult TokenValidationResult { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public ClaimsPrincipal Subject { get; set; }
    }
}