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
using System.Threading.Tasks;

namespace IdentityServer8.Stores
{
    /// <summary>
    /// Interface for authorization request messages that are sent from the authorization endpoint to the login and consent UI.
    /// </summary>
    public interface IAuthorizationParametersMessageStore
    {
        /// <summary>
        /// Writes the authorization parameters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The identifier for the stored message.</returns>
        Task<string> WriteAsync(Message<IDictionary<string, string[]>> message);

        /// <summary>
        /// Reads the authorization parameters.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Message<IDictionary<string, string[]>>> ReadAsync(string id);

        /// <summary>
        /// Deletes the authorization parameters.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task DeleteAsync(string id);
    }
}