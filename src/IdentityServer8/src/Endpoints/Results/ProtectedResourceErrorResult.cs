/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Microsoft.Net.Http.Headers;

namespace IdentityServer8.Endpoints.Results;

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
