/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Stores;

/// <summary>
/// Interface for the device flow store
/// </summary>
public interface IDeviceFlowStore
{
    /// <summary>
    /// Stores the device authorization request.
    /// </summary>
    /// <param name="deviceCode">The device code.</param>
    /// <param name="userCode">The user code.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data);

    /// <summary>
    /// Finds device authorization by user code.
    /// </summary>
    /// <param name="userCode">The user code.</param>
    /// <returns></returns>
    Task<DeviceCode> FindByUserCodeAsync(string userCode);

    /// <summary>
    /// Finds device authorization by device code.
    /// </summary>
    /// <param name="deviceCode">The device code.</param>
    Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode);

    /// <summary>
    /// Updates device authorization, searching by user code.
    /// </summary>
    /// <param name="userCode">The user code.</param>
    /// <param name="data">The data.</param>
    Task UpdateByUserCodeAsync(string userCode, DeviceCode data);

    /// <summary>
    /// Removes the device authorization, searching by device code.
    /// </summary>
    /// <param name="deviceCode">The device code.</param>
    Task RemoveByDeviceCodeAsync(string deviceCode);
}
