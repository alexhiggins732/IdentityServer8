/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace ConsoleHost;

class Program
{
    static void Main(string[] args)
    {
        var connectionString = "server=(localdb)\\mssqllocaldb;database=IdentityServer8.EntityFramework-8.0.0;trusted_connection=yes;";

        var services = new ServiceCollection();
        services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace));
        services.AddOperationalDbContext(options =>
        {
            options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);

            // this enables automatic token cleanup. this is optional.
            options.EnableTokenCleanup = false;
            options.TokenCleanupInterval = 5; // interval in seconds, short for testing
        });

        var sp = services.BuildServiceProvider();
        using (var scope = sp.CreateScope())
        {
            var svc = scope.ServiceProvider.GetRequiredService<TokenCleanupService>();
            svc.RemoveExpiredGrantsAsync().GetAwaiter().GetResult();
        }
    }
}
