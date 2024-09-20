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
/// In-memory device flow store
/// </summary>
/// <seealso cref="IdentityServer8.Stores.IDeviceFlowStore" />
public class InMemoryDeviceFlowStore : IDeviceFlowStore
{
    private readonly List<InMemoryDeviceAuthorization> _repository = new List<InMemoryDeviceAuthorization>();

    /// <summary>
    /// Stores the device authorization request.
    /// </summary>
    /// <param name="deviceCode">The device code.</param>
    /// <param name="userCode">The user code.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data)
    {
        lock (_repository)
        {
            _repository.Add(new InMemoryDeviceAuthorization(deviceCode, userCode, data));
        }
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Finds device authorization by user code.
    /// </summary>
    /// <param name="userCode">The user code.</param>
    public Task<DeviceCode> FindByUserCodeAsync(string userCode)
    {
        DeviceCode foundDeviceCode;

        lock (_repository)
        {
            foundDeviceCode = _repository.FirstOrDefault(x => x.UserCode == userCode)?.Data;
        }

        return Task.FromResult(foundDeviceCode);
    }

    /// <summary>
    /// Finds device authorization by device code.
    /// </summary>
    /// <param name="deviceCode">The device code.</param>
    public Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
    {
        DeviceCode foundDeviceCode;

        lock (_repository)
        {
            foundDeviceCode = _repository.FirstOrDefault(x => x.DeviceCode == deviceCode)?.Data;
        }

        return Task.FromResult(foundDeviceCode);
    }

    /// <summary>
    /// Updates device authorization, searching by user code.
    /// </summary>
    /// <param name="userCode">The user code.</param>
    /// <param name="data">The data.</param>
    public Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
    {
        lock (_repository)
        {
            var foundData = _repository.FirstOrDefault(x => x.UserCode == userCode);

            if (foundData != null)
            {
                foundData.Data = data;
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Removes the device authorization, searching by device code.
    /// </summary>
    /// <param name="deviceCode">The device code.</param>
    /// <returns></returns>
    public Task RemoveByDeviceCodeAsync(string deviceCode)
    {
        lock (_repository)
        {
            var foundData = _repository.FirstOrDefault(x => x.DeviceCode == deviceCode);

            if (foundData != null)
            {
                _repository.Remove(foundData);
            }
        }


        return Task.CompletedTask;
    }

    private class InMemoryDeviceAuthorization
    {
        public InMemoryDeviceAuthorization(string deviceCode, string userCode, DeviceCode data)
        {
            DeviceCode = deviceCode;
            UserCode = userCode;
            Data = data;
        }

        public string DeviceCode { get; }
        public string UserCode { get; }
        public DeviceCode Data { get; set; }
    }
}
