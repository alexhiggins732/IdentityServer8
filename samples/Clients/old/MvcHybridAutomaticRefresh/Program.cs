/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var services = builder.Services;

services.AddMvc();
services.AddHttpClient();

services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.Cookie.Name = "mvchybridautorefresh";
    })
    .AddAutomaticTokenManagement()
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = Constants.Authority;
        options.RequireHttpsMetadata = false;

        options.ClientSecret = "secret";
        options.ClientId = "mvc.hybrid.autorefresh";

        options.ResponseType = "code id_token";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("api1");
        options.Scope.Add("offline_access");

        options.ClaimActions.MapAllExcept("iss", "nbf", "exp", "aud", "nonce", "iat", "c_hash");

        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = JwtClaimTypes.Name,
            RoleClaimType = JwtClaimTypes.Role,
        };
    });
       



using (var app = builder.Build())
{
    app.UseDeveloperExceptionPage();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseDeveloperExceptionPage();
    app.UseStaticFiles();
    app.UseAuthentication();



    app.MapControllerRoute(
               name: "default",
                      pattern: "{controller=Home}/{action=Index}/{id?}");

    await app.RunAsync();
}

