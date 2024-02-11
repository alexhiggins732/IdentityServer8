/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Collections.Generic;

namespace IdentityServer8.Validation
{
    /// <summary>
    /// Models the result of JWT request validation.
    /// </summary>
    public class JwtRequestValidationResult : ValidationResult
    {
        /// <summary>
        /// The key/value pairs from the JWT payload of a successfuly validated request.
        /// </summary>
        public Dictionary<string, string> Payload { get; set; }
    }
}