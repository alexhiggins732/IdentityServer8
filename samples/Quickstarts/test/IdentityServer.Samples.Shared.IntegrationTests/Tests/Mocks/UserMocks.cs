/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 

 Copyright (c) 2018 Jan Skoruba

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer.Samples.Shared.IntegrationTests.Common;
using IdentityServer8.Test;

namespace IdentityServer.Samples.Shared.IntegrationTests.Mocks
{
    public static class UserMocks
    {
        public static string UserPassword = "Pa$$word123";
        public static string AntiForgeryTokenKey = "__RequestVerificationToken";

        public static TestUser GenerateDefaultTestUser()
        {
            return new TestUser
            {
                Username = "alice",
                Password = "alice",

            };
        }

        public static TestUser TestUser { get; set; } = GenerateDefaultTestUser();
        public static void SetTestUser(TestUser user) => TestUser = user;

        public static Dictionary<string, string> GenerateRegisterData()
        {
            return new Dictionary<string, string>
            {
                { "UserName", Guid.NewGuid().ToString()},
                { "Password", UserPassword },
                { "ConfirmPassword", UserPassword},
                { "Email", $"{Guid.NewGuid().ToString()}@{Guid.NewGuid().ToString()}.com"}
            };
        }

        public static Dictionary<string, string> GenerateLoginData(string userName, string password, string antiForgeryToken)
        {
            var loginDataForm = new Dictionary<string, string>
            {
                {"Username", userName},
                {"Password", password},
                {"button", "login"},
                {AntiForgeryTokenKey, antiForgeryToken}
            };

            return loginDataForm;
        }
        public static Dictionary<string, string> GenerateLogoutData(string logoutId, string antiForgeryToken)
        {
            var logoutFormData = new Dictionary<string, string>
            {
                {"logoutId", logoutId},
                {AntiForgeryTokenKey, antiForgeryToken}
            };

            return logoutFormData;
        }

        public static Dictionary<string, string> GenerateManageProfileData(string email, string antiForgeryToken)
        {
            var manageData = new Dictionary<string, string>
            {
                { "Name", Guid.NewGuid().ToString()},
                { AntiForgeryTokenKey, antiForgeryToken},
                { "Email", email }
            };

            return manageData;
        }

        public static async Task<HttpResponseMessage> RegisterNewUserAsync(HttpClient client, Dictionary<string, string> registerDataForm)
        {
            const string accountRegisterAction = "/Account/Register";

            var registerResponse = await client.GetAsync(accountRegisterAction);
            var antiForgeryToken = await registerResponse.ExtractAntiForgeryToken();

            registerDataForm.Remove(AntiForgeryTokenKey);
            registerDataForm.Add(AntiForgeryTokenKey, antiForgeryToken);

            var requestMessage = RequestHelper.CreatePostRequestWithCookies(accountRegisterAction, registerDataForm, registerResponse);
            var responseMessage = await client.SendAsync(requestMessage);

            return responseMessage;
        }
    }
}
