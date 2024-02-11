/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityServer8.Models;

namespace IdentityServer8.Configuration
{
    /// <summary>
    /// Options for Content Security Policy
    /// </summary>
    public class CspOptions
    {
        /// <summary>
        /// Gets or sets the minimum CSP level.
        /// </summary>
        public CspLevel Level { get; set; } = CspLevel.Two;

        /// <summary>
        /// Gets or sets a value indicating whether the deprected X-Content-Security-Policy header should be added.
        /// </summary>
        public bool AddDeprecatedHeader { get; set; } = true;
    }
}