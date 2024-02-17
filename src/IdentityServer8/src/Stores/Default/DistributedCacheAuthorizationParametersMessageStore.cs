/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Stores.Default;

/// <summary>
/// Implementation of IAuthorizationParametersMessageStore that uses the IDistributedCache.
/// </summary>
public class DistributedCacheAuthorizationParametersMessageStore : IAuthorizationParametersMessageStore
{
    private readonly IDistributedCache _distributedCache;
    private readonly IHandleGenerationService _handleGenerationService;

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="distributedCache"></param>
    /// <param name="handleGenerationService"></param>
    public DistributedCacheAuthorizationParametersMessageStore(IDistributedCache distributedCache, IHandleGenerationService handleGenerationService)
    {
        _distributedCache = distributedCache;
        _handleGenerationService = handleGenerationService;
    }

    private string CacheKeyPrefix => "DistributedCacheAuthorizationParametersMessageStore";
    
    /// <inheritdoc/>
    public async Task<string> WriteAsync(Message<IDictionary<string, string[]>> message)
    {
        // since this store is trusted and the JWT request processing has provided redundant entries
        // in the NameValueCollection, we are removing the JWT "request_uri" param so that when they
        // are reloaded/revalidated we don't re-trigger outbound requests. we could possibly do the
        // same for the "request" param, but it's less of a concern (as it's just a signature check).
        message.Data.Remove(OidcConstants.AuthorizeRequest.RequestUri);

        var key = await _handleGenerationService.GenerateAsync();
        var cacheKey = $"{CacheKeyPrefix}-{key}";
        
        var json = ObjectSerializer.ToString(message);

        var options = new DistributedCacheEntryOptions();
        options.SetSlidingExpiration(Constants.DefaultCacheDuration);

        await _distributedCache.SetStringAsync(cacheKey, json, options);

        return key;
    }

    /// <inheritdoc/>
    public async Task<Message<IDictionary<string, string[]>>> ReadAsync(string id)
    {
        var cacheKey = $"{CacheKeyPrefix}-{id}";
        var json = await _distributedCache.GetStringAsync(cacheKey);

        if (json == null)
        {
            return new Message<IDictionary<string, string[]>>(new Dictionary<string, string[]>());
        }

        return ObjectSerializer.FromString<Message<IDictionary<string, string[]>>>(json);
    }

    /// <inheritdoc/>
    public Task DeleteAsync(string id)
    {
        return _distributedCache.RemoveAsync(id);
    }
}
