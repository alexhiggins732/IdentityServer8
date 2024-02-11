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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer8.Services
{
    /// <summary>
    /// Implements persisted grant logic
    /// </summary>
    public interface IPersistedGrantService
    {
        /// <summary>
        /// Gets all grants for a given subject ID.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<Grant>> GetAllGrantsAsync(string subjectId);

        /// <summary>
        /// Removes all grants for a given subject id, and optionally client id and session id combination.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier (optional).</param>
        /// <param name="sessionId">The sesion id (optional).</param>
        /// <returns></returns>
        Task RemoveAllGrantsAsync(string subjectId, string clientId = null, string sessionId = null);
    }
}