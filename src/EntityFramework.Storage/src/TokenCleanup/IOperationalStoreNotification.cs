/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.EntityFramework.Entities;

namespace IdentityServer8.EntityFramework
{
    /// <summary>
    /// Interface to model notifications from the TokenCleanup feature.
    /// </summary>
    public interface IOperationalStoreNotification
    {
        /// <summary>
        /// Notification for persisted grants being removed.
        /// </summary>
        /// <param name="persistedGrants"></param>
        /// <returns></returns>
        Task PersistedGrantsRemovedAsync(IEnumerable<PersistedGrant> persistedGrants);

        /// <summary>
        /// Notification for device codes being removed.
        /// </summary>
        /// <param name="deviceCodes"></param>
        /// <returns></returns>
        Task DeviceCodesRemovedAsync(IEnumerable<DeviceFlowCodes> deviceCodes);
    }
}
