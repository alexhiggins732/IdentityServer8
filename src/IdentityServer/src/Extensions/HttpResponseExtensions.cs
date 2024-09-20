/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

#pragma warning disable 1591


namespace IdentityServer8.Extensions;

public static class HttpResponseExtensions
{
    public static async Task WriteJsonAsync(this HttpResponse response, object o, string contentType = null)
    {
        var json = ObjectSerializer.ToString(o);
        await response.WriteJsonAsync(json, contentType);
        await response.Body.FlushAsync();
    }

    public static async Task WriteJsonAsync(this HttpResponse response, string json, string contentType = null)
    {
        response.ContentType = contentType ?? "application/json; charset=UTF-8";
        await response.WriteAsync(json);
        await response.Body.FlushAsync();
    }

    public static void SetCache(this HttpResponse response, int maxAge, params string[] varyBy)
    {
        if (maxAge == 0)
        {
            SetNoCache(response);
        }
        else if (maxAge > 0)
        {
            if (!response.Headers.ContainsKey("Cache-Control"))
            {
                response.Headers.Append("Cache-Control", $"max-age={maxAge}");
            }

            if (varyBy?.Any() == true)
            {
                var vary = varyBy.Aggregate((x, y) => x + "," + y);
                if (response.Headers.ContainsKey("Vary"))
                {
                    vary = response.Headers["Vary"].ToString() + "," + vary;
                }
                response.Headers["Vary"] = vary;
            }
        }
    }

    public static void SetNoCache(this HttpResponse response)
    {
        if (!response.Headers.ContainsKey("Cache-Control"))
        {
            response.Headers.Append("Cache-Control", "no-store, no-cache, max-age=0");
        }
        else
        {
            response.Headers["Cache-Control"] = "no-store, no-cache, max-age=0";
        }

        if (!response.Headers.ContainsKey("Pragma"))
        {
            response.Headers.Append("Pragma", "no-cache");
        }
    }

    public static async Task WriteHtmlAsync(this HttpResponse response, string html)
    {
        response.ContentType = "text/html; charset=UTF-8";
        await response.WriteAsync(html, Encoding.UTF8);
        await response.Body.FlushAsync();
    }

    public static void RedirectToAbsoluteUrl(this HttpResponse response, string url)
    {
        if (url.IsLocalUrl())
        {
            if (url.StartsWith("~/")) url = url.Substring(1);
            url = response.HttpContext.GetIdentityServerBaseUrl().EnsureTrailingSlash() + url.RemoveLeadingSlash();
        }
        response.RedirectIfAllowed(url);
    }

    public static void AddScriptCspHeaders(this HttpResponse response, CspOptions options, string hash)
    {
        var csp1part = options.Level == CspLevel.One ? "'unsafe-inline' " : string.Empty;
        var cspHeader = $"default-src 'none'; script-src {csp1part}'{hash}'";

        AddCspHeaders(response.Headers, options, cspHeader);
    }

    public static void AddStyleCspHeaders(this HttpResponse response, CspOptions options, string hash, string frameSources)
    {
        var csp1part = options.Level == CspLevel.One ? "'unsafe-inline' " : string.Empty;
        var cspHeader = $"default-src 'none'; style-src {csp1part}'{hash}'";

        if (!string.IsNullOrEmpty(frameSources))
        {
            cspHeader += $"; frame-src {frameSources}";
        }

        AddCspHeaders(response.Headers, options, cspHeader);
    }

    public static void AddCspHeaders(IHeaderDictionary headers, CspOptions options, string cspHeader)
    {
        if (!headers.ContainsKey("Content-Security-Policy"))
        {
            headers.Append("Content-Security-Policy", cspHeader);
        }
        if (options.AddDeprecatedHeader && !headers.ContainsKey("X-Content-Security-Policy"))
        {
            headers.Append("X-Content-Security-Policy", cspHeader);
        }
    }
}
