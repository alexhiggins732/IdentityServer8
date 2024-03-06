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
    public class ManageControllerTests : BaseClassFixture
    {
        public ManageControllerTests(TestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task ManageDoesNotExistInBaseIdentityServer()
        {
            // Act
            var response = await Client.GetAsync("/Manage");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact(Skip = "Base Identity Server Does Not Have Manage Contoller")]
        public async Task AuthorizeUserCanAccessManageViews()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            // Register new user
            var registerFormData = UserMocks.GenerateRegisterData();
            var registerResponse = await UserMocks.RegisterNewUserAsync(Client,registerFormData);

            // Get cookie with user identity for next request
            Client.PutCookiesOnRequest(registerResponse);
            
            foreach (var route in RoutesConstants.GetManageRoutes())
            {
                // Act
                var response = await Client.GetAsync($"/Manage/{route}");

                // Assert
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }


        [Fact(Skip = "Base Identity Server Does Not Have Manage Contoller")]
        public async Task UnAuthorizeUserCannotAccessManageViews()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            foreach (var route in RoutesConstants.GetManageRoutes())
            {
                // Act
                var response = await Client.GetAsync($"/Manage/{route}");

                // Assert      
                response.StatusCode.Should().Be(HttpStatusCode.Redirect);

                //The redirect to login
                response.Headers.Location.ToString().Should().Contain("Account/Login");
            }
        }


        [Fact(Skip = "Base Identity Server Does Not Have Manage Contoller")]
        public async Task UserIsAbleToUpdateProfile()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            // Register new user
            var registerFormData = UserMocks.GenerateRegisterData();
            var registerResponse = await UserMocks.RegisterNewUserAsync(Client, registerFormData);

            // Get cookie with user identity for next request
            Client.PutCookiesOnRequest(registerResponse);

            // Prepare request to update profile
            const string manageAction = "/Manage/Index";
            var manageResponse = await Client.GetAsync(manageAction);
            var antiForgeryToken = await manageResponse.ExtractAntiForgeryToken();

            var manageProfileData = UserMocks.GenerateManageProfileData(registerFormData["Email"], antiForgeryToken);

            // Update profile
            var requestWithAntiForgeryCookie = RequestHelper.CreatePostRequestWithCookies(manageAction, manageProfileData, manageResponse);
            var requestWithIdentityCookie = CookiesHelper.CopyCookiesFromResponse(requestWithAntiForgeryCookie, registerResponse);
            var responseMessage = await Client.SendAsync(requestWithIdentityCookie);

            // Assert      
            responseMessage.StatusCode.Should().Be(HttpStatusCode.Redirect);

            //The redirect to login
            responseMessage.Headers.Location.ToString().Should().Be("/Manage");
        }
    }
}
