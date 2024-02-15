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
    var builder = WebApplication.CreateBuilder(args.Where(x => x != "/seed").ToArray());

    var services = builder.Services;

    services.AddControllersWithViews();

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    services.AddIdentityServer(options =>
         {
             options.Events.RaiseErrorEvents = true;
             options.Events.RaiseInformationEvents = true;
             options.Events.RaiseFailureEvents = true;
             options.Events.RaiseSuccessEvents = true;

             // see https://IdentityServer8.readthedocs.io/en/latest/topics/resources.html
             options.EmitStaticAudienceClaim = true;
         })
         .AddInMemoryIdentityResources(Config.IdentityResources)
         .AddInMemoryApiScopes(Config.ApiScopes)
         .AddInMemoryClients(Config.Clients)
         .AddAspNetIdentity<ApplicationUser>()
         // not recommended for production - you need to store your key material somewhere secure
         .AddDeveloperSigningCredential();


    services.AddAuthentication()
        .AddGoogle(options =>
        {
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            // register your IdentityServer with Google at https://console.developers.google.com
            // enable the Google+ API
            // set the redirect URI to https://localhost:5001/signin-google
            options.ClientId = "copy client ID from Google here";
            options.ClientSecret = "copy client secret from Google here";
        });


    using (var app = builder.Build())
    {


        if (args.Contains("/seed"))
        {
            Log.Information("Seeding database...");
            var config = app.Services.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            SeedData.EnsureSeedData(connectionString);
            Log.Information("Done seeding database.");
            return 0;
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
            app.UseDatabaseErrorPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }


        app.UseStaticFiles();

        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.MapDefaultControllerRoute();


        Log.Information("Starting host...");

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
