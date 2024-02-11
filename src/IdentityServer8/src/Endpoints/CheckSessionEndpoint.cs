/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityServer8.Endpoints.Results;
using IdentityServer8.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace IdentityServer8.Endpoints
{
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
}