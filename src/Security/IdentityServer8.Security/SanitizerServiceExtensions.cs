/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace Microsoft.Extensions.DependencyInjection;
public static class SanitizerServiceExtensions
{
    public static IServiceCollection AddSanitizers(this IServiceCollection services)
    {
        var factory = new SanitizerFactory();
        services.AddSingleton(factory);
        services.AddSingleton<ISanitizerFactory>(factory);
        services.AddSingleton<ISanitizerService, SanitizerService>();
        services.AddSingleton<ISanitizer, Sanitizer>();

        var props = typeof(ISanitizer).GetProperties();
        foreach (var prop in props)
        {
            var type = Enum.Parse<SanitizerType>(prop.PropertyType.Name.Substring(1));
            var sanitizer = factory.Create(type);
            services.AddSingleton(prop.PropertyType, sanitizer);
            services.AddSingleton(sanitizer);
        }

        return services;
    }

    public static string? SanitizeForLog(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        return Ioc.Sanitizer.Log.Sanitize(input?.ToString(), mode);
    }

    public static string? SanitizeForHtml(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        return Ioc.Sanitizer.Html.Sanitize(input?.ToString(), mode);
    }

    public static string? SanitizeForXml(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        return Ioc.Sanitizer.Xml.Sanitize(input?.ToString(), mode);
    }

    public static string? SanitizeForJson(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        return Ioc.Sanitizer.Json.Sanitize(input?.ToString(), mode);
    }

    public static string SanitizeForRedirect(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        var rawUrl = input?.ToString() ?? "";
        if (string.IsNullOrEmpty(rawUrl))
            return rawUrl;
        else
        {
            if (Uri.TryCreate(rawUrl, UriKind.RelativeOrAbsolute, out var uri))
            {
                var parsedUrl = uri.ToString();
                if (parsedUrl == rawUrl.ToString())
                    return parsedUrl;
                else
                {
                    return uri.ToString().SanitizeForLog() ?? "";
                }

            }
            else
            {
                throw new ArgumentException("Invalid URL", nameof(input));
            }
        }

    }

    public static string? SanitizeForUrl(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        return Ioc.Sanitizer.Url.Sanitize(input?.ToString(), mode);
    }

    public static string? SanitizeForCss(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        return Ioc.Sanitizer.Css.Sanitize(input?.ToString(), mode);
    }

    public static string? SanitizeForScript(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        return Ioc.Sanitizer.Script.Sanitize(input?.ToString(), mode);
    }

    public static string? SanitizeForStyle(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        return Ioc.Sanitizer.Style.Sanitize(input?.ToString(), mode);
    }

    public static string? SanitizeForSql(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        return Ioc.Sanitizer.Sql.Sanitize(input?.ToString(), mode);
    }

    public static string? SantizeForRedirect(this object? input, SanitizerMode mode = SanitizerMode.Clean)
    {
        var decoded = Uri.UnescapeDataString(input?.ToString() ?? "");
        decoded.SanitizeForHtml();
        var escaped = Uri.EscapeDataString(decoded);
        return escaped;
    }

}
