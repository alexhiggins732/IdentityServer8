/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer.UnitTests.Common;
using IdentityServer8.Configuration;
using IdentityServer8.Endpoints.Results;
using IdentityServer8.Extensions;
using IdentityServer8.Models;
using IdentityServer8.Validation;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace IdentityServer.UnitTests.Endpoints.Results
{
    public class EndSessionCallbackResultTests
    {
        private EndSessionCallbackResult _subject;

        private EndSessionCallbackValidationResult _result = new EndSessionCallbackValidationResult();
        private IdentityServerOptions _options = TestIdentityServerOptions.Create();

        private DefaultHttpContext _context = new DefaultHttpContext();

        public EndSessionCallbackResultTests()
        {
            _context.SetIdentityServerOrigin("https://server");
            _context.SetIdentityServerBasePath("/");
            _context.Response.Body = new MemoryStream();

            _subject = new EndSessionCallbackResult(_result, _options);
        }

        [Fact]
        public async Task error_should_return_400()
        {
            _result.IsError = true;

            await _subject.ExecuteAsync(_context);

            _context.Response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task success_should_render_html_and_iframes()
        {
            _result.IsError = false;
            _result.FrontChannelLogoutUrls = new string[] { "http://foo.com", "http://bar.com" };

            await _subject.ExecuteAsync(_context);

            _context.Response.ContentType.Should().StartWith("text/html");
            _context.Response.Headers["Cache-Control"].First().Should().Contain("no-store");
            _context.Response.Headers["Cache-Control"].First().Should().Contain("no-cache");
            _context.Response.Headers["Cache-Control"].First().Should().Contain("max-age=0");
            _context.Response.Headers["Content-Security-Policy"].First().Should().Contain("default-src 'none';");
            _context.Response.Headers["Content-Security-Policy"].First().Should().Contain("style-src 'sha256-u+OupXgfekP+x/f6rMdoEAspPCYUtca912isERnoEjY=';");
            _context.Response.Headers["Content-Security-Policy"].First().Should().Contain("frame-src http://foo.com http://bar.com");
            _context.Response.Headers["X-Content-Security-Policy"].First().Should().Contain("default-src 'none';");
            _context.Response.Headers["X-Content-Security-Policy"].First().Should().Contain("style-src 'sha256-u+OupXgfekP+x/f6rMdoEAspPCYUtca912isERnoEjY=';");
            _context.Response.Headers["X-Content-Security-Policy"].First().Should().Contain("frame-src http://foo.com http://bar.com");
            _context.Response.Body.Seek(0, SeekOrigin.Begin);
            using (var rdr = new StreamReader(_context.Response.Body))
            {
                var html = rdr.ReadToEnd();
                html.Should().Contain("<iframe src='http://foo.com'></iframe>");
                html.Should().Contain("<iframe src='http://bar.com'></iframe>");
            }
        }

        [Fact]
        public async Task fsuccess_should_add_unsafe_inline_for_csp_level_1()
        {
            _result.IsError = false;

            _options.Csp.Level = CspLevel.One;

            await _subject.ExecuteAsync(_context);

            _context.Response.Headers["Content-Security-Policy"].First().Should().Contain("style-src 'unsafe-inline' 'sha256-u+OupXgfekP+x/f6rMdoEAspPCYUtca912isERnoEjY='");
            _context.Response.Headers["X-Content-Security-Policy"].First().Should().Contain("style-src 'unsafe-inline' 'sha256-u+OupXgfekP+x/f6rMdoEAspPCYUtca912isERnoEjY='");
        }

        [Fact]
        public async Task form_post_mode_should_not_add_deprecated_header_when_it_is_disabled()
        {
            _result.IsError = false;

            _options.Csp.AddDeprecatedHeader = false;

            await _subject.ExecuteAsync(_context);

            _context.Response.Headers["Content-Security-Policy"].First().Should().Contain("style-src 'sha256-u+OupXgfekP+x/f6rMdoEAspPCYUtca912isERnoEjY='");
            _context.Response.Headers["X-Content-Security-Policy"].Should().BeEmpty();
        }
    }
}
