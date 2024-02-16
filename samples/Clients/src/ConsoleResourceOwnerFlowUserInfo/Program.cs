/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/


HttpClient tokenClient = new HttpClient();
DiscoveryCache cache = new DiscoveryCache(Constants.Authority);


Console.Title = "Console ResourceOwner Flow UserInfo";

var response = await RequestTokenAsync();
response.Show();

await GetClaimsAsync(response.AccessToken);


async Task<TokenResponse> RequestTokenAsync()
{
    var disco = await cache.GetAsync();
    if (disco.IsError) throw new Exception(disco.Error);

    var response = await tokenClient.RequestPasswordTokenAsync(new PasswordTokenRequest
    {
        Address = disco.TokenEndpoint,

        ClientId = "roclient",
        ClientSecret = "secret",

        UserName = "bob",
        Password = "bob",

        Scope = "openid custom.profile"
    });

    if (response.IsError) throw new Exception(response.Error);
    return response;
}

async Task GetClaimsAsync(string token)
{
    var disco = await cache.GetAsync();
    if (disco.IsError) throw new Exception(disco.Error);

    var response = await tokenClient.GetUserInfoAsync(new UserInfoRequest
    {
        Address = disco.UserInfoEndpoint,
        Token = token
    });

    if (response.IsError) throw new Exception(response.Error);

    "\n\nUser claims:".ConsoleGreen();
    foreach (var claim in response.Claims)
    {
        Console.WriteLine("{0}\n {1}", claim.Type, claim.Value);
    }
}