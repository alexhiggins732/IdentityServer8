/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Models;

/// <summary>
/// Token request errors
/// </summary>
public enum TokenRequestErrors
{
    /// <summary>
    /// invalid_request
    /// </summary>
    InvalidRequest,

    /// <summary>
    /// invalid_client
    /// </summary>
    InvalidClient,

    /// <summary>
    /// invalid_grant
    /// </summary>
    InvalidGrant,

    /// <summary>
    /// unauthorized_client
    /// </summary>
    UnauthorizedClient,

    /// <summary>
    /// unsupported_grant_type
    /// </summary>
    UnsupportedGrantType,

    /// <summary>
    /// invalid_scope
    /// </summary>
    InvalidScope,

    /// <summary>
    /// invalid_target
    /// </summary>
    InvalidTarget
}
