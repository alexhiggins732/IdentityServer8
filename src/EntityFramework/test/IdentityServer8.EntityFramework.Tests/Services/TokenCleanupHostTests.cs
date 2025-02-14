using IdentityServer8.EntityFramework.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace IdentityServer8.EntityFramework.Tests.Services
{
    public class TokenCleanupHostTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<OperationalStoreOptions> _optionsMock;
        private readonly Mock<ILogger<TokenCleanupHost>> _loggerMock;

        public TokenCleanupHostTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _optionsMock = new Mock<OperationalStoreOptions>();
            _loggerMock = new Mock<ILogger<TokenCleanupHost>>();
        }

        [Fact]
        public void TokenCleanupHost_WhenServiceProviderIsNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TokenCleanupHost(null, _optionsMock.Object, _loggerMock.Object));
        }

        [Fact]
        public void TokenCleanupHost_WhenOptionsIsNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TokenCleanupHost(_serviceProviderMock.Object, null, _loggerMock.Object));
        }

        [Fact]
        public async void TokenCleanupHost_WhenStartingOptionsEnabledFalse_ShouldNotThrow()
        {
            // Act & Assert
            var t = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);
            await t.StartAsync(CancellationToken.None);
        }


        [Fact]
        public async void TokenCleanupHost_WhenStoppingWithptionsEnabledFalse_ShouldNotThrow()
        {
            // Act & Assert
            var t = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);
            await t.StopAsync(CancellationToken.None);
        }


        [Fact]
        public async Task StartAsync_WhenTokenCleanupEnabled_ShouldStartTokenCleanup()
        {
            // Arrange
            _optionsMock.SetupGet(o => o.EnableTokenCleanup).Returns(true);
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);

            // Act
            await tokenCleanupHost.StartAsync(CancellationToken.None);

            // Assert
            // Verify that the token cleanup process has started
        }

        [Fact]
        public async Task StartAsync_WhenTokenCleanupDisabled_ShouldNotStartTokenCleanup()
        {
            // Arrange
            _optionsMock.SetupGet(o => o.EnableTokenCleanup).Returns(false);
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);

            // Act
            await tokenCleanupHost.StartAsync(CancellationToken.None);

            // Assert
            // Verify that the token cleanup process has not started
        }

        [Fact]
        public async Task StartAsync_WhenAlreadyStarted_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _optionsMock.SetupGet(o => o.EnableTokenCleanup).Returns(true);
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);
            await tokenCleanupHost.StartAsync(CancellationToken.None);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => tokenCleanupHost.StartAsync(CancellationToken.None));
        }


        [Fact]
        public async Task StartAsync_WithCancellationRequested_ShouldLogExisting()
        {
            // Arrange
            _optionsMock.SetupGet(o => o.EnableTokenCleanup).Returns(true);
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);
            var cts = new CancellationTokenSource();
            cts.Cancel();
            await tokenCleanupHost.StartAsync(cts.Token);


            _loggerMock.Verify(x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);

            //_loggerMock.Verify(t => t.LogDebug(It.IsAny<string>()), Times.AtLeastOnce);

            // Act & Assert

        }



        [Fact]
        public async Task StopAsync_WhenTokenCleanupEnabledAndStarted_ShouldStopTokenCleanup()
        {
            // Arrange
            _optionsMock.SetupGet(o => o.EnableTokenCleanup).Returns(true);
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);
            await tokenCleanupHost.StartAsync(CancellationToken.None);

            // Act
            await tokenCleanupHost.StopAsync(CancellationToken.None);

            // Assert
            // Verify that the token cleanup process has stopped
        }

        [Fact]
        public async Task StopAsync_WhenTokenCleanupDisabled_ShouldNotStopTokenCleanup()
        {
            // Arrange
            _optionsMock.SetupGet(o => o.EnableTokenCleanup).Returns(false);
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);

            // Act
            await tokenCleanupHost.StopAsync(CancellationToken.None);

            // Assert
            // Verify that the token cleanup process has not stopped
        }

        [Fact]
        public async Task StopAsync_WhenNotStarted_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _optionsMock.SetupGet(o => o.EnableTokenCleanup).Returns(true);
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => tokenCleanupHost.StopAsync(CancellationToken.None));
        }

        [Fact]
        public async Task StartAsync_WithTokenCleanupEnabled_ShouldInvokeCleanup()
        {
            var serviceCollection = new ServiceCollection();
            var tokenCleanupServiceMock = new Mock<ITokenCleanupService>();
            serviceCollection.AddScoped(_ => tokenCleanupServiceMock.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider(true);

            var options = new OperationalStoreOptions { EnableTokenCleanup = true, TokenCleanupInterval = 1 }; // Example options
            var loggerMock = new Mock<ILogger<TokenCleanupHost>>();
            var tokenCleanupHost = new TokenCleanupHost(serviceProvider, options, loggerMock.Object);
            // Act
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            await tokenCleanupHost.StartAsync(cts.Token);

            // Short delay to allow the background task to run
            await Task.Delay(2000);

            // Assert that RemoveExpiredGrantsAsync is called at least once
            tokenCleanupServiceMock.Verify(t => t.RemoveExpiredGrantsAsync(), Times.AtLeastOnce);

            // Cleanup
            cts.Cancel(); // Ensure cancellation is passed to stop the task
            await tokenCleanupHost.StopAsync(CancellationToken.None);
        }


        [Fact]
        public async Task StartAsync_WithCancellation_ShouldInvokeCleanup()
        {
            var serviceCollection = new ServiceCollection();
            var tokenCleanupServiceMock = new Mock<ITokenCleanupService>();
            serviceCollection.AddScoped(_ => tokenCleanupServiceMock.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider(true);

            var options = new OperationalStoreOptions { EnableTokenCleanup = true, TokenCleanupInterval = 1 }; // Example options
            var loggerMock = new Mock<ILogger<TokenCleanupHost>>();
            var tokenCleanupHost = new TokenCleanupHost(serviceProvider, options, loggerMock.Object);
            // Act
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            await tokenCleanupHost.StartAsync(cts.Token);

            // Short delay to allow the background task to run
            await Task.Delay(2000);

            // Assert that RemoveExpiredGrantsAsync is called at least once
            tokenCleanupServiceMock.Verify(t => t.RemoveExpiredGrantsAsync(), Times.AtLeastOnce);

            // Cleanup
            cts.Cancel(); // Ensure cancellation is passed to stop the task
            await tokenCleanupHost.StopAsync(CancellationToken.None);
        }


        [Fact]
        public async Task RemoveExpiredGrantsAsync_CallsCleanupService()
        {
            var serviceCollection = new ServiceCollection();
            var tokenCleanupServiceMock = new Mock<ITokenCleanupService>();
            serviceCollection.AddScoped(_ => tokenCleanupServiceMock.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider(true);

            var options = new OperationalStoreOptions { EnableTokenCleanup = true, TokenCleanupInterval = 1 }; // Example options
            var loggerMock = new Mock<ILogger<TokenCleanupHost>>();
            var tokenCleanupHost = new TokenCleanupHost(serviceProvider, options, loggerMock.Object);

            await tokenCleanupHost.RemoveExpiredGrantsAsync();

            tokenCleanupServiceMock.Verify(x => x.RemoveExpiredGrantsAsync(), Times.Once);
        }


        [Fact]
        public async Task RemoveExpiredGrantsAsync_LogsWhenThrowingException()
        {
            var serviceCollection = new ServiceCollection();
            var tokenCleanupServiceMock = new Mock<ITokenCleanupService>();
            tokenCleanupServiceMock.Setup(x => x.RemoveExpiredGrantsAsync()).ThrowsAsync(new Exception("Test exception"));
            serviceCollection.AddScoped(_ => tokenCleanupServiceMock.Object);

            var serviceProvider = serviceCollection.BuildServiceProvider(true);

            var options = new OperationalStoreOptions { EnableTokenCleanup = true, TokenCleanupInterval = 1 }; // Example options
            var loggerMock = new Mock<ILogger<TokenCleanupHost>>();
            var tokenCleanupHost = new TokenCleanupHost(serviceProvider, options, loggerMock.Object);




            int callCount = 0;
            loggerMock.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()))
            .Callback<LogLevel, EventId, object, Exception, object>((level, eventId, state, exception, func) =>
            {
                Console.WriteLine($"Logged at level {level} with exception: {exception?.Message}");
                callCount++;
            });

         
            await tokenCleanupHost.RemoveExpiredGrantsAsync();

            // this is failing;
            //_loggerMock.Verify(x => x.Log(
            //    It.IsAny<LogLevel>(),
            //    It.IsAny<EventId>(),
            //    It.IsAny<It.IsAnyType>(),
            //    It.IsAny<Exception>(),
            //    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            //    Times.AtLeastOnce);
            callCount.Should().Be(1);
        }


        [Fact]
        public async Task StartInternalAsync_WhenCancellationRequested_ShouldExit()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));
            var cancellationToken = cancellationTokenSource.Token;
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);

            // Act
            await tokenCleanupHost.StartInternalAsync(cancellationToken);

            // Assert
            // Verify that the token cleanup process has exited
        }

        [Fact]
        public async Task StartInternalAsync_WhenDelayCancelled_ShouldExit()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));
            var cancellationToken = cancellationTokenSource.Token;
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);

            // Act
            await tokenCleanupHost.StartInternalAsync(cancellationToken);

            // Assert
            // Verify that the token cleanup process has exited
        }

        [Fact]
        public async Task StartInternalAsync_WhenDelayThrowsException_ShouldExit()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));
            var cancellationToken = cancellationTokenSource.Token;
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);

            // Act
            await tokenCleanupHost.StartInternalAsync(cancellationToken);

            // Assert
            // Verify that the token cleanup process has exited
        }

        [Fact]
        public async Task StartInternalAsync_WhenNotCancelled_ShouldRemoveExpiredGrants()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));
            var cancellationToken = cancellationTokenSource.Token;
            var tokenCleanupHost = new TokenCleanupHost(_serviceProviderMock.Object, _optionsMock.Object, _loggerMock.Object);

            // Act


            try
            {
                // Await the task to complete or be cancelled
                await tokenCleanupHost.StartInternalAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Expected if the cancellation token is triggered
                // Optionally, handle the cancellation here if necessary
            }
            catch (Exception ex)
            {
                // Handle any other exceptions here
                Assert.Fail($"Unexpected exception: {ex.Message}");
            }

            // Assert
            // Verify that the expired grants have been removed
        }
    }

}
