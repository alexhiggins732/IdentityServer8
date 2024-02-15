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
/// Models a validated request to the token endpoint.
/// </summary>
public class ValidatedTokenRequest : ValidatedRequest
{
    /// <summary>
    /// Gets or sets the type of the grant.
    /// </summary>
    /// <value>
    /// The type of the grant.
    /// </value>
    public string GrantType { get; set; }
    
    /// <summary>
    /// Gets or sets the scopes.
    /// </summary>
    /// <value>
    /// The scopes.
    /// </value>
    public IEnumerable<string> RequestedScopes { get; set; }

    /// <summary>
    /// Gets or sets the username used in the request.
    /// </summary>
    /// <value>
    /// The name of the user.
    /// </value>
    public string UserName { get; set; }
    
    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    /// <value>
    /// The refresh token.
    /// </value>
    public RefreshToken RefreshToken { get; set; }
    
    /// <summary>
    /// Gets or sets the refresh token handle.
    /// </summary>
    /// <value>
    /// The refresh token handle.
    /// </value>
    public string RefreshTokenHandle { get; set; }

    /// <summary>
    /// Gets or sets the authorization code.
    /// </summary>
    /// <value>
    /// The authorization code.
    /// </value>
    public AuthorizationCode AuthorizationCode { get; set; }

    /// <summary>
    /// Gets or sets the authorization code handle.
    /// </summary>
    /// <value>
    /// The authorization code handle.
    /// </value>
    public string AuthorizationCodeHandle { get; set; }

    /// <summary>
    /// Gets or sets the code verifier.
    /// </summary>
    /// <value>
    /// The code verifier.
    /// </value>
    public string CodeVerifier { get; set; }

    /// <summary>
    /// Gets or sets the device code.
    /// </summary>
    /// <value>
    /// The device code.
    /// </value>
    public DeviceCode DeviceCode { get; set; }
}
