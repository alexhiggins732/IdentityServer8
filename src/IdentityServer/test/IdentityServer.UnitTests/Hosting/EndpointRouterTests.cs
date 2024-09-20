/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer.UnitTests.Common;
using IdentityServer8.Configuration;
using IdentityServer8.Hosting;
using Microsoft.AspNetCore.Http;
using Xunit;
using static IdentityServer8.Constants;

namespace IdentityServer.UnitTests.Hosting
{
    public class EndpointRouterTests
    {
        private Dictionary<string, IdentityServer8.Hosting.Endpoint> _pathMap;
        private List<IdentityServer8.Hosting.Endpoint> _endpoints;
        private IdentityServerOptions _options;
        private EndpointRouter _subject;

        public EndpointRouterTests()
        {
            _pathMap = new Dictionary<string, IdentityServer8.Hosting.Endpoint>();
            _endpoints = new List<IdentityServer8.Hosting.Endpoint>();
            _options = new IdentityServerOptions();
            _subject = new EndpointRouter(_endpoints, _options, TestLogger.Create<EndpointRouter>());
        }

        [Fact]
        public void Endpoint_ctor_requires_path_to_start_with_slash()
        {
            Action a = () => new IdentityServer8.Hosting.Endpoint("ep1", "ep1", typeof(MyEndpointHandler));
            a.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Find_should_return_null_for_incorrect_path()
        {
            _endpoints.Add(new IdentityServer8.Hosting.Endpoint("ep1", "/ep1", typeof(MyEndpointHandler)));
            _endpoints.Add(new IdentityServer8.Hosting.Endpoint("ep2", "/ep2", typeof(MyOtherEndpointHandler)));

            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString("/wrong");
            ctx.RequestServices = new StubServiceProvider();

            var result = _subject.Find(ctx);
            result.Should().BeNull();
        }

        [Fact]
        public void Find_should_find_path()
        {
            _endpoints.Add(new IdentityServer8.Hosting.Endpoint("ep1", "/ep1", typeof(MyEndpointHandler)));
            _endpoints.Add(new IdentityServer8.Hosting.Endpoint("ep2", "/ep2", typeof(MyOtherEndpointHandler)));

            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString("/ep1");
            ctx.RequestServices = new StubServiceProvider();

            var result = _subject.Find(ctx);
            result.Should().BeOfType<MyEndpointHandler>();
        }

        [Fact]
        public void Find_should_not_find_nested_paths()
        {
            _endpoints.Add(new IdentityServer8.Hosting.Endpoint("ep1", "/ep1", typeof(MyEndpointHandler)));
            _endpoints.Add(new IdentityServer8.Hosting.Endpoint("ep2", "/ep2", typeof(MyOtherEndpointHandler)));

            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString("/ep1/subpath");
            ctx.RequestServices = new StubServiceProvider();

            var result = _subject.Find(ctx);
            result.Should().BeNull();
        }

        [Fact]
        public void Find_should_find_first_registered_mapping()
        {
            _endpoints.Add(new IdentityServer8.Hosting.Endpoint("ep1", "/ep1", typeof(MyEndpointHandler)));
            _endpoints.Add(new IdentityServer8.Hosting.Endpoint("ep1", "/ep1", typeof(MyOtherEndpointHandler)));

            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString("/ep1");
            ctx.RequestServices = new StubServiceProvider();

            var result = _subject.Find(ctx);
            result.Should().BeOfType<MyEndpointHandler>();
        }

        [Fact]
        public void Find_should_return_null_for_disabled_endpoint()
        {
            _endpoints.Add(new IdentityServer8.Hosting.Endpoint(EndpointNames.Authorize, "/ep1", typeof(MyEndpointHandler)));
            _endpoints.Add(new IdentityServer8.Hosting.Endpoint("ep2", "/ep2", typeof(MyOtherEndpointHandler)));

            _options.Endpoints.EnableAuthorizeEndpoint = false;

            var ctx = new DefaultHttpContext();
            ctx.Request.Path = new PathString("/ep1");
            ctx.RequestServices = new StubServiceProvider();

            var result = _subject.Find(ctx);
            result.Should().BeNull();
        }

        private class MyEndpointHandler : IEndpointHandler
        {
            public Task<IEndpointResult> ProcessAsync(HttpContext context)
            {
                throw new NotImplementedException();
            }
        }

        private class MyOtherEndpointHandler : IEndpointHandler
        {
            public Task<IEndpointResult> ProcessAsync(HttpContext context)
            {
                throw new NotImplementedException();
            }
        }

        private class StubServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(MyEndpointHandler)) return new MyEndpointHandler();
                if (serviceType == typeof(MyOtherEndpointHandler)) return new MyOtherEndpointHandler();

                throw new InvalidOperationException();
            }
        }
    }
}
