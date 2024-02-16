/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllersWithViews();

services
.AddHttpClient()
.AddSingleton<IDiscoveryCache>(r =>
{
    var factory = r.GetRequiredService<IHttpClientFactory>();
    return new DiscoveryCache(Constants.Authority, () => factory.CreateClient());
}
)
.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie(options =>
{
    options.Cookie.Name = "mvccode";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = Constants.Authority;
    options.RequireHttpsMetadata = false;

    options.ClientId = "mvc.code";
    options.ClientSecret = "secret";

    // code flow + PKCE (PKCE is turned on by default)
    options.ResponseType = "code";
    options.UsePkce = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("resource1.scope1");
    options.Scope.Add("transaction:123");
    //options.Scope.Add("transaction");
    options.Scope.Add("offline_access");

    // not mapped by default
    options.ClaimActions.MapJsonKey("website", "website");

    // keeps id_token smaller
    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = JwtClaimTypes.Name,
        RoleClaimType = JwtClaimTypes.Role,
    };
});

var app = builder.Build();

app.UseSerilogRequestLogging()
.UseDeveloperExceptionPage()
.UseHttpsRedirection()
.UseStaticFiles()
.UseRouting()
.UseAuthentication()
.UseAuthorization();

app.MapDefaultControllerRoute().RequireAuthorization();

await app.RunAsync();
