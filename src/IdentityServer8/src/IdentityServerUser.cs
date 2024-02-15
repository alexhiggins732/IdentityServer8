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
/// Model properties of an IdentityServer user
/// </summary>
public class IdentityServerUser
{
    /// <summary>
    /// Subject ID (mandatory)
    /// </summary>
    public string SubjectId { get; }

    /// <summary>
    /// Display name (optional)
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Identity provider (optional)
    /// </summary>
    public string IdentityProvider { get; set; }

    /// <summary>
    /// Authentication methods
    /// </summary>
    public ICollection<string> AuthenticationMethods { get; set; } = new HashSet<string>();

    /// <summary>
    /// Authentication time
    /// </summary>
    public DateTime? AuthenticationTime { get; set; }

    /// <summary>
    /// Additional claims
    /// </summary>
    public ICollection<Claim> AdditionalClaims { get; set; } = new HashSet<Claim>(new ClaimComparer());

    /// <summary>
    /// Initializes a user identity
    /// </summary>
    /// <param name="subjectId">The subject ID</param>
    public IdentityServerUser(string subjectId)
    {
        if (subjectId.IsMissing()) throw new ArgumentException("SubjectId is mandatory", nameof(subjectId));

        SubjectId = subjectId;
    }

    /// <summary>
    /// Creates an IdentityServer claims principal
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public ClaimsPrincipal CreatePrincipal()
    {
        if (SubjectId.IsMissing()) throw new ArgumentException("SubjectId is mandatory", nameof(SubjectId));
        var claims = new List<Claim> { new Claim(JwtClaimTypes.Subject, SubjectId) };

        if (DisplayName.IsPresent())
        {
            claims.Add(new Claim(JwtClaimTypes.Name, DisplayName));
        }

        if (IdentityProvider.IsPresent())
        {
            claims.Add(new Claim(JwtClaimTypes.IdentityProvider, IdentityProvider));
        }

        if (AuthenticationTime.HasValue)
        {
            claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, new DateTimeOffset(AuthenticationTime.Value).ToUnixTimeSeconds().ToString()));
        }

        if (AuthenticationMethods.Any())
        {
            foreach (var amr in AuthenticationMethods)
            {
                claims.Add(new Claim(JwtClaimTypes.AuthenticationMethod, amr));
            }
        }

        claims.AddRange(AdditionalClaims);

        var id = new ClaimsIdentity(claims.Distinct(new ClaimComparer()), Constants.IdentityServerAuthenticationType, JwtClaimTypes.Name, JwtClaimTypes.Role);
        return new ClaimsPrincipal(id);
    }
}
