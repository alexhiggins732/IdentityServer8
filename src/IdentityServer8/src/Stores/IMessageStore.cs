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
    /// Interface for a message store
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IMessageStore<TModel>
    {
        /// <summary>
        /// Writes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>An identifier for the message</returns>
        Task<string> WriteAsync(Message<TModel> message);

        /// <summary>
        /// Reads the message.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Message<TModel>> ReadAsync(string id);
    }
}