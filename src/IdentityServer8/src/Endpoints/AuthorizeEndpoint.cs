/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using IdentityServer8.Configuration;
using IdentityServer8.Endpoints.Results;
using IdentityServer8.Extensions;
using IdentityServer8.Hosting;
using IdentityServer8.ResponseHandling;
using IdentityServer8.Services;
using IdentityServer8.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer8.Endpoints
{
    internal class AuthorizeEndpoint : AuthorizeEndpointBase
    {
        public AuthorizeEndpoint(
           IEventService events,
           ILogger<AuthorizeEndpoint> logger,
           IdentityServerOptions options,
           IAuthorizeRequestValidator validator,
           IAuthorizeInteractionResponseGenerator interactionGenerator,
           IAuthorizeResponseGenerator authorizeResponseGenerator,
           IUserSession userSession)
            : base(events, logger, options, validator, interactionGenerator, authorizeResponseGenerator, userSession)
        {
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            Logger.LogDebug("Start authorize request");

            NameValueCollection values;

            if (HttpMethods.IsGet(context.Request.Method))
            {
                values = context.Request.Query.AsNameValueCollection();
            }
            else if (HttpMethods.IsPost(context.Request.Method))
            {
                if (!context.Request.HasApplicationFormContentType())
                {
                    return new StatusCodeResult(HttpStatusCode.UnsupportedMediaType);
                }

                values = context.Request.Form.AsNameValueCollection();
            }
            else
            {
                return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            var user = await UserSession.GetUserAsync();
            var result = await ProcessAuthorizeRequestAsync(values, user, null);

            Logger.LogTrace("End authorize request. result type: {0}", result?.GetType().ToString() ?? "-none-");

            return result;
        }
    }
}
