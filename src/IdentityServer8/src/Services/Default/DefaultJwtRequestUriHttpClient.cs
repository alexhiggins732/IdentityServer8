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
/// Default JwtRequest client
/// </summary>
public class DefaultJwtRequestUriHttpClient : IJwtRequestUriHttpClient
{
    private readonly HttpClient _client;
    private readonly IdentityServerOptions _options;
    private readonly ILogger<DefaultJwtRequestUriHttpClient> _logger;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="client">An HTTP client</param>
    /// <param name="options">The options.</param>
    /// <param name="loggerFactory">The logger factory</param>
    public DefaultJwtRequestUriHttpClient(HttpClient client, IdentityServerOptions options, ILoggerFactory loggerFactory)
    {
        _client = client;
        _options = options;
        _logger = loggerFactory.CreateLogger<DefaultJwtRequestUriHttpClient>();
    }


    /// <inheritdoc />
    public async Task<string> GetJwtAsync(string url, Client client)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, url);
        req.Properties.Add(IdentityServerConstants.JwtRequestClientKey, client);

        var response = await _client.SendAsync(req);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            if (_options.StrictJarValidation)
            {
                if (!string.Equals(response.Content.Headers.ContentType.MediaType,
                    $"application/{JwtClaimTypes.JwtTypes.AuthorizationRequest}", StringComparison.Ordinal))
                {
                    _logger.LogError("Invalid content type {type} from jwt url {url}", response.Content.Headers.ContentType.MediaType, url);
                    return null;
                }
            }

            _logger.LogDebug("Success http response from jwt url {url}", url);
            
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
            
        _logger.LogError("Invalid http status code {status} from jwt url {url}", response.StatusCode, url);
        return null;
    }
}
