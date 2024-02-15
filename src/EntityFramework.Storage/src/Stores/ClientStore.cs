/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.Models;

namespace IdentityServer8.EntityFramework.Stores
{
    /// <summary>
    /// Implementation of IClientStore thats uses EF.
    /// </summary>
    /// <seealso cref="IdentityServer8.Stores.IClientStore" />
    public class ClientStore : IClientStore
    {
        /// <summary>
        /// The DbContext.
        /// </summary>
        protected readonly IConfigurationDbContext Context;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<ClientStore> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public ClientStore(IConfigurationDbContext context, ILogger<ClientStore> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

        /// <summary>
        /// Finds a client by id
        /// </summary>
        /// <param name="clientId">The client id</param>
        /// <returns>
        /// The client
        /// </returns>
        public virtual async Task<Client> FindClientByIdAsync(string clientId)
        {
            IQueryable<Entities.Client> baseQuery = Context.Clients
                .Where(x => x.ClientId == clientId);

            var client = (await baseQuery.ToArrayAsync())
                .SingleOrDefault(x => x.ClientId == clientId);
            if (client == null) return null;

            await baseQuery.Include(x => x.AllowedCorsOrigins).SelectMany(c => c.AllowedCorsOrigins).LoadAsync();
            await baseQuery.Include(x => x.AllowedGrantTypes).SelectMany(c => c.AllowedGrantTypes).LoadAsync();
            await baseQuery.Include(x => x.AllowedScopes).SelectMany(c => c.AllowedScopes).LoadAsync();
            await baseQuery.Include(x => x.Claims).SelectMany(c => c.Claims).LoadAsync();
            await baseQuery.Include(x => x.ClientSecrets).SelectMany(c => c.ClientSecrets).LoadAsync();
            await baseQuery.Include(x => x.IdentityProviderRestrictions).SelectMany(c => c.IdentityProviderRestrictions).LoadAsync();
            await baseQuery.Include(x => x.PostLogoutRedirectUris).SelectMany(c => c.PostLogoutRedirectUris).LoadAsync();
            await baseQuery.Include(x => x.Properties).SelectMany(c => c.Properties).LoadAsync();
            await baseQuery.Include(x => x.RedirectUris).SelectMany(c => c.RedirectUris).LoadAsync();

            var model = client.ToModel();

            Logger.LogDebug("{clientId} found in database: {clientIdFound}", Ioc.Sanitizer.Log.Sanitize(clientId), model != null);

            return model;
        }
    }
}
