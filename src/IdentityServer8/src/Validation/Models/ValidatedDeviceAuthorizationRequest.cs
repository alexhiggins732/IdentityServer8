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
/// Models a validated request to the device authorization endpoint.
/// </summary>
public class ValidatedDeviceAuthorizationRequest : ValidatedRequest
{
    /// <summary>
    /// Gets or sets the requested scopes.
    /// </summary>
    /// <value>
    /// The scopes.
    /// </value>
    public IEnumerable<string> RequestedScopes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is open identifier request.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is open identifier request; otherwise, <c>false</c>.
    /// </value>
    public bool IsOpenIdRequest { get; set; }

    /// <summary>
    /// Gets the description the user assigned to the device being authorized.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; }
}
