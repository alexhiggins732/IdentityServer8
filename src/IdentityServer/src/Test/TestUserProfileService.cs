/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Test;

/// <summary>
/// Profile service for test users
/// </summary>
/// <seealso cref="IdentityServer.Services.IProfileService" />
public class TestUserProfileService : IProfileService
{
    /// <summary>
    /// The logger
    /// </summary>
    protected readonly ILogger Logger;
    
    /// <summary>
    /// The users
    /// </summary>
    protected readonly TestUserStore Users;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestUserProfileService"/> class.
    /// </summary>
    /// <param name="users">The users.</param>
    /// <param name="logger">The logger.</param>
    public TestUserProfileService(TestUserStore users, ILogger<TestUserProfileService> logger)
    {
        Users = users;
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

        if (context.RequestedClaimTypes.Any())
        {
            var user = Users.FindBySubjectId(context.Subject.GetSubjectId());
            if (user != null)
            {
                context.AddRequestedClaims(user.Claims);
            }
        }

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

        var user = Users.FindBySubjectId(context.Subject.GetSubjectId());
        context.IsActive = user?.IsActive == true;

        return Task.CompletedTask;
    }
}
