using IdentityServer8.Configuration;
using IdentityServer8.EntityFramework.DbContexts;
using IdentityServer8.EntityFramework.Entities;
using IdentityServer8.EntityFramework.Interfaces;
using IdentityServer8.EntityFramework.Services;
using IdentityServer8.EntityFramework.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
namespace IdentityServer8.EntityFramework.Tests.ExtensionTests
{

    public class IdentityServerEntityFrameworkBuilderExtensionsTests
    {
        [Fact]
        public void AddConfigurationStore_Should_AddConfigurationDbContext()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddConfigurationStore();

            // Assert
            var services = builder.Services;
            var dbContextService = services.FirstOrDefault(s => s.ServiceType == typeof(ConfigurationDbContext));
            Assert.NotNull(dbContextService);
        }

        [Fact]
        public void AddConfigurationStore_Generic_Should_AddConfigurationDbContext()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddConfigurationStore<ConfigurationDbContext>();

            // Assert
            var services = builder.Services;
            var dbContextService = services.FirstOrDefault(s => s.ServiceType == typeof(ConfigurationDbContext));
            Assert.NotNull(dbContextService);
        }

        [Fact]
        public void AddConfigurationStore_Should_AddClientStore()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddConfigurationStore();

            // Assert
            var services = builder.Services;
            var clientStoreService = services.FirstOrDefault(s => s.ServiceType == typeof(ClientStore));
            Assert.NotNull(clientStoreService);
        }

        [Fact]
        public void AddConfigurationStore_Should_AddResourceStore()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddConfigurationStore();

            // Assert
            var services = builder.Services;
            var resourceStoreService = services.FirstOrDefault(s => s.ImplementationType == typeof(ResourceStore));
            Assert.NotNull(resourceStoreService);
        }

        [Fact]
        public void AddConfigurationStore_Should_AddCorsPolicyService()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddConfigurationStore();

            // Assert
            var services = builder.Services;
            var corsPolicyService = services.FirstOrDefault(s => s.ImplementationType == typeof(CorsPolicyService));
            Assert.NotNull(corsPolicyService);
        }

        [Fact]
        public void AddConfigurationStoreCache_Should_AddInMemoryCaching()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddConfigurationStoreCache();

            // Assert
            var services = builder.Services;
            var cachingService = services.FirstOrDefault(s => s.ImplementationType == typeof(MemoryCache));
            Assert.NotNull(cachingService);
        }

        [Fact]
        public void AddConfigurationStoreCache_Should_AddClientStoreCache()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddConfigurationStoreCache();

            // Assert
            var services = builder.Services;
            var clientStoreCacheService = services.FirstOrDefault(s => s.ServiceType == typeof(ClientStore));
            Assert.NotNull(clientStoreCacheService);
        }

        [Fact]
        public void AddConfigurationStoreCache_Should_AddResourceStoreCache()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddConfigurationStoreCache();

            // Assert
            var services = builder.Services;
            var resourceStoreCacheService = services.FirstOrDefault(s => s.ServiceType == typeof(ResourceStore));
            Assert.NotNull(resourceStoreCacheService);
        }

        [Fact]
        public void AddConfigurationStoreCache_Should_AddCorsPolicyCache()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddConfigurationStoreCache();

            // Assert
            var services = builder.Services;
            var corsPolicyCacheService = services.FirstOrDefault(s => s.ServiceType == typeof(CorsPolicyService));
            Assert.NotNull(corsPolicyCacheService);
        }

        [Fact]
        public void AddOperationalStore_Should_AddOperationalDbContext()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddOperationalStore();

            // Assert
            var services = builder.Services;
            var dbContextService = services.FirstOrDefault(s => s.ServiceType == typeof(PersistedGrantDbContext));
            Assert.NotNull(dbContextService);
        }

        [Fact]
        public void AddOperationalStore_Generic_Should_AddOperationalDbContext()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddOperationalStore<PersistedGrantDbContext>();

            // Assert
            var services = builder.Services;
            var dbContextService = services.FirstOrDefault(s => s.ServiceType == typeof(PersistedGrantDbContext));
            Assert.NotNull(dbContextService);
        }

        [Fact]
        public void AddOperationalStore_Should_AddPersistedGrantStore()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddOperationalStore();

            // Assert
            var services = builder.Services;
            var persistedGrantStoreService = services.FirstOrDefault(s => s.ImplementationType == typeof(PersistedGrantStore));
            Assert.NotNull(persistedGrantStoreService);
        }

        [Fact]
        public void AddOperationalStore_Should_AddDeviceFlowStore()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddOperationalStore();

            // Assert
            var services = builder.Services;
            var deviceFlowStoreService = services.FirstOrDefault(s => s.ImplementationType == typeof(DeviceFlowStore));
            Assert.NotNull(deviceFlowStoreService);
        }

        [Fact]
        public void AddOperationalStore_Should_AddTokenCleanupHost()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddOperationalStore();

            // Assert
            var services = builder.Services;
            var tokenCleanupHostService = services.FirstOrDefault(s => s.ImplementationType == typeof(TokenCleanupHost));
            Assert.NotNull(tokenCleanupHostService);
        }

        [Fact]
        public void AddOperationalStoreNotification_Should_AddOperationalStoreNotification()
        {
            // Arrange
            var builder = new IdentityServerBuilder(new ServiceCollection());

            // Act
            builder.AddOperationalStoreNotification<CustomOperationalStoreNotification>();

            // Assert
            var services = builder.Services;
            var operationalStoreNotificationService = services.FirstOrDefault(s => s.ImplementationType == typeof(CustomOperationalStoreNotification));
            Assert.NotNull(operationalStoreNotificationService);
        }
    }


    // Custom operational store notification class for testing
    public class CustomOperationalStoreNotification : IOperationalStoreNotification
    {
        // Implement required members
        public Task DeviceCodesRemovedAsync(IEnumerable<DeviceFlowCodes> deviceCodes)
        {
            throw new NotImplementedException();
        }

        public Task PersistedGrantsRemovedAsync(IEnumerable<PersistedGrant> persistedGrants)
        {
            throw new NotImplementedException();
        }
    }
}


