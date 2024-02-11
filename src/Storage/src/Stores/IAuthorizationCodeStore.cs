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
using System.Threading.Tasks;

namespace IdentityServer8.Stores
{
    /// <summary>
    /// Interface for the authorization code store
    /// </summary>
    public interface IAuthorizationCodeStore
    {
        /// <summary>
        /// Stores the authorization code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        Task<string> StoreAuthorizationCodeAsync(AuthorizationCode code);

        /// <summary>
        /// Gets the authorization code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        Task<AuthorizationCode> GetAuthorizationCodeAsync(string code);

        /// <summary>
        /// Removes the authorization code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        Task RemoveAuthorizationCodeAsync(string code);
   }
}