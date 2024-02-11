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
using IdentityServer8.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IdentityServer8.Validation
{
    /// <summary>
    /// Parser for finding the best secret in an Enumerable List
    /// </summary>
    public interface ISecretsListParser
    {
        /// <summary>
        /// Tries to find the best secret on the context that can be used for authentication
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>
        /// A parsed secret
        /// </returns>
        Task<ParsedSecret> ParseAsync(HttpContext context);

        /// <summary>
        /// Gets all available authentication methods.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAvailableAuthenticationMethods();
    }
}
