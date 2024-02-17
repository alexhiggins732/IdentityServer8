/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Events;

/// <summary>
/// Event for device authorization failure
/// </summary>
/// <seealso cref="IdentityServer8.Events.Event" />
public class DeviceAuthorizationFailureEvent : Event
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceAuthorizationFailureEvent"/> class.
    /// </summary>
    /// <param name="result">The result.</param>
    public DeviceAuthorizationFailureEvent(DeviceAuthorizationRequestValidationResult result)
        : this()
    {
        if (result.ValidatedRequest != null)
        {
            ClientId = result.ValidatedRequest.Client?.ClientId;
            ClientName = result.ValidatedRequest.Client?.ClientName;
            Scopes = result.ValidatedRequest.RequestedScopes?.ToSpaceSeparatedString();
            
        }

        Endpoint = Constants.EndpointNames.DeviceAuthorization;
        Error = result.Error;
        ErrorDescription = result.ErrorDescription;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceAuthorizationFailureEvent"/> class.
    /// </summary>
    public DeviceAuthorizationFailureEvent()
        : base(EventCategories.DeviceFlow,
            "Device Authorization Failure",
            EventTypes.Failure,
            EventIds.DeviceAuthorizationFailure)
    {
    }

    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    /// <value>
    /// The client identifier.
    /// </value>
    public string ClientId { get; set; }

    /// <summary>
    /// Gets or sets the name of the client.
    /// </summary>
    /// <value>
    /// The name of the client.
    /// </value>
    public string ClientName { get; set; }

    /// <summary>
    /// Gets or sets the endpoint.
    /// </summary>
    /// <value>
    /// The endpoint.
    /// </value>
    public string Endpoint { get; set; }

    /// <summary>
    /// Gets or sets the scopes.
    /// </summary>
    /// <value>
    /// The scopes.
    /// </value>
    public string Scopes { get; set; }
    
    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    /// <value>
    /// The error.
    /// </value>
    public string Error { get; set; }

    /// <summary>
    /// Gets or sets the error description.
    /// </summary>
    /// <value>
    /// The error description.
    /// </value>
    public string ErrorDescription { get; set; }
}
