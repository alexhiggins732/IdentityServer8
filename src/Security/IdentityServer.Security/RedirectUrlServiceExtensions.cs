/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace Microsoft.Extensions.DependencyInjection;

public static class RedirectUrlServiceExtensions
{
    public static bool IsAllowedRedirect(this string redirectUrl)
    {
        return Ioc.RedirectService.IsRedirectAllowed(redirectUrl);
    }
    public static bool IsAllowedRedirect(this Uri uri)
    {
        return Ioc.RedirectService.IsRedirectAllowed(uri.ToString());
    }

    public static void RedirectIfAllowed(this HttpResponse response, string url)
    {
        if (IsAllowedRedirect(url))
            response.Redirect(url.SanitizeForRedirect());
        else
            SetRedirectNotAllowed(response);
    }

    public static void SetRedirectNotAllowed(this HttpResponse response)
    {
        response.StatusCode = (int) HttpStatusCode.Forbidden;
    }

    public static void SetRedirectNotAllowed(this HttpContext ctx)
    {
        ctx.Response.SetRedirectNotAllowed();
    }

}
