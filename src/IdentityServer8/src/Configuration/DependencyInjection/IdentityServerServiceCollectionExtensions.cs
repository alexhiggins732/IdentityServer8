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
/// DI extension methods for adding IdentityServer
/// </summary>
public static class IdentityServerServiceCollectionExtensions
{
    /// <summary>
    /// Creates a builder.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddIdentityServerBuilder(this IServiceCollection services)
    {
        return new IdentityServerBuilder(services);
    }

    /// <summary>
    /// Adds IdentityServer.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services)
    {
        var builder = services.AddIdentityServerBuilder();

        builder
            .AddRequiredPlatformServices()
            .AddCookieAuthentication()
            .AddCoreServices()
            .AddDefaultEndpoints()
            .AddPluggableServices()
            .AddValidators()
            .AddResponseGenerators()
            .AddDefaultSecretParsers()
            .AddDefaultSecretValidators();

        // provide default in-memory implementation, not suitable for most production scenarios
        builder.AddInMemoryPersistedGrants();

        return builder;
    }

    /// <summary>
    /// Adds IdentityServer.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="setupAction">The setup action.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, Action<IdentityServerOptions> setupAction)
    {
        services.Configure(setupAction);
        return services.AddIdentityServer();
    }

    /// <summary>
    /// Adds the IdentityServer.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServerOptions>(configuration);
        return services.AddIdentityServer();
    }

    /// <summary>
    /// Configures the OpenIdConnect handlers to persist the state parameter into the server-side IDistributedCache.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="schemes">The schemes to configure. If none provided, then all OpenIdConnect schemes will use the cache.</param>
    public static IServiceCollection AddOidcStateDataFormatterCache(this IServiceCollection services, params string[] schemes)
    {
        services.AddSingleton<IPostConfigureOptions<OpenIdConnectOptions>>(
            svcs => new ConfigureOpenIdConnectOptions(
                schemes,
                svcs.GetRequiredService<IHttpContextAccessor>())
        );

        return services;
    }
}
