/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using Xunit;

namespace IdentityServer8.EntityFramework.IntegrationTests;

/// <summary>
/// Base class for integration tests, responsible for initializing test database providers & an xUnit class fixture
/// </summary>
/// <typeparam name="TClass">The type of the class.</typeparam>
/// <typeparam name="TDbContext">The type of the database context.</typeparam>
/// <typeparam name="TStoreOption">The type of the store option.</typeparam>
/// <seealso cref="DatabaseProviderFixture{T}" />
public class IntegrationTest<TClass, TDbContext, TStoreOption> : IClassFixture<DatabaseProviderFixture<TDbContext>>
    where TDbContext : DbContext
{
    public static readonly TheoryData<DbContextOptions<TDbContext>> TestDatabaseProviders;
    protected readonly TStoreOption StoreOptions = Activator.CreateInstance<TStoreOption>();

    static IntegrationTest()
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Console.WriteLine($"Running Local Tests for {typeof(TClass).Name}");

            TestDatabaseProviders = new TheoryData<DbContextOptions<TDbContext>>
            {
                DatabaseProviderBuilder.BuildInMemory<TDbContext>(typeof(TClass).Name),
                DatabaseProviderBuilder.BuildSqlite<TDbContext>(typeof(TClass).Name),
                DatabaseProviderBuilder.BuildLocalDb<TDbContext>(typeof(TClass).Name)
            };
        }
        else
        {
            TestDatabaseProviders = new TheoryData<DbContextOptions<TDbContext>>
            {
                DatabaseProviderBuilder.BuildInMemory<TDbContext>(typeof(TClass).Name),
                DatabaseProviderBuilder.BuildSqlite<TDbContext>(typeof(TClass).Name)
            };
            Console.WriteLine("Skipping DB integration tests on non-Windows");
        }
    }

    protected IntegrationTest(DatabaseProviderFixture<TDbContext> fixture)
    {
        fixture.Options = TestDatabaseProviders.SelectMany(x => x.Select(y => (DbContextOptions<TDbContext>)y)).ToList();
        fixture.StoreOptions = StoreOptions;
    }
}
