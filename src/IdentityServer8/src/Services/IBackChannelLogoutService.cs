/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityServer8.Models;
using System.Threading.Tasks;

namespace IdentityServer8.Services
{
    /// <summary>
    /// The service responsible for performing back-channel logout notification.
    /// </summary>
    public interface IBackChannelLogoutService
    {
        /// <summary>
        /// Performs http back-channel logout notification.
        /// </summary>
        /// <param name="context">The context of the back channel logout notification.</param>
        Task SendLogoutNotificationsAsync(LogoutNotificationContext context);
    }

    
}
