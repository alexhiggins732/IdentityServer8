/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

#pragma warning disable 1591


using System.Net.Http.Headers;

namespace IdentityServer8.Extensions;

public static class HttpRequestExtensions
{
    public static string GetCorsOrigin(this HttpRequest request)
    {
        var origin = request.Headers["Origin"].FirstOrDefault();
        var thisOrigin = request.Scheme + "://" + request.Host;

        // see if the Origin is different than this server's origin. if so
        // that indicates a proper CORS request. some browsers send Origin
        // on POST requests.
        if (origin != null && origin != thisOrigin)
        {
            return origin;
        }

        return null;
    }
    
    internal static bool HasApplicationFormContentType(this HttpRequest request)
    {
        if (request.ContentType is null) return false;
        
        if (MediaTypeHeaderValue.TryParse(request.ContentType, out var header))
        {
            // Content-Type: application/x-www-form-urlencoded; charset=utf-8
            return header.MediaType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}
