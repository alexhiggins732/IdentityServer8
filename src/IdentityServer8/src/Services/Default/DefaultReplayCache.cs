/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Services;

/// <summary>
/// Default implementation of the replay cache using IDistributedCache
/// </summary>
public class DefaultReplayCache : IReplayCache
{
    private const string Prefix = nameof(DefaultReplayCache) + ":";
    
    private readonly IDistributedCache _cache;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="cache"></param>
    public DefaultReplayCache(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    /// <inheritdoc />
    public async Task AddAsync(string purpose, string handle, DateTimeOffset expiration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = expiration
        };
        
        await _cache.SetAsync(Prefix + purpose + handle, new byte[] { }, options);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string purpose, string handle)
    {
        return (await _cache.GetAsync(Prefix + purpose + handle, default)) != null;
    }
}
