/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Endpoints;

internal class EndSessionCallbackEndpoint : IEndpointHandler
{
    private readonly IEndSessionRequestValidator _endSessionRequestValidator;
    private readonly ILogger _logger;

    public EndSessionCallbackEndpoint(
        IEndSessionRequestValidator endSessionRequestValidator,
        ILogger<EndSessionCallbackEndpoint> logger)
    {
        _endSessionRequestValidator = endSessionRequestValidator;
        _logger = logger;
    }

    public async Task<IEndpointResult> ProcessAsync(HttpContext context)
    {
        if (!HttpMethods.IsGet(context.Request.Method))
        {
            _logger.LogWarning("Invalid HTTP method for end session callback endpoint.");
            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }

        _logger.LogDebug("Processing signout callback request");

        var parameters = context.Request.Query.AsNameValueCollection();
        var result = await _endSessionRequestValidator.ValidateCallbackAsync(parameters);

        if (!result.IsError)
        {
            _logger.LogInformation("Successful signout callback.");
        }
        else
        {
            _logger.LogError("Error validating signout callback: {error}", result.Error);
        }
        
        return new EndSessionCallbackResult(result);
    }
}
