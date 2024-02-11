/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace IdentityServer8.Stores
{
    /// <summary>
    /// Default signing credentials store
    /// </summary>
    /// <seealso cref="IdentityServer8.Stores.ISigningCredentialStore" />
    public class InMemorySigningCredentialsStore : ISigningCredentialStore
    {
        private readonly SigningCredentials _credential;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemorySigningCredentialsStore"/> class.
        /// </summary>
        /// <param name="credential">The credential.</param>
        public InMemorySigningCredentialsStore(SigningCredentials credential)
        {
            _credential = credential;
        }

        /// <summary>
        /// Gets the signing credentials.
        /// </summary>
        /// <returns></returns>
        public Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            return Task.FromResult(_credential);
        }
    }
}