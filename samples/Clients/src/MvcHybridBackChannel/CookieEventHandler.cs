/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

public class CookieEventHandler : CookieAuthenticationEvents
{
    public CookieEventHandler(LogoutSessionManager logoutSessions)
    {
        LogoutSessions = logoutSessions;
    }

    public LogoutSessionManager LogoutSessions { get; }

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        if (context.Principal.Identity.IsAuthenticated)
        {
            var sub = context.Principal.FindFirst("sub")?.Value;
            var sid = context.Principal.FindFirst("sid")?.Value;

            if (LogoutSessions.IsLoggedOut(sub, sid))
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync();

                // todo: if we have a refresh token, it should be revoked here.
            }
        }
    }
}
