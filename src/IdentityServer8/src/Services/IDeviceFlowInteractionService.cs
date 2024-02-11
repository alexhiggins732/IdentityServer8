/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Threading.Tasks;
using IdentityServer8.Models;

namespace IdentityServer8.Services
{
    /// <summary>
    ///  Provide services be used by the user interface to communicate with IdentityServer.
    /// </summary>
    public interface IDeviceFlowInteractionService
    {
        /// <summary>
        /// Gets the authorization context asynchronous.
        /// </summary>
        /// <param name="userCode">The user code.</param>
        /// <returns></returns>
        Task<DeviceFlowAuthorizationRequest> GetAuthorizationContextAsync(string userCode);

        /// <summary>
        /// Handles the request asynchronous.
        /// </summary>
        /// <param name="userCode">The user code.</param>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        Task<DeviceFlowInteractionResult> HandleRequestAsync(string userCode, ConsentResponse consent);
    }
}