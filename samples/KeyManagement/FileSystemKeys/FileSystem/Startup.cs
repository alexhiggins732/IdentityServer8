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
    public IWebHostEnvironment Environment { get; }

    public Startup(IConfiguration config, IWebHostEnvironment environment)
    {
        Environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var name = "CN=test.dataprotection";
        var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find(name, false).FirstOrDefault();

        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Environment.ContentRootPath, "dataprotectionkeys")));
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
                    // https://www.identityserver.com/products/KeyManagement
                    options.Licensee = "your licensee";
                    options.License = "your license key";
                })
                //.EnableInMemoryCaching()
                .PersistKeysToFileSystem(Path.Combine(Environment.ContentRootPath, @"signingkeys"))
                .ProtectKeysWithDataProtection();

                // .PersistKeysWith<TYourStore>() // use this when you implement your own ISigningKeyStore
                //.EnableInMemoryCaching() // caching disabled unless explicitly enabled
                // run "..\cert\cert.ps1" from a powershell prompt to create new cert/pfx
                // put the pfx created in the local machine store
                //.ProtectKeysWithX509Certificate("CN=SigningKeysMasterKey")
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
