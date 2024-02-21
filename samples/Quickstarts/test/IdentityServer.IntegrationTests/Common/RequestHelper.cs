/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 

 Copyright (c) 2018 Jan Skoruba

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace IdentityServer8.STS.Identity.IntegrationTests.Common
{
    public class RequestHelper
    {
        /// <summary>
        /// Create post request
        /// </summary>
        /// <param name="path"></param>
        /// <param name="formPostBodyData"></param>
        /// <returns></returns>
        public static HttpRequestMessage CreatePostRequest(string path, Dictionary<string, string> formPostBodyData)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = new FormUrlEncodedContent(ToFormPostData(formPostBodyData))
            };

            return httpRequestMessage;
        }

        /// <summary>
        /// Prepare form data
        /// </summary>
        /// <param name="formPostBodyData"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> ToFormPostData(Dictionary<string, string> formPostBodyData)
        {
            var result = new List<KeyValuePair<string, string>>();

            formPostBodyData.Keys.ToList().ForEach(key =>
            {
                result.Add(new KeyValuePair<string, string>(key, formPostBodyData[key]));
            });

            return result;
        }

        /// <summary>
        /// Create post request with cookies from response
        /// </summary>
        /// <param name="path"></param>
        /// <param name="formPostBodyData"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static HttpRequestMessage CreatePostRequestWithCookies(string path, Dictionary<string, string> formPostBodyData,
            HttpResponseMessage response)
        {
            var httpRequestMessage = CreatePostRequest(path, formPostBodyData);

            return CookiesHelper.CopyCookiesFromResponse(httpRequestMessage, response);
        }
    }
}