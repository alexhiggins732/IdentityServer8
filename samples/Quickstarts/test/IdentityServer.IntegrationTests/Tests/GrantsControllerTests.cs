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
using IdentityServer.Samples.Identity.IntegrationTests.Common;
using IdentityServer.Samples.Identity.IntegrationTests.Mocks;
using IdentityServer.Samples.Identity.IntegrationTests.Tests.Base;
using Xunit;

namespace IdentityServer.Samples.Identity.IntegrationTests.Tests
{
    public class GrantsControllerTests : BaseClassFixture
    {
        public GrantsControllerTests(TestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task AuthorizeUserCanAccessGrantsView()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            const string accountLoginAction = "/Account/Login";
            //var loginUrl = accountLoginAction + "?ReturnUrl=%2FGrants%2FIndex";
            var loginResponse = await Client.GetAsync(accountLoginAction);
            loginResponse.EnsureSuccessStatusCode();

            // Clear headers
            Client.DefaultRequestHeaders.Clear();
            var antiForgeryToken = await loginResponse.ExtractAntiForgeryToken();
            // Prepare request to login
            var loginDataForm = UserMocks.GenerateLoginData("alice", "alice", antiForgeryToken);

            // Login
            var requestMessage = RequestHelper.CreatePostRequestWithCookies(accountLoginAction, loginDataForm, loginResponse);
            var responseMessage = await Client.SendAsync(requestMessage);
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            // Get cookie with user identity for next request
            Client.PutCookiesOnRequest(responseMessage);

            // Act
            var response = await Client.GetAsync("/Grants/Index");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UnAuthorizeUserCannotAccessGrantsView()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            // Act
            var response = await Client.GetAsync("/Grants/Index");

            // Assert      
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);

            //The redirect to login
            response.Headers.Location.ToString().Should().Contain("Account/Login");
        }
    }
}
