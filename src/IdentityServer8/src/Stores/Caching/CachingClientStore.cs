/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityServer8.Extensions;
using IdentityServer8.Models;
using IdentityServer8.Services;
using System.Threading.Tasks;
using IdentityServer8.Configuration;
using Microsoft.Extensions.Logging;

namespace IdentityServer8.Stores
{
    /// <summary>
    /// Cache decorator for IClientStore
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IdentityServer8.Stores.IClientStore" />
    public class CachingClientStore<T> : IClientStore
        where T : IClientStore
    {
        private readonly IdentityServerOptions _options;
        private readonly ICache<Client> _cache;
        private readonly IClientStore _inner;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingClientStore{T}"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="inner">The inner.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="logger">The logger.</param>
        public CachingClientStore(IdentityServerOptions options, T inner, ICache<Client> cache, ILogger<CachingClientStore<T>> logger)
        {
            _options = options;
            _inner = inner;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Finds a client by id
        /// </summary>
        /// <param name="clientId">The client id</param>
        /// <returns>
        /// The client
        /// </returns>
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await _cache.GetAsync(clientId,
                _options.Caching.ClientStoreExpiration,
                async () => await _inner.FindClientByIdAsync(clientId),
                _logger);

            return client;
        }
    }
}