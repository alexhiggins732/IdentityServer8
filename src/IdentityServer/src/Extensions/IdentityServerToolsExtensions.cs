/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8;

/// <summary>
/// Extensions for IdentityServerTools
/// </summary>
public static class IdentityServerToolsExtensions
{
    /// <summary>
    /// Issues the client JWT.
    /// </summary>
    /// <param name="tools">The tools.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="lifetime">The lifetime.</param>
    /// <param name="scopes">The scopes.</param>
    /// <param name="audiences">The audiences.</param>
    /// <param name="additionalClaims">Additional claims</param>
    /// <returns></returns>
    public static async Task<string> IssueClientJwtAsync(this IdentityServerTools tools,
        string clientId,
        int lifetime,
        IEnumerable<string> scopes = null,
        IEnumerable<string> audiences = null,
        IEnumerable<Claim> additionalClaims = null)
    {
        var claims = new HashSet<Claim>(new ClaimComparer());
        var context = tools.ContextAccessor.HttpContext;
        var options = context.RequestServices.GetRequiredService<IdentityServerOptions>();
        
        if (additionalClaims != null)
        {
            foreach (var claim in additionalClaims)
            {
                claims.Add(claim);
            }
        }

        claims.Add(new Claim(JwtClaimTypes.ClientId, clientId));

        if (!scopes.EnumerableIsNullOrEmpty())
        {
            foreach (var scope in scopes)
            {
                claims.Add(new Claim(JwtClaimTypes.Scope, scope));
            }
        }

        if (options.EmitStaticAudienceClaim)
        {
            claims.Add(new Claim(JwtClaimTypes.Audience, string.Format(IdentityServerConstants.AccessTokenAudience, tools.ContextAccessor.HttpContext.GetIdentityServerIssuerUri().EnsureTrailingSlash())));
        }
        
        if (!audiences.EnumerableIsNullOrEmpty())
        {
            foreach (var audience in audiences)
            {
                claims.Add(new Claim(JwtClaimTypes.Audience, audience));
            }
        }

        return await tools.IssueJwtAsync(lifetime, claims);
    }
}
