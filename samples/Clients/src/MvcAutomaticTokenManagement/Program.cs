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
    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    var services = builder.Services;


    // add MVC
    services.AddControllersWithViews();

    // add cookie-based session management with OpenID Connect authentication
    services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
    })
        .AddCookie("cookie", options =>
        {
            options.Cookie.Name = "mvcclient";

            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = false;

            // could be used to automatically trigger re-authentication (if you want to do that at the pipeline level)
            //options.Events.OnValidatePrincipal = async e =>
            //{
            //    var currentToken = await e.HttpContext.GetAccessTokenAsync();

            //    if (string.IsNullOrWhiteSpace(currentToken))
            //    {
            //        e.RejectPrincipal();
            //    }
            //};

            options.Events.OnSigningOut = async e =>
            {
                // automatically revoke refresh token at signout time
                await e.HttpContext.RevokeUserRefreshTokenAsync();
            };
        })
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = Constants.Authority;
            options.RequireHttpsMetadata = false;

            options.ClientId = "mvc.tokenmanagement";
            options.ClientSecret = "secret";

            // code flow + PKCE (PKCE is turned on by default)
            options.ResponseType = "code";
            options.UsePkce = true;

            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("resource1.scope1");
            options.Scope.Add("offline_access");

            // keeps id_token smaller
            options.GetClaimsFromUserInfoEndpoint = true;
            options.SaveTokens = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        });

    // add automatic token management
    services.AddAccessTokenManagement();

    // add HTTP client to call protected API
    services.AddUserAccessTokenHttpClient("client", configureClient: client =>
    {
        client.BaseAddress = new Uri(Constants.SampleApi);
    });

    using (var app = builder.Build())
    {
        app
            .UseSerilogRequestLogging()
            .UseDeveloperExceptionPage()
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization();

        app.MapDefaultControllerRoute().RequireAuthorization();

        await app.RunAsync();
        return 0;
    }
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

Serilog.ILogger ConfigureLogger() =>
    Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .MinimumLevel.Override("IdentityModel", LogEventLevel.Debug)
    .MinimumLevel.Override("System.Net.Http", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    .CreateLogger();