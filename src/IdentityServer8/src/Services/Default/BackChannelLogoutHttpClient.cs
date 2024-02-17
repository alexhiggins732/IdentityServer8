/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Services;

/// <summary>
/// Models making HTTP requests for back-channel logout notification.
/// </summary>
public class DefaultBackChannelLogoutHttpClient : IBackChannelLogoutHttpClient
{
    private readonly HttpClient _client;
    private readonly ILogger<DefaultBackChannelLogoutHttpClient> _logger;

    /// <summary>
    /// Constructor for BackChannelLogoutHttpClient.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="loggerFactory"></param>
    public DefaultBackChannelLogoutHttpClient(HttpClient client, ILoggerFactory loggerFactory)
    {
        _client = client;
        _logger = loggerFactory.CreateLogger<DefaultBackChannelLogoutHttpClient>();
    }

    /// <summary>
    /// Posts the payload to the url.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    public async Task PostAsync(string url, Dictionary<string, string> payload)
    {
        try
        {
            var response = await _client.PostAsync(url, new FormUrlEncodedContent(payload));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug("Response from back-channel logout endpoint: {url} status code: {status}", url, (int)response.StatusCode);
            }
            else
            {
                _logger.LogWarning("Response from back-channel logout endpoint: {url} status code: {status}", url, (int)response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception invoking back-channel logout for url: {url}", url);
        }
    }
}
