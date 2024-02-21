/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 

 Copyright (c) 2018 Jan Skoruba

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.Client;
using IdentityServer.Samples.Shared.IntegrationTests.Tests.Base;
using Xunit;

namespace IdentityServer.Samples.Shared.IntegrationTests.Tests
{
    public abstract class IdentityServerTests<TFixture> : BaseClassFixture<TFixture>
        where TFixture : class
    {
        public IdentityServerTests(TestFixture<TFixture> fixture) : base(fixture)
        {
        }


        [Fact]
        public async Task CanShowDiscoveryEndpoint()
        {
            var disco = await Client.GetDiscoveryDocumentAsync("http://localhost");

            disco.Should().NotBeNull();
            disco.IsError.Should().Be(false);

            disco.KeySet.Keys.Count.Should().Be(1);
        }
    }
}
