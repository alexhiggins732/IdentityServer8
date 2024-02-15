/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Services;

/// <summary>
/// Default profile service implementation.
/// This implementation sources all claims from the current subject (e.g. the cookie).
/// </summary>
/// <seealso cref="IdentityServer8.Services.IProfileService" />
public class DefaultProfileService : IProfileService
{
    /// <summary>
    /// The logger
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultProfileService"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public DefaultProfileService(ILogger<DefaultProfileService> logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public virtual Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        context.LogProfileRequest(Logger);
        context.AddRequestedClaims(context.Subject.Claims);
        context.LogIssuedClaims(Logger);

        return Task.CompletedTask;
    }

    /// <summary>
    /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
    /// (e.g. during token issuance or validation).
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public virtual Task IsActiveAsync(IsActiveContext context)
    {
        Logger.LogDebug("IsActive called from: {caller}", context.Caller);

        context.IsActive = true;
        return Task.CompletedTask;
    }
}
