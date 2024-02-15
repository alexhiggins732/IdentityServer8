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
/// Represents contextual information about a device flow authorization request.
/// </summary>
public class DeviceFlowAuthorizationRequest
{
    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    /// <value>
    /// The client.
    /// </value>
    public Client Client { get; set; }

    /// <summary>
    /// Gets or sets the validated resources.
    /// </summary>
    /// <value>
    /// The scopes requested.
    /// </value>
    public ResourceValidationResult ValidatedResources { get; set; }
}
