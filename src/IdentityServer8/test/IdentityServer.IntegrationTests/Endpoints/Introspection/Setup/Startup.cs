/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer.IntegrationTests.Endpoints.Introspection.Setup;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var builder = services.AddIdentityServer(options =>
        {
            options.IssuerUri = "https://idsvr8";
            options.Endpoints.EnableAuthorizeEndpoint = false;
        });

        builder.AddInMemoryClients(Clients.Get());
        builder.AddInMemoryApiResources(Scopes.GetApis());
        builder.AddInMemoryApiScopes(Scopes.GetScopes());
        builder.AddTestUsers(Users.Get());
        builder.AddDeveloperSigningCredential(persistKey: false);
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.UseIdentityServer();
    }
}
