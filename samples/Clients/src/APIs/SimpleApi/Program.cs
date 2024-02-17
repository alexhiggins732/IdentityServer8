/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

ConfigureLogger();

Console.Title = "Resource API";

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

var services = builder.Services;

services.AddControllers();

services.AddCors()
    .AddDistributedMemoryCache()
    .AddScopeTransformation()
    .AddAuthentication("token")

    // JWT tokens
    .AddJwtBearer("token", options =>
    {
        options.Authority = Constants.Authority;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    })

    // Use multiple token validation handlers:
    // https://stackoverflow.com/questions/63399810/can-i-create-an-identity-server-4-asp-net-core-api-using-2-different-token-authe
    //https://leastprivilege.com/2020/07/06/flexible-access-token-validation-in-asp-net-core/
    //https://leastprivilege.com/2020/06/15/the-jwt-profile-for-oauth-2-0-access-tokens-and-identityserver/
    // reference tokens
    .AddOAuth2Introspection("introspection", options =>
    {
        options.Authority = Constants.Authority;
        options.ClientId = "resource1";
        options.ClientSecret = "secret";
    });



using (var app = builder.Build())
{
    app
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseCors(policy =>
        {
            policy.WithOrigins("https://localhost:44300");

            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.WithExposedHeaders("WWW-Authenticate");
        });



    app.MapGet("/identity", (HttpContext ctx, ILogger logger) =>
    {
        var claims = ctx.User.Claims.Select(c => new { c.Type, c.Value });
        logger.LogInformation("claims: {claims}", claims);
        return Results.Json(claims);
    });

    await app.RunAsync();
}


void ConfigureLogger()
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
        .CreateLogger();
}
