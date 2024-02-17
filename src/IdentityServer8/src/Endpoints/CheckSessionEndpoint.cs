/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Endpoints;

internal class CheckSessionEndpoint : IEndpointHandler
{
    private readonly ILogger _logger;

    public CheckSessionEndpoint(ILogger<CheckSessionEndpoint> logger)
    {
        _logger = logger;
    }

    public Task<IEndpointResult> ProcessAsync(HttpContext context)
    {
        IEndpointResult result;

        if (!HttpMethods.IsGet(context.Request.Method))
        {
            _logger.LogWarning("Invalid HTTP method for check session endpoint");
            result = new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }
        else
        {
            _logger.LogDebug("Rendering check session result");
            result = new CheckSessionResult();
        }

        return Task.FromResult(result);
    }
}
