/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.Configuration;
using IdentityServer8.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.IntegrationTests.Clients.Setup;

public class StartupWithCustomTokenResponses
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication();

        var builder = services.AddIdentityServer(options =>
        {
            options.IssuerUri = "https://idsvr8";

            options.Events = new EventsOptions
            {
                RaiseErrorEvents = true,
                RaiseFailureEvents = true,
                RaiseInformationEvents = true,
                RaiseSuccessEvents = true
            };
        });

        builder.AddInMemoryClients(Clients.Get());
        builder.AddInMemoryIdentityResources(Scopes.GetIdentityScopes());
        builder.AddInMemoryApiResources(Scopes.GetApiResources());
        builder.AddInMemoryApiScopes(Scopes.GetApiScopes());

        builder.AddDeveloperSigningCredential(persistKey: false);

        services.AddTransient<IResourceOwnerPasswordValidator, CustomResponseResourceOwnerValidator>();
        builder.AddExtensionGrantValidator<CustomResponseExtensionGrantValidator>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseIdentityServer();
    }
}
