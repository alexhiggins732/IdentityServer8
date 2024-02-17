/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer8.EntityFramework.IntegrationTests;

/// <summary>
/// xUnit ClassFixture for creating and deleting integration test databases.
/// </summary>
/// <typeparam name="T">DbContext of Type T</typeparam>
public class DatabaseProviderFixture<T> : IDisposable where T : DbContext
{
    public object StoreOptions;
    public List<DbContextOptions<T>> Options;

    public void Dispose()
    {
        if (Options != null) // null check since fixtures are created even when tests are skipped
        {
            foreach (var option in Options.ToList())
            {
                using (var context = (T)Activator.CreateInstance(typeof(T), option, StoreOptions))
                {
                    context.Database.EnsureDeleted();
                }
            }
        }
    }
}
