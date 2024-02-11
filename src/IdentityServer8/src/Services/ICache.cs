/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System;
using System.Threading.Tasks;

namespace IdentityServer8.Services
{
    /// <summary>
    /// Abstract interface to model data caching
    /// </summary>
    /// <typeparam name="T">The data type to be cached</typeparam>
    public interface ICache<T>
        where T : class
    {
        /// <summary>
        /// Gets the cached data based upon a key index.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The cached item, or <c>null</c> if no item matches the key.</returns>
        Task<T> GetAsync(string key);

        /// <summary>
        /// Caches the data based upon a key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="expiration">The expiration.</param>
        /// <returns></returns>
        Task SetAsync(string key, T item, TimeSpan expiration);
    }
}