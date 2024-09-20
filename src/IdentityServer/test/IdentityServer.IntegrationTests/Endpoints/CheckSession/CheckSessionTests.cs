/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer.IntegrationTests.Common;
using Xunit;

namespace IdentityServer.IntegrationTests.Endpoints.CheckSession;

public class CheckSessionTests
{
    private const string Category = "Check session endpoint";

    private IdentityServerPipeline _mockPipeline = new IdentityServerPipeline();

    public CheckSessionTests()
    {
        _mockPipeline.Initialize();
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task get_request_should_not_return_404()
    {
        var response = await _mockPipeline.BackChannelClient.GetAsync(IdentityServerPipeline.CheckSessionEndpoint);

        response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
    }
}
