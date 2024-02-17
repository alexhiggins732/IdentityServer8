/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add EF database support to IdentityServer.
/// </summary>
public static class IdentityServerEntityFrameworkBuilderExtensions
{
    /// <summary>
    /// Configures EF implementation of IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddConfigurationStore(
        this IIdentityServerBuilder builder,
        Action<ConfigurationStoreOptions> storeOptionsAction = null)
    {
        return builder.AddConfigurationStore<ConfigurationDbContext>(storeOptionsAction);
    }

    /// <summary>
    /// Configures EF implementation of IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
    /// </summary>
    /// <typeparam name="TContext">The IConfigurationDbContext to use.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddConfigurationStore<TContext>(
        this IIdentityServerBuilder builder,
        Action<ConfigurationStoreOptions> storeOptionsAction = null)
        where TContext : DbContext, IConfigurationDbContext
    {
        builder.Services.AddConfigurationDbContext<TContext>(storeOptionsAction);

        builder.AddClientStore<ClientStore>();
        builder.AddResourceStore<ResourceStore>();
        builder.AddCorsPolicyService<CorsPolicyService>();

        return builder;
    }

    /// <summary>
    /// Configures caching for IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddConfigurationStoreCache(
        this IIdentityServerBuilder builder)
    {
        builder.AddInMemoryCaching();

        // add the caching decorators
        builder.AddClientStoreCache<ClientStore>();
        builder.AddResourceStoreCache<ResourceStore>();
        builder.AddCorsPolicyCache<CorsPolicyService>();

        return builder;
    }

    /// <summary>
    /// Configures EF implementation of IPersistedGrantStore with IdentityServer.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddOperationalStore(
        this IIdentityServerBuilder builder,
        Action<OperationalStoreOptions> storeOptionsAction = null)
    {
        return builder.AddOperationalStore<PersistedGrantDbContext>(storeOptionsAction);
    }

    /// <summary>
    /// Configures EF implementation of IPersistedGrantStore with IdentityServer.
    /// </summary>
    /// <typeparam name="TContext">The IPersistedGrantDbContext to use.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddOperationalStore<TContext>(
        this IIdentityServerBuilder builder,
        Action<OperationalStoreOptions> storeOptionsAction = null)
        where TContext : DbContext, IPersistedGrantDbContext
    {
        builder.Services.AddOperationalDbContext<TContext>(storeOptionsAction);

        builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
        builder.Services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();
        builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();

        return builder;
    }

    /// <summary>
    /// Adds an implementation of the IOperationalStoreNotification to IdentityServer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddOperationalStoreNotification<T>(
       this IIdentityServerBuilder builder)
       where T : class, IOperationalStoreNotification
    {
        builder.Services.AddOperationalStoreNotification<T>();
        return builder;
    }
}
