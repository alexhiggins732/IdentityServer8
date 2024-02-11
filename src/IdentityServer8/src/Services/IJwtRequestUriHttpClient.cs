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
using IdentityServer8.Models;

namespace IdentityServer8.Services
{
    /// <summary>
    /// Models making HTTP requests for JWTs from the authorize endpoint.
    /// </summary>
    public interface IJwtRequestUriHttpClient
    {
        /// <summary>
        /// Gets a JWT from the url.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<string> GetJwtAsync(string url, Client client);
    }
}