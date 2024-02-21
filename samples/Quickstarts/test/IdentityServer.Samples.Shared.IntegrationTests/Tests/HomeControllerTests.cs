/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 

 Copyright (c) 2018 Jan Skoruba

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer.Samples.Shared.IntegrationTests.Tests.Base;
using Xunit;

namespace IdentityServer.Samples.Shared.IntegrationTests.Tests
{
    public abstract class HomeControllerTests<TFixture> : BaseClassFixture<TFixture>
        where TFixture : class
    {
        public HomeControllerTests(TestFixture<TFixture> fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task EveryoneHasAccessToHomepage()
        {
            Client.DefaultRequestHeaders.Clear();

            // Act
            var response = await Client.GetAsync("/home/index");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}