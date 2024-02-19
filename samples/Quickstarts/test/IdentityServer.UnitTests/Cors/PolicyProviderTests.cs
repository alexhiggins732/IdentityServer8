/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using  IdentityServer.Common;
using IdentityServer8.Configuration;
using IdentityServer8.Configuration.DependencyInjection;
using IdentityServer8.Hosting;
using IdentityServer8.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace  IdentityServer.Cors
{
    public class PolicyProviderTests
    {
        private const string Category = "PolicyProvider";

        private CorsPolicyProvider _subject;
        private List<string> _allowedPaths = new List<string>();

        private MockCorsPolicyProvider _mockInner = new MockCorsPolicyProvider();
        private MockCorsPolicyService _mockPolicy = new MockCorsPolicyService();
        private IdentityServerOptions _options;

        public PolicyProviderTests()
        {
            Init();
        }

        internal void Init()
        {
            _options = new IdentityServerOptions();
            _options.Cors.CorsPaths.Clear();
            foreach(var path in _allowedPaths)
            {
                _options.Cors.CorsPaths.Add(new PathString(path));
            }

            var ctx = new DefaultHttpContext();
            var svcs = new ServiceCollection();
            svcs.AddSingleton<ICorsPolicyService>(_mockPolicy);
            ctx.RequestServices = svcs.BuildServiceProvider();
            var ctxAccessor = new HttpContextAccessor();
            ctxAccessor.HttpContext = ctx;

            _subject = new CorsPolicyProvider(
                TestLogger.Create<CorsPolicyProvider>(),
                new Decorator<ICorsPolicyProvider>(_mockInner),
                _options,
                ctxAccessor);
        }

        [Theory]
        [InlineData("/foo")]
        [InlineData("/bar/")]
        [InlineData("/baz/quux")]
        [InlineData("/baz/quux/")]
        [Trait("Category", Category)]
        public async Task valid_paths_should_call_policy_service(string path)
        {
            _allowedPaths.AddRange(new string[] {
                "/foo",
                "/bar/",
                "/baz/quux",
                "/baz/quux/"
            });
            Init();

            var ctx = new DefaultHttpContext();
            ctx.Request.Scheme = "https";
            ctx.Request.Host = new HostString("server");
            ctx.Request.Path = new PathString(path);
            ctx.Request.Headers.Append("Origin", "http://notserver");

            var response = await _subject.GetPolicyAsync(ctx, _options.Cors.CorsPolicyName);

            _mockPolicy.WasCalled.Should().BeTrue();
            _mockInner.WasCalled.Should().BeFalse();
        }

        [Theory]
        [InlineData("/foo/")]
        [InlineData("/xoxo")]
        [InlineData("/xoxo/")]
        [InlineData("/foo/xoxo")]
        [InlineData("/baz/quux/xoxo")]
        [Trait("Category", Category)]
        public async Task invalid_paths_should_not_call_policy_service(string path)
        {
            _allowedPaths.AddRange(new string[] {
                "/foo",
                "/bar",
                "/baz/quux"
            });
            Init();

            var ctx = new DefaultHttpContext();
            ctx.Request.Scheme = "https";
            ctx.Request.Host = new HostString("server");
            ctx.Request.Path = new PathString(path);
            ctx.Request.Headers.Append("Origin", "http://notserver");

            var response = await _subject.GetPolicyAsync(ctx, _options.Cors.CorsPolicyName);

            _mockPolicy.WasCalled.Should().BeFalse();
            _mockInner.WasCalled.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task different_policy_name_should_call_inner_policy_service()
        {
            _allowedPaths.AddRange(new string[] {
                "/foo",
                "/bar",
                "/baz/quux"
            });
            Init();

            var ctx = new DefaultHttpContext();
            ctx.Request.Scheme = "https";
            ctx.Request.Host = new HostString("server");
            ctx.Request.Path = new PathString("/foo");
            ctx.Request.Headers.Append("Origin", "http://notserver");

            var response = await _subject.GetPolicyAsync(ctx, "wrong_name");

            _mockPolicy.WasCalled.Should().BeFalse();
            _mockInner.WasCalled.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task origin_same_as_server_should_not_call_policy()
        {
            _allowedPaths.AddRange(new string[] {
                "/foo"
            });
            Init();

            var ctx = new DefaultHttpContext();
            ctx.Request.Scheme = "https";
            ctx.Request.Host = new HostString("server");
            ctx.Request.Path = new PathString("/foo");
            ctx.Request.Headers.Append("Origin", "https://server");

            var response = await _subject.GetPolicyAsync(ctx, _options.Cors.CorsPolicyName);

            _mockPolicy.WasCalled.Should().BeFalse();
            _mockInner.WasCalled.Should().BeFalse();
        }

        [Theory]
        [InlineData("https://notserver")]
        [InlineData("http://server")]
        [Trait("Category", Category)]
        public async Task origin_not_same_as_server_should_call_policy(string origin)
        {
            _allowedPaths.AddRange(new string[] {
                "/foo"
            });
            Init();

            var ctx = new DefaultHttpContext();
            ctx.Request.Scheme = "https";
            ctx.Request.Host = new HostString("server");
            ctx.Request.Path = new PathString("/foo");
            ctx.Request.Headers.Append("Origin", origin);

            var response = await _subject.GetPolicyAsync(ctx, _options.Cors.CorsPolicyName);

            _mockPolicy.WasCalled.Should().BeTrue();
            _mockInner.WasCalled.Should().BeFalse();
        }
    }
}
