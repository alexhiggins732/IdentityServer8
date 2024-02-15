/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.IO;
using System.Text;
using FluentAssertions;
using IdentityServer.UnitTests.Common;
using IdentityServer8;
using IdentityServer8.Configuration;
using IdentityServer8.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Xunit;

namespace IdentityServer.UnitTests.Validation.Secrets
{
    public class FormPostCredentialExtraction
    {
        private const string Category = "Secrets - Form Post Secret Parsing";

        private IdentityServerOptions _options;
        private PostBodySecretParser _parser;

        public FormPostCredentialExtraction()
        {
            _options = new IdentityServerOptions();
            _parser = new PostBodySecretParser(_options, new LoggerFactory().CreateLogger<PostBodySecretParser>());
        }

        [Fact]
        [Trait("Category", Category)]
        public async void EmptyContext()
        {
            var context = new DefaultHttpContext();
            context.Request.Body = new MemoryStream();

            var secret = await _parser.ParseAsync(context);

            secret.Should().BeNull();
        }

        [Fact]
        [Trait("Category", Category)]
        public async void Valid_PostBody()
        {
            var context = new DefaultHttpContext();

            var body = "client_id=client&client_secret=secret";

            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            context.Request.ContentType = "application/x-www-form-urlencoded";

            var secret = await _parser.ParseAsync(context);

            secret.Type.Should().Be(IdentityServerConstants.ParsedSecretTypes.SharedSecret);
            secret.Id.Should().Be("client");
            secret.Credential.Should().Be("secret");
        }

        [Fact]
        [Trait("Category", Category)]
        public async void ClientId_Too_Long()
        {
            var context = new DefaultHttpContext();

            var longClientId = "x".Repeat(_options.InputLengthRestrictions.ClientId + 1);
            var body = string.Format("client_id={0}&client_secret=secret", longClientId);

            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            context.Request.ContentType = "application/x-www-form-urlencoded";

            var secret = await _parser.ParseAsync(context);

            secret.Should().BeNull();
        }

        [Fact]
        [Trait("Category", Category)]
        public async void ClientSecret_Too_Long()
        {
            var context = new DefaultHttpContext();

            var longClientSecret = "x".Repeat(_options.InputLengthRestrictions.ClientSecret + 1);
            var body = string.Format("client_id=client&client_secret={0}", longClientSecret);

            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            context.Request.ContentType = "application/x-www-form-urlencoded";

            var secret = await _parser.ParseAsync(context);

            secret.Should().BeNull();
        }

        [Fact]
        [Trait("Category", Category)]
        public async void Missing_ClientId()
        {
            var context = new DefaultHttpContext();

            var body = "client_secret=secret";

            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            context.Request.ContentType = "application/x-www-form-urlencoded";

            var secret = await _parser.ParseAsync(context);

            secret.Should().BeNull();
        }

        [Fact]
        [Trait("Category", Category)]
        public async void Missing_ClientSecret()
        {
            var context = new DefaultHttpContext();

            var body = "client_id=client";

            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            context.Request.ContentType = "application/x-www-form-urlencoded";

            var secret = await _parser.ParseAsync(context);

            secret.Should().NotBeNull();
            secret.Type.Should().Be(IdentityServerConstants.ParsedSecretTypes.NoSecret);
        }

        [Fact]
        [Trait("Category", Category)]
        public async void Malformed_PostBody()
        {
            var context = new DefaultHttpContext();

            var body = "malformed";

            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            context.Request.ContentType = "application/x-www-form-urlencoded";

            var secret = await _parser.ParseAsync(context);

            secret.Should().BeNull();
        }
    }
}
