/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.ResponseHandling;

/// <summary>
/// Interface for the device authorization response generator
/// </summary>
public interface IDeviceAuthorizationResponseGenerator
{
    /// <summary>
    /// Processes the response.
    /// </summary>
    /// <param name="validationResult">The validation result.</param>
    /// <param name="baseUrl">The base URL.</param>
    /// <returns></returns>
    Task<DeviceAuthorizationResponse> ProcessAsync(DeviceAuthorizationRequestValidationResult validationResult, string baseUrl);
}
