/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors.Infrastructure;
using IdentityServer8.Configuration;
using IdentityServer8.Configuration.DependencyInjection;
using IdentityServer8.Services;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer8.Extensions;

namespace IdentityServer8.Hosting
{
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
                    _logger.LogDebug("CORS request made for path: {path} from origin: {origin}", path, origin);

                    // manually resolving this from DI because this: 
                    // https://github.com/aspnet/CORS/issues/105
                    var corsPolicyService = _httpContext.HttpContext.RequestServices.GetRequiredService<ICorsPolicyService>();

                    if (await corsPolicyService.IsOriginAllowedAsync(origin))
                    {
                        _logger.LogDebug("CorsPolicyService allowed origin: {origin}", origin);
                        return Allow(origin);
                    }
                    else
                    {
                        _logger.LogWarning("CorsPolicyService did not allow origin: {origin}", origin);
                    }
                }
                else
                {
                    _logger.LogDebug("CORS request made for path: {path} from origin: {origin} but was ignored because path was not for an allowed IdentityServer CORS endpoint", path, origin);
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
}
