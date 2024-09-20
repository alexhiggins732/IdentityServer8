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
/// In-memory client store
/// </summary>
public class InMemoryClientStore : IClientStore
{
    private readonly IEnumerable<Client> _clients;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryClientStore"/> class.
    /// </summary>
    /// <param name="clients">The clients.</param>
    public InMemoryClientStore(IEnumerable<Client> clients)
    {
        if (clients.HasDuplicates(m => m.ClientId))
        {
            throw new ArgumentException("Clients must not contain duplicate ids");
        }
        _clients = clients;
    }

    /// <summary>
    /// Finds a client by id
    /// </summary>
    /// <param name="clientId">The client id</param>
    /// <returns>
    /// The client
    /// </returns>
    public Task<Client> FindClientByIdAsync(string clientId)
    {
        var query =
            from client in _clients
            where client.ClientId == clientId
            select client;
        
        return Task.FromResult(query.SingleOrDefault());
    }
}
