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
using IdentityServer8.STS.Identity.IntegrationTests.Common;
using IdentityServer8.STS.Identity.IntegrationTests.Mocks;
using IdentityServer8.STS.Identity.IntegrationTests.Tests.Base;
using Xunit;

namespace IdentityServer8.STS.Identity.IntegrationTests.Tests
{
    public class DiagnosticsControllerTests : BaseClassFixture
    {
        public DiagnosticsControllerTests(TestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task UnAuthorizeUserCannotAccessDiagnosticsView()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            // Act
            var response = await Client.GetAsync("/Diagnostics/Index");

            // Assert      
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);

            //The redirect to login
            response.Headers.Location.ToString().Should().Contain("Account/Login");
        }

        [Fact]
        public async Task AuthorizedUserCanAccessDiagnosticsView()
        {
            Client.DefaultRequestHeaders.Clear();
            const string accountLoginAction = "/Account/Login";
            var loginUrl = accountLoginAction + "?ReturnUrl=%2FDiagnostics%2FIndex";
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

            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            // Act
            Client.PutCookiesOnRequest(responseMessage);
            var response = await Client.GetAsync("/Diagnostics/Index");


            // Assert      
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //The redirect to login
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Claims");
        }
    }
}
