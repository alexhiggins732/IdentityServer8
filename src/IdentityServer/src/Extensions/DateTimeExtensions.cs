/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Extensions;

internal static class DateTimeExtensions
{
    [DebuggerStepThrough]
    public static bool HasExceeded(this DateTime creationTime, int seconds, DateTime now)
    {
        return (now > creationTime.AddSeconds(seconds));
    }

    [DebuggerStepThrough]
    public static int GetLifetimeInSeconds(this DateTime creationTime, DateTime now)
    {
        return ((int)(now - creationTime).TotalSeconds);
    }

    [DebuggerStepThrough]
    public static bool HasExpired(this DateTime? expirationTime, DateTime now)
    {
        if (expirationTime.HasValue &&
            expirationTime.Value.HasExpired(now))
        {
            return true;
        }

        return false;
    }

    [DebuggerStepThrough]
    public static bool HasExpired(this DateTime expirationTime, DateTime now)
    {
        if (now > expirationTime)
        {
            return true;
        }

        return false;
    }
}
