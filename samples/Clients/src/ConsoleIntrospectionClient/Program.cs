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
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleIntrospectionClient
{
    public class Program
    {
        static IDiscoveryCache _cache = new DiscoveryCache(Constants.Authority);

        static async Task Main()
        {
            Console.Title = "Console Introspection Client";

            var response = await RequestTokenAsync();
            await IntrospectAsync(response.AccessToken);
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var disco = await _cache.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new HttpClient();
            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "roclient.reference",
                ClientSecret = "secret",

                UserName = "bob",
                Password = "bob",
                Scope = "resource1.scope1 resource2.scope1"
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        private static async Task IntrospectAsync(string accessToken)
        {
            var disco = await _cache.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new HttpClient();
            var result = await client.IntrospectTokenAsync(new TokenIntrospectionRequest
            {
                Address = disco.IntrospectionEndpoint,

                ClientId = "api1",
                ClientSecret = "secret",
                Token = accessToken
            });

            if (result.IsError)
            {
                Console.WriteLine(result.Error);
            }
            else
            {
                if (result.IsActive)
                {
                    result.Claims.ToList().ForEach(c => Console.WriteLine("{0}: {1}",
                        c.Type, c.Value));
                }
                else
                {
                    Console.WriteLine("token is not active");
                }
            }
        }
    }
}