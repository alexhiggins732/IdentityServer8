/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Validation;

/// <summary>
/// Validation result for device authorization requests
/// </summary>
public class DeviceAuthorizationRequestValidationResult : ValidationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceAuthorizationRequestValidationResult"/> class.
    /// </summary>
    /// <param name="request">The request.</param>
    public DeviceAuthorizationRequestValidationResult(ValidatedDeviceAuthorizationRequest request)
    {
        IsError = false;

        ValidatedRequest = request;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceAuthorizationRequestValidationResult"/> class.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="error">The error.</param>
    /// <param name="errorDescription">The error description.</param>
    public DeviceAuthorizationRequestValidationResult(ValidatedDeviceAuthorizationRequest request, string error, string errorDescription = null)
    {
        IsError = true;

        Error = error;
        ErrorDescription = errorDescription;
        ValidatedRequest = request;
    }

    /// <summary>
    /// Gets the validated request.
    /// </summary>
    /// <value>
    /// The validated request.
    /// </value>
    public ValidatedDeviceAuthorizationRequest ValidatedRequest { get; }
}
