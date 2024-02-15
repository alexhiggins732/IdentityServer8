/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.EntityFramework.Services;

/// <summary>
/// Implementation of ICorsPolicyService that consults the client configuration in the database for allowed CORS origins.
/// </summary>
/// <seealso cref="IdentityServer8.Services.ICorsPolicyService" />
public class CorsPolicyService : ICorsPolicyService
{
    private readonly IHttpContextAccessor _context;
    private readonly ILogger<CorsPolicyService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorsPolicyService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">context</exception>
    public CorsPolicyService(IHttpContextAccessor context, ILogger<CorsPolicyService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    /// <summary>
    /// Determines whether origin is allowed.
    /// </summary>
    /// <param name="origin">The origin.</param>
    /// <returns></returns>
    public async Task<bool> IsOriginAllowedAsync(string origin)
    {
        origin = origin.ToLowerInvariant();

        // doing this here and not in the ctor because: https://github.com/aspnet/CORS/issues/105
        var dbContext = _context.HttpContext.RequestServices.GetRequiredService<IConfigurationDbContext>();

        var query = from o in dbContext.ClientCorsOrigins
                    where o.Origin == origin
                    select o;
        
        var isAllowed = await query.AnyAsync();

        _logger.LogDebug("Origin {origin} is allowed: {originAllowed}", Ioc.Sanitizer.Log.Sanitize(origin), isAllowed);

        return isAllowed;
    }
}
