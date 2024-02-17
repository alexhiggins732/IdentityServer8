/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using FluentAssertions;
using IdentityServer8.EntityFramework.DbContexts;
using IdentityServer8.EntityFramework.Entities;
using IdentityServer8.EntityFramework.Interfaces;
using IdentityServer8.EntityFramework.Options;
using IdentityServer8.EntityFramework.Stores;
using IdentityServer8.Stores;
using IdentityServer8.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IdentityServer8.EntityFramework.IntegrationTests.TokenCleanup;

public class TokenCleanupTests : IntegrationTest<TokenCleanupTests, PersistedGrantDbContext, OperationalStoreOptions>
{


    public TokenCleanupTests(DatabaseProviderFixture<PersistedGrantDbContext> fixture) : base(fixture)
    {
        foreach (var options in TestDatabaseProviders.SelectMany(x => x.Select(y => (DbContextOptions<PersistedGrantDbContext>)y)).ToList())
        {
            using (var context = new PersistedGrantDbContext(options, StoreOptions))
            {
                context.Database.EnsureCreated();
            }
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task RemoveExpiredGrantsAsync_WhenExpiredGrantsExist_ExpectExpiredGrantsRemoved(DbContextOptions<PersistedGrantDbContext> options)
    {
        var expiredGrant = new PersistedGrant
        {
            Key = Guid.NewGuid().ToString(),
            ClientId = "app1",
            Type = "reference",
            SubjectId = "123",
            Expiration = DateTime.UtcNow.AddDays(-3),
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options, StoreOptions))
        {
            context.PersistedGrants.Add(expiredGrant);
            context.SaveChanges();
        }

        await CreateSut(options).RemoveExpiredGrantsAsync();

        using (var context = new PersistedGrantDbContext(options, StoreOptions))
        {
            context.PersistedGrants.FirstOrDefault(x => x.Key == expiredGrant.Key).Should().BeNull();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task RemoveExpiredGrantsAsync_WhenValidGrantsExist_ExpectValidGrantsInDb(DbContextOptions<PersistedGrantDbContext> options)
    {
        var validGrant = new PersistedGrant
        {
            Key = Guid.NewGuid().ToString(),
            ClientId = "app1",
            Type = "reference",
            SubjectId = "123",
            Expiration = DateTime.UtcNow.AddDays(3),
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options, StoreOptions))
        {
            context.PersistedGrants.Add(validGrant);
            context.SaveChanges();
        }

        await CreateSut(options).RemoveExpiredGrantsAsync();

        using (var context = new PersistedGrantDbContext(options, StoreOptions))
        {
            context.PersistedGrants.FirstOrDefault(x => x.Key == validGrant.Key).Should().NotBeNull();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task RemoveExpiredGrantsAsync_WhenExpiredDeviceGrantsExist_ExpectExpiredDeviceGrantsRemoved(DbContextOptions<PersistedGrantDbContext> options)
    {
        var expiredGrant = new DeviceFlowCodes
        {
            DeviceCode = Guid.NewGuid().ToString(),
            UserCode = Guid.NewGuid().ToString(),
            ClientId = "app1",
            SubjectId = "123",
            CreationTime = DateTime.UtcNow.AddDays(-4),
            Expiration = DateTime.UtcNow.AddDays(-3),
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options, StoreOptions))
        {
            context.DeviceFlowCodes.Add(expiredGrant);
            context.SaveChanges();
        }

        await CreateSut(options).RemoveExpiredGrantsAsync();

        using (var context = new PersistedGrantDbContext(options, StoreOptions))
        {
            context.DeviceFlowCodes.FirstOrDefault(x => x.DeviceCode == expiredGrant.DeviceCode).Should().BeNull();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task RemoveExpiredGrantsAsync_WhenValidDeviceGrantsExist_ExpectValidDeviceGrantsInDb(DbContextOptions<PersistedGrantDbContext> options)
    {
        var validGrant = new DeviceFlowCodes
        {
            DeviceCode = Guid.NewGuid().ToString(),
            UserCode = "2468",
            ClientId = "app1",
            SubjectId = "123",
            CreationTime = DateTime.UtcNow.AddDays(-4),
            Expiration = DateTime.UtcNow.AddDays(3),
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options, StoreOptions))
        {
            context.DeviceFlowCodes.Add(validGrant);
            context.SaveChanges();
        }

        await CreateSut(options).RemoveExpiredGrantsAsync();

        using (var context = new PersistedGrantDbContext(options, StoreOptions))
        {
            context.DeviceFlowCodes.FirstOrDefault(x => x.DeviceCode == validGrant.DeviceCode).Should().NotBeNull();
        }
    }

    private EntityFramework.TokenCleanupService CreateSut(DbContextOptions<PersistedGrantDbContext> options)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddIdentityServer()
            .AddTestUsers(new List<TestUser>())
            .AddInMemoryClients(new List<Models.Client>())
            .AddInMemoryIdentityResources(new List<Models.IdentityResource>())
            .AddInMemoryApiResources(new List<Models.ApiResource>());

        services.AddScoped<IPersistedGrantDbContext, PersistedGrantDbContext>(_ =>
            new PersistedGrantDbContext(options, StoreOptions));
        services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
        services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();
        
        services.AddTransient<EntityFramework.TokenCleanupService>();
        services.AddSingleton(StoreOptions);

        return services.BuildServiceProvider().GetRequiredService<EntityFramework.TokenCleanupService>();
        //return new EntityFramework.TokenCleanupService(
        //    services.BuildServiceProvider(),
        //    new NullLogger<EntityFramework.TokenCleanup>(),
        //    StoreOptions);
    }
}
