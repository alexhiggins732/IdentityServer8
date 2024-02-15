/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Hosting;

internal class EndpointRouter : IEndpointRouter
{
    private readonly IEnumerable<Endpoint> _endpoints;
    private readonly IdentityServerOptions _options;
    private readonly ILogger _logger;

    public EndpointRouter(IEnumerable<Endpoint> endpoints, IdentityServerOptions options, ILogger<EndpointRouter> logger)
    {
        _endpoints = endpoints;
        _options = options;
        _logger = logger;
    }

    public IEndpointHandler Find(HttpContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        foreach(var endpoint in _endpoints)
        {
            var path = endpoint.Path;
            if (context.Request.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
            {
                var endpointName = endpoint.Name;
                _logger.LogDebug("Request path {path} matched to endpoint type {endpoint}", Ioc.Sanitizer.Log.Sanitize(context.Request.Path), endpointName);

                return GetEndpointHandler(endpoint, context);
            }
        }

        _logger.LogTrace("No endpoint entry found for request path: {path}", Ioc.Sanitizer.Log.Sanitize(context.Request.Path));

        return null;
    }

    private IEndpointHandler GetEndpointHandler(Endpoint endpoint, HttpContext context)
    {
        if (_options.Endpoints.IsEndpointEnabled(endpoint))
        {
            if (context.RequestServices.GetService(endpoint.Handler) is IEndpointHandler handler)
            {
                _logger.LogDebug("Endpoint enabled: {endpoint}, successfully created handler: {endpointHandler}", endpoint.Name, endpoint.Handler.FullName);
                return handler;
            }

            _logger.LogDebug("Endpoint enabled: {endpoint}, failed to create handler: {endpointHandler}", endpoint.Name, endpoint.Handler.FullName);
        }
        else
        {
            _logger.LogWarning("Endpoint disabled: {endpoint}", endpoint.Name);
        }

        return null;
    }
}
