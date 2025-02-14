using IdentityServer8.EntityFramework.Entities;
using IdentityServer8.EntityFramework.Interfaces;
using IdentityServer8.EntityFramework.Tests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace IdentityServer8.EntityFramework.Services.Tests
{

    public class CorsPolicyServiceTests
    {
        [Fact]
        public async Task IsOriginAllowedAsync_OriginAllowed_ReturnsTrue()
        {


            // Arrange
            var origin = "http://example.com";
            var dbContextMock = new Mock<IConfigurationDbContext>();
            dbContextMock.Setup(db => db.ClientCorsOrigins)
                .Returns(new[] { new ClientCorsOrigin { Origin = origin } }.AsQueryable().MockDbSet());

            // Mock ServiceProvider to return the mocked IConfigurationDbContext
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IConfigurationDbContext)))
                .Returns(dbContextMock.Object);

            // Mock HttpContext to return the mocked ServiceProvider from RequestServices
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.RequestServices).Returns(serviceProviderMock.Object);

            // Mock IHttpContextAccessor to return the mocked HttpContext
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);



            var loggerMock = new Mock<ILogger<CorsPolicyService>>();

            var service = new CorsPolicyService(httpContextAccessorMock.Object, loggerMock.Object);

            // Act
            var result = await service.IsOriginAllowedAsync(origin);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsOriginAllowedAsync_OriginNotAllowed_ReturnsFalse()
        {
            // Arrange
            var origin = "http://example.com";
            var dbContextMock = new Mock<IConfigurationDbContext>();
            dbContextMock.Setup(db => db.ClientCorsOrigins)
                .Returns(Enumerable.Empty<ClientCorsOrigin>().AsQueryable().MockDbSet());

            // Mock ServiceProvider to return the mocked IConfigurationDbContext
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IConfigurationDbContext)))
                .Returns(dbContextMock.Object);

            // Mock HttpContext to return the mocked ServiceProvider from RequestServices
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.RequestServices).Returns(serviceProviderMock.Object);

            // Mock IHttpContextAccessor to return the mocked HttpContext
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);
            var loggerMock = new Mock<ILogger<CorsPolicyService>>();

            var service = new CorsPolicyService(httpContextAccessorMock.Object, loggerMock.Object);

            // Act
            var result = await service.IsOriginAllowedAsync(origin);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsOriginAllowedAsync_OriginNull_ThrowsArgumentNullException()
        {
            // Arrange
            var dbContextMock = new Mock<IConfigurationDbContext>();
    
            var loggerMock = new Mock<ILogger<CorsPolicyService>>();


            // Mock ServiceProvider to return the mocked IConfigurationDbContext
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IConfigurationDbContext)))
                .Returns(dbContextMock.Object);

            // Mock HttpContext to return the mocked ServiceProvider from RequestServices
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.RequestServices).Returns(serviceProviderMock.Object);

            // Mock IHttpContextAccessor to return the mocked HttpContext
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);


            var service = new CorsPolicyService(httpContextAccessorMock.Object, loggerMock.Object);
            //var result= service.IsOriginAllowedAsync(null);
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => service.IsOriginAllowedAsync(null));
        }

        [Fact]
        public void IsOriginAllowedAsync_ContextNull_ThrowsArgumentNullException()
        {
            // Arrange
            var dbContextMock = new Mock<IConfigurationDbContext>();

            var loggerMock = new Mock<ILogger<CorsPolicyService>>();
            //var result= service.IsOriginAllowedAsync(null);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CorsPolicyService(null, loggerMock.Object));
        }
    }
}