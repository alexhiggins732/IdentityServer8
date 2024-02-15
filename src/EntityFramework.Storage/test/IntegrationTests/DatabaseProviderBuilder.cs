/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Microsoft.EntityFrameworkCore;

namespace IdentityServer8.EntityFramework.IntegrationTests;

/// <summary>
/// Helper methods to initialize DbContextOptions for the specified database provider and context.
/// </summary>
public class DatabaseProviderBuilder
{
    public static DbContextOptions<T> BuildInMemory<T>(string name) where T : DbContext
    {
        var builder = new DbContextOptionsBuilder<T>();
        builder.UseInMemoryDatabase(name);
        return builder.Options;
    }

    public static DbContextOptions<T> BuildSqlite<T>(string name) where T : DbContext
    {
        var builder = new DbContextOptionsBuilder<T>();
        builder.UseSqlite($"Filename=./Test.IdentityServer8.EntityFramework-3.1.0.{name}.db");
        return builder.Options;
    }

    public static DbContextOptions<T> BuildLocalDb<T>(string name) where T : DbContext
    {
        var builder = new DbContextOptionsBuilder<T>();
        builder.UseSqlServer(
            $@"Data Source=(LocalDb)\MSSQLLocalDB;database=Test.IdentityServer8.EntityFramework-3.1.0.{name};trusted_connection=yes;");
        return builder.Options;
    }
}
