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
    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }
    public ILoggerFactory LoggerFactory { get; set; }

    public Startup(IConfiguration config, IWebHostEnvironment environment, ILoggerFactory loggerFactory)
    {
        Configuration = config;
        Environment = environment;
        LoggerFactory = loggerFactory;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //var name = "CN=test.dataprotection";
        //var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find(name, false).FirstOrDefault();

        var cn = Configuration.GetConnectionString("db");

        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Environment.ContentRootPath, "dataprotectionkeys")));
        //.PersistKeysToDatabase(new DatabaseKeyManagementOptions
        //{
        //    ConfigureDbContext = b => b.UseSqlServer(cn),
        //    LoggerFactory = LoggerFactory,
        //});
        //.ProtectKeysWithCertificate(cert);

        var builder = services.AddIdentityServer()
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryApiResources(Config.GetApis())
            .AddInMemoryClients(Config.GetClients())
            .AddSigningKeyManagement(
                options => // configuring options is optional :)
                {
                    options.DeleteRetiredKeys = true;
                    options.KeyType = IdentityServer4.KeyManagement.KeyType.RSA;

                    // all of these values in here are changed for local testing
                    options.InitializationDuration = TimeSpan.FromSeconds(5);
                    options.InitializationSynchronizationDelay = TimeSpan.FromSeconds(1);

                    options.KeyActivationDelay = TimeSpan.FromSeconds(10);
                    options.KeyExpiration = options.KeyActivationDelay * 2;
                    options.KeyRetirement = options.KeyActivationDelay * 3;

                    // You can get your own license from:
                    // https://www.identityserver8.com/products/KeyManagement
                    options.Licensee = "your licensee";
                    options.License = "your license key";
                })
                .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Environment.ContentRootPath, "signingkeys")).FullName)
                //.PersistKeysToDatabase(new DatabaseKeyManagementOptions {
                //    ConfigureDbContext = b => b.UseSqlServer(cn),
                //})
                .ProtectKeysWithDataProtection()
            //.EnableInMemoryCaching() // caching disabled unless explicitly enabled
            ;
    }

    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseIdentityServer();
    }
}

