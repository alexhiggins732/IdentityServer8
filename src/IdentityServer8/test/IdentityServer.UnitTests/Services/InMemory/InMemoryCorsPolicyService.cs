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
using IdentityServer.UnitTests.Common;
using IdentityServer8.Models;
using IdentityServer8.Services;
using Xunit;

namespace IdentityServer.UnitTests.Services.InMemory
{
    public class InMemoryCorsPolicyServiceTests
    {
        private const string Category = "InMemoryCorsPolicyService";

        private InMemoryCorsPolicyService _subject;
        private List<Client> _clients = new List<Client>();

        public InMemoryCorsPolicyServiceTests()
        {
            _subject = new InMemoryCorsPolicyService(TestLogger.Create<InMemoryCorsPolicyService>(), _clients);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task client_has_origin_should_allow_origin()
        {
            _clients.Add(new Client
            {
                AllowedCorsOrigins = new List<string>
                {
                    "http://foo"
                }
            });

            (await _subject.IsOriginAllowedAsync("http://foo")).Should().BeTrue();
        }

        [Theory]
        [InlineData("http://foo")]
        [InlineData("https://bar")]
        [InlineData("http://bar-baz")]
        [Trait("Category", Category)]
        public async Task client_does_not_has_origin_should_not_allow_origin(string clientOrigin)
        {
            _clients.Add(new Client
            {
                AllowedCorsOrigins = new List<string>
                {
                    clientOrigin
                }
            });
            (await _subject.IsOriginAllowedAsync("http://bar")).Should().Be(false);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task client_has_many_origins_and_origin_is_in_list_should_allow_origin()
        {
            _clients.Add(new Client
            {
                AllowedCorsOrigins = new List<string>
                {
                    "http://foo",
                    "http://bar",
                    "http://baz"
                }
            });
            (await _subject.IsOriginAllowedAsync("http://bar")).Should().Be(true);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task client_has_many_origins_and_origin_is_in_not_list_should_not_allow_origin()
        {
            _clients.Add(new Client
            {
                AllowedCorsOrigins = new List<string>
                {
                    "http://foo",
                    "http://bar",
                    "http://baz"
                }
            });
            (await _subject.IsOriginAllowedAsync("http://quux")).Should().Be(false);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task many_clients_have_same_origins_should_allow_origin()
        {
            _clients.AddRange(new Client[] {
                new Client
                {
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://foo"
                    }
                },
                new Client
                {
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://foo"
                    }
                }
            });
            (await _subject.IsOriginAllowedAsync("http://foo")).Should().BeTrue();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task handle_invalid_cors_origin_format_exception()
        {
            _clients.AddRange(new Client[] {
                new Client
                {
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://foo",
                        "http://ba z"
                    }
                },
                new Client
                {
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://foo",
                        "http://bar"
                    }
                }
            });
            (await _subject.IsOriginAllowedAsync("http://bar")).Should().BeTrue();
        }
    }
}
