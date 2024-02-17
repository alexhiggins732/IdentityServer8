/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Microsoft.AspNetCore.Mvc;

Console.Title = "API";

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// accepts any access token issued by identity server
// adds an authorization policy for scope 'api1'
services
    .AddAuthorization(options =>
    {
        options.AddPolicy("ApiScope", policy =>
        {
            policy
                .RequireAuthenticatedUser()
                .RequireClaim("scope", "api1");
        });
    })
    .AddCors(options =>
    {
        // this defines a CORS policy called "default"
        options.AddPolicy("default", policy =>
        {
            policy.WithOrigins("https://localhost:5003")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    })
    .AddControllers();

// accepts any access token issued by identity server
services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001";
        options.TokenValidationParameters =
            new() { ValidateAudience = false };
    });



using (var app = builder.Build())
{
    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseCors("default");

    app.MapGet("/identity", (HttpContext context) =>
            new JsonResult(context?.User?.Claims.Select(c => new { c.Type, c.Value }))
        ).RequireAuthorization("ApiScope");

    await app.RunAsync();
}
