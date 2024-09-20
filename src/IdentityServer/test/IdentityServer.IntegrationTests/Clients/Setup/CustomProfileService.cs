/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Threading.Tasks;
using IdentityServer8.Models;
using IdentityServer8.Test;
using Microsoft.Extensions.Logging;

namespace IdentityServer.IntegrationTests.Clients.Setup;

class CustomProfileService : TestUserProfileService
{
    public CustomProfileService(TestUserStore users, ILogger<TestUserProfileService> logger) : base(users, logger)
    { }

    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        await base.GetProfileDataAsync(context);

        if (context.Subject.Identity.AuthenticationType == "custom")
        {
            var extraClaim = context.Subject.FindFirst("extra_claim");
            if (extraClaim != null)
            {
                context.IssuedClaims.Add(extraClaim);
            }
        }
    }
}
