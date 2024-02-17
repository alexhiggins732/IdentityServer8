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

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "oidc";
        })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie.Name = "mvcimplicit";
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = Constants.Authority;
                options.RequireHttpsMetadata = false;

                options.ClientId = "mvc.implicit";

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                options.SaveTokens = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                };

                options.Events.OnRedirectToIdentityProvider = e =>
                {
                    var jwt = CreateJwtRequest(
                        options.ClientId,
                        options.Authority,
                        new Claim("additional_data", "data"));

                    e.ProtocolMessage.SetParameter("request", jwt);

                    return Task.CompletedTask;
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

    private static string CreateJwtRequest(string clientId, string audience, params Claim[] claims)
    {
        var certificate = new X509Certificate2("client.pfx");
        var now = DateTime.UtcNow;

        var token = new JwtSecurityToken(
                clientId,
                audience,
                claims,
                now,
                now.AddMinutes(1),
                new SigningCredentials(
                    new X509SecurityKey(certificate),
                    SecurityAlgorithms.RsaSha256
                )
            );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}
