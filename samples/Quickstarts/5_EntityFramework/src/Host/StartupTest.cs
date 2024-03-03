/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Xml.Linq;
using Secret = IdentityServer8.Models.Secret;

using Microsoft.EntityFrameworkCore;
namespace IdentityServer.QuickStarts.EntityFramework;

public class StartupTests : StartupTest
{
    public StartupTests()
    {
        IsTest = true;
    }
}
public class StartupTest
{

    public static bool IsTest = false;
    public void ConfigureServices(IServiceCollection services)
    {


        services.AddScoped<ISignInHelper, HttpContextSignInHelper>();
        services.AddControllersWithViews();

        var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
        const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=IdentityServer8.Quickstart.EntityFramework-4.0.0;trusted_connection=yes;";
        //const string sqliteConnectionString = $"Filename=./Test.IdentityServer8.EntityFramework-3.1.0.db";
        services.AddIdentityServer()
            .AddTestUsers(TestUsers.Users)
            .AddConfigurationStore(options =>
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                }
                else
                {
                    options.ConfigureDbContext = b => b.UseInMemoryDatabase("EntityFramework");
                    // sql => sql.MigrationsAssembly(migrationsAssembly));

                }
            })
            .AddOperationalStore(options =>
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                }
                else
                {
                    options.ConfigureDbContext = b => b.UseInMemoryDatabase("EntityFramework");
                    // sql => sql.MigrationsAssembly(migrationsAssembly));
                }
            })
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
    }

    public void Configure(IApplicationBuilder app)
    {
        InitializeDatabase(app);

        var env = app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseStaticFiles()
            .UseRouting()
            .UseIdentityServer();
        app
            .UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });



    }
    [SupportedOSPlatformGuard("windows")]
    internal static void InitializeDatabase(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            //if (StartupTests.IsTest)
            //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //        serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            if (StartupTests.IsTest)
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    context.Database.Migrate();

            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }

}
