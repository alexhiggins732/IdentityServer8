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
/// Validation result for introspection request
/// </summary>
/// <seealso cref="IdentityServer8.Validation.ValidationResult" />
public class IntrospectionRequestValidationResult : ValidationResult
{
    /// <summary>
    /// Gets or sets the request parameters.
    /// </summary>
    /// <value>
    /// The parameters.
    /// </value>
    public NameValueCollection Parameters { get; set; }

    /// <summary>
    /// Gets or sets the API.
    /// </summary>
    /// <value>
    /// The API.
    /// </value>
    public ApiResource Api { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the token is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the token is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the claims.
    /// </summary>
    /// <value>
    /// The claims.
    /// </value>
    public IEnumerable<Claim> Claims { get; set; }

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    /// <value>
    /// The token.
    /// </value>
    public string Token { get; set; }
}

/// <summary>
/// Failure reasons for introspection request
/// </summary>
public enum IntrospectionRequestValidationFailureReason
{
    /// <summary>
    /// none
    /// </summary>
    None,

    /// <summary>
    /// missing token
    /// </summary>
    MissingToken,

    /// <summary>
    /// invalid token
    /// </summary>
    InvalidToken,

    /// <summary>
    /// invalid scope
    /// </summary>
    InvalidScope
}
