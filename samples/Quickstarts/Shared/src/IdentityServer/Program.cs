/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Secret = IdentityServer8.Models.Secret;

ConfigureLogger();

try
{
    Log.Information("Starting host...");

    var builder = WebApplication.CreateBuilder(args);

    var services = builder.Services;

    // uncomment, if you want to add an MVC-based UI
    //services.AddControllersWithViews();

    services
        .AddIdentityServer()
        .AddInMemoryApiScopes(new ApiScope[] { new("api1", "My API") })
        .AddDeveloperSigningCredential()
        .AddInMemoryClients(new Client[] { new()
            {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets = new Secret[] {new ("secret".Sha256()) },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
            }
        });


    using (var app = builder.Build())
    {
        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        // uncomment if you want to add MVC
        //app.UseStaticFiles();
        //app.UseRouting();

        app.UseIdentityServer();

        // uncomment, if you want to add MVC-based
        //app.UseAuthorization();
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapDefaultControllerRoute();
        //});

        await app.RunAsync();
    }

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}


void ConfigureLogger() => Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .Enrich.FromLogContext()
    // uncomment to write to Azure diagnostics stream
    //.WriteTo.File(
    //    @"D:\home\LogFiles\Application\identityserver.txt",
    //    fileSizeLimitBytes: 1_000_000,
    //    rollOnFileSizeLimit: true,
    //    shared: true,
    //    flushToDiskInterval: TimeSpan.FromSeconds(1))
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
    .CreateLogger();
