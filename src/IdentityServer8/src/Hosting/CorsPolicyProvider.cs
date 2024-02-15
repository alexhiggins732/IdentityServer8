/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Hosting;

internal class CorsPolicyProvider : ICorsPolicyProvider
{
    private readonly ILogger _logger;
    private readonly ICorsPolicyProvider _inner;
    private readonly IdentityServerOptions _options;
    private readonly IHttpContextAccessor _httpContext;

    public CorsPolicyProvider(
        ILogger<CorsPolicyProvider> logger,
        Decorator<ICorsPolicyProvider> inner,
        IdentityServerOptions options,
        IHttpContextAccessor httpContext)
    {
        _logger = logger;
        _inner = inner.Instance;
        _options = options;
        _httpContext = httpContext;
    }

    public Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
    {
        if (_options.Cors.CorsPolicyName == policyName)
        {
            return ProcessAsync(context);
        }
        else
        {
            return _inner.GetPolicyAsync(context, policyName);
        }
    }
    private async Task<CorsPolicy> ProcessAsync(HttpContext context)
    {
        var origin = context.Request.GetCorsOrigin();
        if (origin != null)
        {
            var path = context.Request.Path;
            if (IsPathAllowed(path))
            {
                var sanitizedOrigin = origin.SanitizeForLog();
                _logger.LogDebug("CORS request made for path: {path} from origin: {origin}", path.SanitizeForLog(), sanitizedOrigin);

                // manually resolving this from DI because this: 
                // https://github.com/aspnet/CORS/issues/105
                var corsPolicyService = _httpContext.HttpContext.RequestServices.GetRequiredService<ICorsPolicyService>();

                if (await corsPolicyService.IsOriginAllowedAsync(origin))
                {
                    _logger.LogDebug("CorsPolicyService allowed origin: {origin}", sanitizedOrigin);
                    return Allow(origin);
                }
                else
                {
                    _logger.LogWarning("CorsPolicyService did not allow origin: {origin}", sanitizedOrigin);
                }
            }
            else
            {
                _logger.LogDebug("CORS request made for path: {path} from origin: {origin} but was ignored because path was not for an allowed IdentityServer CORS endpoint", Ioc.Sanitizer.Log.Sanitize(path), Ioc.Sanitizer.Log.Sanitize(origin));
            }
        }

        return null;
    }

    private CorsPolicy Allow(string origin)
    {
        var policyBuilder = new CorsPolicyBuilder()
            .WithOrigins(origin)
            .AllowAnyHeader()
            .AllowAnyMethod();

        if (_options.Cors.PreflightCacheDuration.HasValue)
        {
            policyBuilder.SetPreflightMaxAge(_options.Cors.PreflightCacheDuration.Value);
        }

        return policyBuilder.Build();
    }

    private bool IsPathAllowed(PathString path)
    {
        return _options.Cors.CorsPaths.Any(x => path == x);
    }
}
