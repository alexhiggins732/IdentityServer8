/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using Clients;
using IdentityModel.Client;
using System.Text.Json;

namespace ConsoleCustomGrant
{
    class Program
    {
        static IDiscoveryCache _cache = new DiscoveryCache(Constants.Authority);

        static async Task Main()
        {
            Console.Title = "Console Custom Grant";

            // custom grant type with subject support
            var response = await RequestTokenAsync("custom");
            response.Show();

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);

            Console.ReadLine();

            // custom grant type without subject support
            response = await RequestTokenAsync("custom.nosubject");
            response.Show();

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
        }

        static async Task<TokenResponse> RequestTokenAsync(string grantType)
        {
            var client = new HttpClient();

            var disco = await _cache.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestTokenAsync(new TokenRequest
            {
                Address = disco.TokenEndpoint,
                GrantType = grantType,

                ClientId = "client.custom",
                ClientSecret = "secret",

                Parameters =
                {
                    { "scope", "resource1.scope1" },
                    { "custom_credential", "custom credential"}
                }
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        static async Task CallServiceAsync(string token)
        {
            var baseAddress = Constants.SampleApi;

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.SetBearerToken(token);
            var response = await client.GetStringAsync("identity");

            "\n\nService claims:".ConsoleGreen();
            var json = JsonSerializer.Deserialize<JsonElement>(response);
            Console.WriteLine(json);
        }
    }
}