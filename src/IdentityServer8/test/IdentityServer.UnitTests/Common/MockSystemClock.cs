/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Microsoft.AspNetCore.Authentication;
using System;

namespace IdentityServer.UnitTests.Common
{
    class MockSystemClock : ISystemClock
    {
        public DateTimeOffset Now { get; set; }

        public DateTimeOffset UtcNow
        {
            get
            {
                return Now;
            }
        }
    }
}
