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
using Microsoft.Net.Http.Headers;

namespace IdentityServer8.STS.Identity.IntegrationTests.Common
{
    public static class CookiesHelper
    {
        public static void PutCookiesOnRequest(this HttpClient client, HttpResponseMessage message)
        {
            var cookies = ExtractCookiesFromResponse(message);

            cookies.Keys.ToList().ForEach(key =>
            {
                client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(key, cookies[key]).ToString());
            });
        }

        public static IDictionary<string, string> ExtractCookiesFromResponse(HttpResponseMessage response)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                SetCookieHeaderValue.ParseList(values.ToList()).ToList().ForEach(cookie =>
                {
                    result.Add(cookie.Name.Value, cookie.Value.Value);
                });
            }

            return result;
        }

        public static HttpRequestMessage PutCookiesOnRequest(HttpRequestMessage request, IDictionary<string, string> cookies)
        {
            cookies.Keys.ToList().ForEach(key =>
            {
                request.Headers.Add("Cookie", new CookieHeaderValue(key, cookies[key]).ToString());
            });

            return request;
        }

        public static HttpRequestMessage CopyCookiesFromResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            return PutCookiesOnRequest(request, ExtractCookiesFromResponse(response));
        }

        public static bool ExistsCookie(HttpResponseMessage responseMessage, string cookieName)
        {
            var existsCookie = false;
            const string cookieHeader = "Set-Cookie";

            if (responseMessage.Headers.TryGetValues(cookieHeader, out var cookies))
            {
                existsCookie = cookies.Any(x => x.Contains(cookieName));
            }

            return existsCookie;
        }
    }
}