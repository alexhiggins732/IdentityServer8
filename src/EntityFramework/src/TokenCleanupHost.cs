/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper to cleanup expired persisted grants.
/// </summary>
public class TokenCleanupHost : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly OperationalStoreOptions _options;
    private readonly ILogger<TokenCleanupHost> _logger;

    private TimeSpan CleanupInterval => TimeSpan.FromSeconds(_options.TokenCleanupInterval);
    
    private CancellationTokenSource _source;

    /// <summary>
    /// Constructor for TokenCleanupHost.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    public TokenCleanupHost(IServiceProvider serviceProvider, OperationalStoreOptions options, ILogger<TokenCleanupHost> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    /// <summary>
    /// Starts the token cleanup polling.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_options.EnableTokenCleanup)
        {
            if (_source != null) throw new InvalidOperationException("Already started. Call Stop first.");

            _logger.LogDebug("Starting grant removal");

            _source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Factory.StartNew(() => StartInternalAsync(_source.Token));
        }
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the token cleanup polling.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_options.EnableTokenCleanup)
        {
            if (_source == null) throw new InvalidOperationException("Not started. Call Start first.");

            _logger.LogDebug("Stopping grant removal");

            _source.Cancel();
            _source = null;
        }

        return Task.CompletedTask;
    }

    private async Task StartInternalAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("CancellationRequested. Exiting.");
                break;
            }

            try
            {
                await Task.Delay(CleanupInterval, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogDebug("TaskCanceledException. Exiting.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError("Task.Delay exception: {0}. Exiting.", ex.Message);
                break;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("CancellationRequested. Exiting.");
                break;
            }

            await RemoveExpiredGrantsAsync();
        }
    }

    async Task RemoveExpiredGrantsAsync()
    {
        try
        {
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var tokenCleanupService = serviceScope.ServiceProvider.GetRequiredService<TokenCleanupService>();
                await tokenCleanupService.RemoveExpiredGrantsAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception removing expired grants: {exception}", ex.Message);
        }
    }
}
