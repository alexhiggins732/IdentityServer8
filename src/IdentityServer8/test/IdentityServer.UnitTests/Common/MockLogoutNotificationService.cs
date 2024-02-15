/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.Models;
using IdentityServer8.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.UnitTests.Common
{
    public class MockLogoutNotificationService : ILogoutNotificationService
    {
        public bool GetFrontChannelLogoutNotificationsUrlsCalled { get; set; }
        public List<string> FrontChannelLogoutNotificationsUrls { get; set; } = new List<string>();

        public bool SendBackChannelLogoutNotificationsCalled { get; set; }
        public List<BackChannelLogoutRequest> BackChannelLogoutRequests { get; set; } = new List<BackChannelLogoutRequest>();

        public Task<IEnumerable<string>> GetFrontChannelLogoutNotificationsUrlsAsync(LogoutNotificationContext context)
        {
            GetFrontChannelLogoutNotificationsUrlsCalled = true;
            return Task.FromResult(FrontChannelLogoutNotificationsUrls.AsEnumerable());
        }

        public Task<IEnumerable<BackChannelLogoutRequest>> GetBackChannelLogoutNotificationsAsync(LogoutNotificationContext context)
        {
            SendBackChannelLogoutNotificationsCalled = true;
            return Task.FromResult(BackChannelLogoutRequests.AsEnumerable());
        }
    }
}
