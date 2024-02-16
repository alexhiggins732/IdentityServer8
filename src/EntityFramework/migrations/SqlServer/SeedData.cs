/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.EntityFramework.DbContexts;
using IdentityServer8.EntityFramework.Mappers;
using IdentityServerHost.Configuration;

namespace SqlServer;

public class SeedData
{
    public static void EnsureSeedData(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            using (var context = scope.ServiceProvider.GetService<ConfigurationDbContext>())
            {
                EnsureSeedData(context);
            }
        }
    }

    private static void EnsureSeedData(ConfigurationDbContext context)
    {
        Console.WriteLine("Seeding database...");

        if (!context.Clients.Any())
        {
            Console.WriteLine("Clients being populated");
            foreach (var client in Clients.Get())
            {
                context.Clients.Add(client.ToEntity());
            }
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("Clients already populated");
        }

        if (!context.IdentityResources.Any())
        {
            Console.WriteLine("IdentityResources being populated");
            foreach (var resource in Resources.IdentityResources)
            {
                context.IdentityResources.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("IdentityResources already populated");
        }

        if (!context.ApiResources.Any())
        {
            Console.WriteLine("ApiResources being populated");
            foreach (var resource in Resources.ApiResources)
            {
                context.ApiResources.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("ApiResources already populated");
        }

        if (!context.ApiScopes.Any())
        {
            Console.WriteLine("Scopes being populated");
            foreach (var resource in Resources.ApiScopes)
            {
                context.ApiScopes.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("Scopes already populated");
        }

        Console.WriteLine("Done seeding database.");
        Console.WriteLine();
    }
}
