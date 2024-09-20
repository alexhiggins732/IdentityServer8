/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Stores;

/// <summary>
/// In-memory persisted grant store
/// </summary>
public class InMemoryPersistedGrantStore : IPersistedGrantStore
{
    private readonly ConcurrentDictionary<string, PersistedGrant> _repository = new ConcurrentDictionary<string, PersistedGrant>();

    /// <inheritdoc/>
    public Task StoreAsync(PersistedGrant grant)
    {
        _repository[grant.Key] = grant;

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<PersistedGrant> GetAsync(string key)
    {
        if (_repository.TryGetValue(key, out PersistedGrant token))
        {
            return Task.FromResult(token);
        }

        return Task.FromResult<PersistedGrant>(null);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
    {
        filter.Validate();
        
        var items = Filter(filter);
        
        return Task.FromResult(items);
    }

    /// <inheritdoc/>
    public Task RemoveAsync(string key)
    {
        _repository.TryRemove(key, out _);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task RemoveAllAsync(PersistedGrantFilter filter)
    {
        filter.Validate();

        var items = Filter(filter);
        
        foreach (var item in items)
        {
            _repository.TryRemove(item.Key, out _);
        }

        return Task.CompletedTask;
    }

    private IEnumerable<PersistedGrant> Filter(PersistedGrantFilter filter)
    {
        var query =
            from item in _repository
            select item.Value;

        if (!String.IsNullOrWhiteSpace(filter.ClientId))
        {
            query = query.Where(x => x.ClientId == filter.ClientId);
        }
        if (!String.IsNullOrWhiteSpace(filter.SessionId))
        {
            query = query.Where(x => x.SessionId == filter.SessionId);
        }
        if (!String.IsNullOrWhiteSpace(filter.SubjectId))
        {
            query = query.Where(x => x.SubjectId == filter.SubjectId);
        }
        if (!String.IsNullOrWhiteSpace(filter.Type))
        {
            query = query.Where(x => x.Type == filter.Type);
        }

        var items = query.ToArray().AsEnumerable();
        return items;
    }
}
