/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

Console.Title = "IdentityServer8";

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
});
var services = builder.Services;

//var name = "CN=test.dataprotection";
//var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find(name, false).FirstOrDefault();

var cn = builder.Configuration.GetConnectionString("db");
var Environment = builder.Environment;
services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Environment.ContentRootPath, "dataprotectionkeys")));
//.PersistKeysToDatabase(new DatabaseKeyManagementOptions
//{
//    ConfigureDbContext = b => b.UseSqlServer(cn),
//    LoggerFactory = LoggerFactory,
//});
//.ProtectKeysWithCertificate(cert);

services.AddIdentityServer()
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
var app = builder.Build();

if (Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseIdentityServer();

await app.RunAsync();


