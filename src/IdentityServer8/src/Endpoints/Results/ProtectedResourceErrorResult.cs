/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Threading.Tasks;
using IdentityServer8.Extensions;
using Microsoft.Extensions.Primitives;
using IdentityServer8.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using IdentityModel;

namespace IdentityServer8.Endpoints.Results
{
    internal class ProtectedResourceErrorResult : IEndpointResult
    {
        public string Error;
        public string ErrorDescription;

        public ProtectedResourceErrorResult(string error, string errorDescription = null)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = 401;
            context.Response.SetNoCache();

            if (Constants.ProtectedResourceErrorStatusCodes.ContainsKey(Error))
            {
                context.Response.StatusCode = Constants.ProtectedResourceErrorStatusCodes[Error];
            }

            if (Error == OidcConstants.ProtectedResourceErrors.ExpiredToken)
            {
                Error = OidcConstants.ProtectedResourceErrors.InvalidToken;
                ErrorDescription = "The access token expired";
            }

            var errorString = string.Format($"error=\"{Error}\"");
            if (ErrorDescription.IsMissing())
            {
                context.Response.Headers.Append(HeaderNames.WWWAuthenticate, new StringValues(new[] { "Bearer realm=\"IdentityServer\"", errorString }).ToString());
            }
            else
            {
                var errorDescriptionString = string.Format($"error_description=\"{ErrorDescription}\"");
                context.Response.Headers.Append(HeaderNames.WWWAuthenticate, new StringValues(new[] { "Bearer realm=\"IdentityServer\"", errorString, errorDescriptionString }).ToString());
            }

            return Task.CompletedTask;
        }
    }
}
