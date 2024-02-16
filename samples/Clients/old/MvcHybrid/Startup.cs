/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/
public class Startup
{
    public Startup()
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddHttpClient();

        services.AddSingleton<IDiscoveryCache>(r =>
        {
            var factory = r.GetRequiredService<IHttpClientFactory>();
            return new DiscoveryCache(Constants.Authority, () => factory.CreateClient());
        });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "oidc";
        })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie.Name = "mvchybrid";
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = Constants.Authority;
                options.RequireHttpsMetadata = false;

                options.ClientSecret = "secret";
                options.ClientId = "mvc.hybrid";

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
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}
