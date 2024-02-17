/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using FluentAssertions;
using IdentityServer8.Extensions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace IdentityServer.UnitTests.Extensions
{
    public class HttpRequestExtensionsTests
    {
        [Fact]
        public void GetCorsOrigin_valid_cors_request_should_return_cors_origin()
        {
            var ctx = new DefaultHttpContext();
            ctx.Request.Scheme = "http";
            ctx.Request.Host = new HostString("foo");
            ctx.Request.Headers.Append("Origin", "http://bar");

            ctx.Request.GetCorsOrigin().Should().Be("http://bar");
        }

        [Fact]
        public void GetCorsOrigin_origin_from_same_host_should_not_return_cors_origin()
        {
            var ctx = new DefaultHttpContext();
            ctx.Request.Scheme = "http";
            ctx.Request.Host = new HostString("foo");
            ctx.Request.Headers.Append("Origin", "http://foo");

            ctx.Request.GetCorsOrigin().Should().BeNull();
        }

        [Fact]
        public void GetCorsOrigin_no_origin_should_not_return_cors_origin()
        {
            var ctx = new DefaultHttpContext();
            ctx.Request.Scheme = "http";
            ctx.Request.Host = new HostString("foo");

            ctx.Request.GetCorsOrigin().Should().BeNull();
        }
    }
}
