/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 

 Copyright (c) 2018 Jan Skoruba

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using FluentAssertions;
using HtmlAgilityPack;
using IdentityServer.Samples.Identity.IntegrationTests.Common;
using IdentityServer.Samples.Identity.IntegrationTests.Mocks;
using IdentityServer.Samples.Identity.IntegrationTests.Tests.Base;
using Xunit;

namespace IdentityServer.Samples.Identity.IntegrationTests.Tests
{
    public class AccountControllerTests : BaseClassFixture
    {
        public AccountControllerTests(TestFixture fixture) : base(fixture)
        {
        }


        [Fact]
        public async Task UserIsNotAbleToRegister()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            // Register new user
            var registerFormData = UserMocks.GenerateRegisterData();
            var registerResponse = await UserMocks.RegisterNewUserAsync(Client, registerFormData);

            registerResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact(Skip = "Base Identity Server Does Not Have Regisration Enabled")]
        public async Task UserIsNotAbleToRegisterWithSameUserName()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            // Register new user
            var registerFormData = UserMocks.GenerateRegisterData();

            var registerResponseFirst = await UserMocks.RegisterNewUserAsync(Client, registerFormData);

            // Assert      
            registerResponseFirst.StatusCode.Should().Be(HttpStatusCode.Redirect);

            //The redirect to login
            registerResponseFirst.Headers.Location.ToString().Should().Be("/");

            var registerResponseSecond = await UserMocks.RegisterNewUserAsync(Client, registerFormData);

            // Assert response
            registerResponseSecond.StatusCode.Should().Be(HttpStatusCode.OK);

            // Get html content
            var contentWithErrorMessage = await registerResponseSecond.Content.ReadAsStringAsync();

            // From String
            var doc = new HtmlDocument();
            doc.LoadHtml(contentWithErrorMessage);

            // Get error messages from validation summary
            var errorNodes = doc.DocumentNode
                .SelectNodes("//div[contains(@class, 'validation-summary-errors')]/ul/li");

            errorNodes.Should().HaveCount(2);

            // Build expected error messages
            var expectedErrorMessages = new List<string>
            {
                $"Username &#x27;{registerFormData["UserName"]}&#x27; is already taken.",
                $"Email &#x27;{registerFormData["Email"]}&#x27; is already taken."
            };

            // Assert
            var containErrors = errorNodes.Select(x => x.InnerText).ToList().SequenceEqual(expectedErrorMessages);

            containErrors.Should().BeTrue();
        }

        [Fact]
        public async Task UserIsAbleToLoginAndLogout()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            const string accountLoginAction = "/Account/Login";
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
            // Assert status code    
            responseMessage.StatusCode.Should().Be(HttpStatusCode.Redirect);
            responseBody.Should().Be("");
            // Assert redirect location
            responseMessage.Headers.Location.ToString().Should().Be("/");

            // Check if response contain cookie with Identity
            const string identityCookieName = "idsrv";
            var existsCookie = CookiesHelper.ExistsCookie(responseMessage, identityCookieName);

            // Assert Identity cookie
            existsCookie.Should().BeTrue();

            var logoutUrl = "/Account/Logout";
            Client.DefaultRequestHeaders.Clear();
            Client.PutCookiesOnRequest(responseMessage);
            var logoutResponse = await Client.GetAsync(logoutUrl);
            logoutResponse.EnsureSuccessStatusCode();

            var content = await logoutResponse.Content.ReadAsStringAsync();
            content.Should().Contain("Would you like to logout");

            Client.DefaultRequestHeaders.Clear();

            var antiForgeryTokenLogout = await logoutResponse.ExtractAntiForgeryToken();
            var logoutFormData = UserMocks.GenerateLogoutData(null, antiForgeryTokenLogout);
            //logoutFormData.Add("__RequestVerificationToken", value);
            var logoutRequestMessage = RequestHelper.CreatePostRequestWithCookies(logoutUrl, logoutFormData, logoutResponse);
            var doLogoutResponse = await Client.SendAsync(logoutRequestMessage);
            var logoutContent = await doLogoutResponse.Content.ReadAsStringAsync();
            logoutContent.Should().Contain("You are now logged out");
            existsCookie = CookiesHelper.ExistsCookie(responseMessage, identityCookieName);
            existsCookie.Should().BeTrue();
        }

        [Fact]
        public async Task CanGetLocalLoginRedirect()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            const string accountLoginAction = "/Account/Login";
            string loginUrl = accountLoginAction + "?returnUrl=~/";
            var loginResponse = await Client.GetAsync(loginUrl);
            loginResponse.EnsureSuccessStatusCode();


            var content = await loginResponse.Content.ReadAsStringAsync();

            // Clear headers
            Client.DefaultRequestHeaders.Clear();
            var antiForgeryToken = await loginResponse.ExtractAntiForgeryToken();
            // Prepare request to login
            var loginDataForm = UserMocks.GenerateLoginData("alice", "alice", antiForgeryToken);


            // Login
            var requestMessage = RequestHelper.CreatePostRequestWithCookies(loginUrl, loginDataForm, loginResponse);
            var responseMessage = await Client.SendAsync(requestMessage);
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            // Assert status code    
            responseMessage.StatusCode.Should().Be(HttpStatusCode.Redirect);
            responseBody.Should().Be("");


        }

        [Fact]
        public async Task CanNotGetArbirtraryExternalLoginRedirect()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            const string accountLoginAction = "/Account/Login";
            string loginUrl = accountLoginAction + "?returnUrl=https://restricted.domainname.com";
            var loginResponse = await Client.GetAsync(loginUrl);
            loginResponse.EnsureSuccessStatusCode();
            var content = await loginResponse.Content.ReadAsStringAsync();

            // Clear headers
            Client.DefaultRequestHeaders.Clear();
            var antiForgeryToken = await loginResponse.ExtractAntiForgeryToken();
            // Prepare request to login
            var loginDataForm = UserMocks.GenerateLoginData("alice", "alice", antiForgeryToken);

            // Login
            var requestMessage = RequestHelper.CreatePostRequestWithCookies(loginUrl, loginDataForm, loginResponse);
            var response = await Client.SendAsync(requestMessage);
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        }
        [Fact]
        public async Task AnymousUserCanGetAccessDenied()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();
            const string accountLoginAction = "/Account/AccessDenied";
            var responseMessage = await Client.GetAsync(accountLoginAction);
            responseMessage.EnsureSuccessStatusCode();

            var contentWithErrorMessage = await responseMessage.Content.ReadAsStringAsync();
            contentWithErrorMessage.Should().Contain("Access Denied");
        }
        [Fact]
        public async Task UserIsNotAbleToLoginWithIncorrectPassword()
        {
            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            // Register new user
            var registerFormData = UserMocks.GenerateRegisterData();

            // Clear headers
            Client.DefaultRequestHeaders.Clear();

            // Prepare request to login
            const string accountLoginAction = "/Account/Login";
            var loginResponse = await Client.GetAsync(accountLoginAction);
            var antiForgeryToken = await loginResponse.ExtractAntiForgeryToken();

            // User Guid like fake password
            var loginDataForm = UserMocks.GenerateLoginData(registerFormData["UserName"], Guid.NewGuid().ToString(), antiForgeryToken);

            // Login
            var requestMessage = RequestHelper.CreatePostRequestWithCookies(accountLoginAction, loginDataForm, loginResponse);
            var responseMessage = await Client.SendAsync(requestMessage);

            // Get html content
            var contentWithErrorMessage = await responseMessage.Content.ReadAsStringAsync();

            // Assert status code    
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            // From String
            var doc = new HtmlDocument();
            doc.LoadHtml(contentWithErrorMessage);

            // Get error messages from validation summary
            var errorNodes = doc.DocumentNode
                .SelectNodes("//div[contains(@class, 'validation-summary-errors')]/ul/li");

            errorNodes.Should().HaveCount(1);

            // Build expected error messages
            var expectedErrorMessages = new List<string>
            {
                "Invalid username or password"
            };

            // Assert
            var containErrors = errorNodes.Select(x => x.InnerText).ToList().SequenceEqual(expectedErrorMessages);

            containErrors.Should().BeTrue();

            // Check if response contain cookie with Identity
            const string identityCookieName = "idsvr";
            var existsCookie = CookiesHelper.ExistsCookie(responseMessage, identityCookieName);

            // Assert Identity cookie
            existsCookie.Should().BeFalse();
        }
    }
}
