/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for registering the local access token authentication handler
/// </summary>
public static class LocalApiAuthenticationExtensions
{
    /// <summary>
    /// Adds support for local APIs
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="transformationFunc">Function to transform the resulting principal</param>
    /// <returns></returns>
    public static IServiceCollection AddLocalApiAuthentication(this IServiceCollection services, Func<ClaimsPrincipal, Task<ClaimsPrincipal>> transformationFunc = null)
    {
        services.AddAuthentication()
            .AddLocalApi(options =>
            {
                options.ExpectedScope = IdentityServerConstants.LocalApi.ScopeName;

                if (transformationFunc != null)
                {
                    options.Events = new LocalApiAuthenticationEvents
                    {
                        OnClaimsTransformation = async e =>
                        {
                            e.Principal = await transformationFunc(e.Principal);
                        }
                    };
                }
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
            {
                policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        });

        return services;
    }

    /// <summary>
    /// Registers the authentication handler for local APIs.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static AuthenticationBuilder AddLocalApi(this AuthenticationBuilder builder)
        => builder.AddLocalApi(IdentityServerConstants.LocalApi.AuthenticationScheme, _ => { });

    /// <summary>
    /// Registers the authentication handler for local APIs.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configureOptions">The configure options.</param>
    /// <returns></returns>
    public static AuthenticationBuilder AddLocalApi(this AuthenticationBuilder builder, Action<LocalApiAuthenticationOptions> configureOptions)
        => builder.AddLocalApi(IdentityServerConstants.LocalApi.AuthenticationScheme, configureOptions);

    /// <summary>
    /// Registers the authentication handler for local APIs.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="authenticationScheme">The authentication scheme.</param>
    /// <param name="configureOptions">The configure options.</param>
    /// <returns></returns>
    public static AuthenticationBuilder AddLocalApi(this AuthenticationBuilder builder, string authenticationScheme, Action<LocalApiAuthenticationOptions> configureOptions)
        => builder.AddLocalApi(authenticationScheme, displayName: null, configureOptions: configureOptions);

    /// <summary>
    /// Registers the authentication handler for local APIs.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="authenticationScheme">The authentication scheme.</param>
    /// <param name="displayName">The display name of this scheme.</param>
    /// <param name="configureOptions">The configure options.</param>
    /// <returns></returns>
    public static AuthenticationBuilder AddLocalApi(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<LocalApiAuthenticationOptions> configureOptions)
    {
        return builder.AddScheme<LocalApiAuthenticationOptions, LocalApiAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }
}
