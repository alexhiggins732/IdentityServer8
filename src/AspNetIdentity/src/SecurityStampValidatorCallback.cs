/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.AspNetIdentity;

/// <summary>
/// Implements callback for SecurityStampValidator's OnRefreshingPrincipal event.
/// </summary>
public class SecurityStampValidatorCallback
{
    /// <summary>
    /// Maintains the claims captured at login time that are not being created by ASP.NET Identity.
    /// This is needed to preserve claims such as idp, auth_time, amr.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public static Task UpdatePrincipal(SecurityStampRefreshingPrincipalContext context)
    {
        var newClaimTypes = context.NewPrincipal.Claims.Select(x => x.Type).ToArray();
        var currentClaimsToKeep = context.CurrentPrincipal.Claims.Where(x => !newClaimTypes.Contains(x.Type)).ToArray();

        var id = context.NewPrincipal.Identities.First();
        id.AddClaims(currentClaimsToKeep);

        return Task.CompletedTask;
    }
}
