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
/// The default validation key store
/// </summary>
/// <seealso cref="IdentityServer8.Stores.IValidationKeysStore" />
public class InMemoryValidationKeysStore : IValidationKeysStore
{
    private readonly IEnumerable<SecurityKeyInfo> _keys;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryValidationKeysStore"/> class.
    /// </summary>
    /// <param name="keys">The keys.</param>
    /// <exception cref="System.ArgumentNullException">keys</exception>
    public InMemoryValidationKeysStore(IEnumerable<SecurityKeyInfo> keys)
    {
        _keys = keys ?? throw new ArgumentNullException(nameof(keys));
    }

    /// <summary>
    /// Gets all validation keys.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
    {
        return Task.FromResult(_keys);
    }
}
