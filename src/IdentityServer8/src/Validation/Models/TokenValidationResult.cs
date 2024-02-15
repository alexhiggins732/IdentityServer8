/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Validation;

/// <summary>
/// Models the validation result of access tokens and id tokens.
/// </summary>
public class TokenValidationResult : ValidationResult
{
    /// <summary>
    /// Gets or sets the claims.
    /// </summary>
    /// <value>
    /// The claims.
    /// </value>
    public IEnumerable<Claim> Claims { get; set; }
    
    /// <summary>
    /// Gets or sets the JWT.
    /// </summary>
    /// <value>
    /// The JWT.
    /// </value>
    public string Jwt { get; set; }

    /// <summary>
    /// Gets or sets the reference token (in case of access token validation).
    /// </summary>
    /// <value>
    /// The reference token.
    /// </value>
    public Token ReferenceToken { get; set; }

    /// <summary>
    /// Gets or sets the reference token identifier (in case of access token validation).
    /// </summary>
    /// <value>
    /// The reference token identifier.
    /// </value>
    public string ReferenceTokenId { get; set; }

    /// <summary>
    /// Gets or sets the refresh token (in case of refresh token validation).
    /// </summary>
    /// <value>
    /// The reference token identifier.
    /// </value>
    public RefreshToken RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    /// <value>
    /// The client.
    /// </value>
    public Client Client { get; set; }
}
