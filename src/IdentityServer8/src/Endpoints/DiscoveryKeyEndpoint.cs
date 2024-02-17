/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Endpoints;

internal class DiscoveryKeyEndpoint : IEndpointHandler
{
    private readonly ILogger _logger;

    private readonly IdentityServerOptions _options;

    private readonly IDiscoveryResponseGenerator _responseGenerator;

    public DiscoveryKeyEndpoint(
        IdentityServerOptions options,
        IDiscoveryResponseGenerator responseGenerator,
        ILogger<DiscoveryKeyEndpoint> logger)
    {
        _logger = logger;
        _options = options;
        _responseGenerator = responseGenerator;
    }

    public async Task<IEndpointResult> ProcessAsync(HttpContext context)
    {
        _logger.LogTrace("Processing discovery request.");

        // validate HTTP
        if (!HttpMethods.IsGet(context.Request.Method))
        {
            _logger.LogWarning("Discovery endpoint only supports GET requests");
            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }

        _logger.LogDebug("Start key discovery request");

        if (_options.Discovery.ShowKeySet == false)
        {
            _logger.LogInformation("Key discovery disabled. 404.");
            return new StatusCodeResult(HttpStatusCode.NotFound);
        }

        // generate response
        _logger.LogTrace("Calling into discovery response generator: {type}", _responseGenerator.GetType().FullName);
        var response = await _responseGenerator.CreateJwkDocumentAsync();

        return new JsonWebKeysResult(response, _options.Discovery.ResponseCacheInterval);
    }
}
