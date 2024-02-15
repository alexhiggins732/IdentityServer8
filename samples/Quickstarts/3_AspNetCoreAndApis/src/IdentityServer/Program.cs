/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

ConfigureLogger();

try
{
    Log.Information("Starting host...");

    var builder = WebApplication.CreateBuilder(args);

    var services = builder.Services;

    services.AddControllersWithViews();

    services
        .AddIdentityServer()
        .AddInMemoryIdentityResources(Config.IdentityResources)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryClients(Config.Clients)
        .AddTestUsers(TestUsers.Users)
        .AddDeveloperSigningCredential();

    services.AddAuthentication()
        .AddGoogle("Google", options =>
        {
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            options.ClientId = "<insert here>";
            options.ClientSecret = "<insert here>";
        })
        .AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
        {
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            options.SignOutScheme = IdentityServerConstants.SignoutScheme;
            options.SaveTokens = true;

            options.Authority = "https://demo.identityserver8.io/";
            options.ClientId = "interactive.confidential";
            options.ClientSecret = "secret";
            options.ResponseType = "code";

            options.TokenValidationParameters = new()
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        });


    using (var app = builder.Build())
    {
        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseStaticFiles()
            .UseRouting()
            .UseIdentityServer()
            .UseAuthorization();

        app.MapDefaultControllerRoute();

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
