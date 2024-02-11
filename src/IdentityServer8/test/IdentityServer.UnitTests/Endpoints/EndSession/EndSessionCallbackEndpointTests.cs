/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityServer8.Endpoints;
using Microsoft.Extensions.Logging;

namespace IdentityServer.UnitTests.Endpoints.EndSession
{
    public class EndSessionCallbackEndpointTests
    {
        private const string Category = "End Session Callback Endpoint";

        StubEndSessionRequestValidator _stubEndSessionRequestValidator = new StubEndSessionRequestValidator();
        EndSessionCallbackEndpoint _subject;

        public EndSessionCallbackEndpointTests()
        {
            _subject = new EndSessionCallbackEndpoint(
                _stubEndSessionRequestValidator,
                new LoggerFactory().CreateLogger<EndSessionCallbackEndpoint>());
        }
    }
}
