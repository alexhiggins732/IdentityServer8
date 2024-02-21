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
using IdentityServer.Samples.Shared.IntegrationTests.Common;
using IdentityServer.Samples.Shared.IntegrationTests.Mocks;
using IdentityServer.Samples.Shared.IntegrationTests.Tests.Base;
using IdentityServer8.Test;
using Xunit;

namespace IdentityServer.Samples.Shared.IntegrationTests
{
    public abstract partial class ControllerTests<TFixture> : BaseClassFixture<TFixture>
        where TFixture : class
    {
        [Fact]
        public async Task AuthorizeUserCanAccessGrantsView()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            const string accountLoginAction = "/Account/Login";
            var loginUrl = accountLoginAction + "?ReturnUrl=%2FGrants%2FIndex";
            var loginResponse = await Client.GetAsync(accountLoginAction);
            loginResponse.EnsureSuccessStatusCode();

            // Clear headers
            Client.DefaultRequestHeaders.Clear();
            var antiForgeryToken = await loginResponse.ExtractAntiForgeryToken();
            // Prepare request to login
            var testUser = UserMocks.TestUser;
            var loginDataForm = UserMocks.GenerateLoginData(testUser.Username, testUser.Password, antiForgeryToken);

            // Login
            var requestMessage = RequestHelper.CreatePostRequestWithCookies(accountLoginAction, loginDataForm, loginResponse);
            var responseMessage = await Client.SendAsync(requestMessage);
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.Redirect);
           
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
