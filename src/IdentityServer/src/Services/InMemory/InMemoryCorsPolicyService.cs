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
/// CORS policy service that configures the allowed origins from a list of clients' redirect URLs.
/// </summary>
public class InMemoryCorsPolicyService : ICorsPolicyService
{
    /// <summary>
    /// Logger
    /// </summary>
    protected readonly ILogger Logger;
    /// <summary>
    /// Clients applications list
    /// </summary>
    protected readonly IEnumerable<Client> Clients;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryCorsPolicyService"/> class.
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="clients">The clients.</param>
    public InMemoryCorsPolicyService(ILogger<InMemoryCorsPolicyService> logger, IEnumerable<Client> clients)
    {
        Logger = logger;
        Clients = clients ?? Enumerable.Empty<Client>();
    }

    /// <summary>
    /// Determines whether origin is allowed.
    /// </summary>
    /// <param name="origin">The origin.</param>
    /// <returns></returns>
    public virtual Task<bool> IsOriginAllowedAsync(string origin)
    {
        var query =
            from client in Clients
            from url in client.AllowedCorsOrigins
            select url.GetOrigin();

        var result = query.Contains(origin, StringComparer.OrdinalIgnoreCase);

        if (result)
        {
            Logger.LogDebug("Client list checked and origin: {0} is allowed", Ioc.Sanitizer.Log.Sanitize(origin));
        }
        else
        {
            Logger.LogDebug("Client list checked and origin: {0} is not allowed", Ioc.Sanitizer.Log.Sanitize(origin));
        }

        return Task.FromResult(result);
    }
}
