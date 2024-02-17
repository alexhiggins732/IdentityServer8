/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

#pragma warning disable 1591

namespace IdentityServer8.Hosting;

public class BaseUrlMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IdentityServerOptions _options;

    public BaseUrlMiddleware(RequestDelegate next, IdentityServerOptions options)
    {
        _next = next;
        _options = options;
    }

    public async Task Invoke(HttpContext context)
    {
        var request = context.Request;
        
        context.SetIdentityServerBasePath(request.PathBase.Value.RemoveTrailingSlash());

        await _next(context);
    }
}
