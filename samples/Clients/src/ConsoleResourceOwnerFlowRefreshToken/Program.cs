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


Console.Title = "Console ResourceOwner Flow RefreshToken";

var response = await RequestTokenAsync();
response.Show();

Console.ReadLine();

var refresh_token = response.RefreshToken;

while (true)
{
    response = await RefreshTokenAsync(refresh_token);
    ShowResponse(response);

    Console.ReadLine();
    await CallServiceAsync(response.AccessToken);

    if (response.RefreshToken != refresh_token)
    {
        refresh_token = response.RefreshToken;
    }
}
async Task<TokenResponse> RequestTokenAsync()
{
    var disco = await cache.GetAsync();

    var response = await tokenClient.RequestPasswordTokenAsync(new PasswordTokenRequest
    {
        Address = disco.TokenEndpoint,

        ClientId = "roclient",
        ClientSecret = "secret",

        UserName = "bob",
        Password = "bob",

        Scope = "resource1.scope1 offline_access",
    });

    if (response.IsError) throw new Exception(response.Error);
    return response;
}
 async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
{
    Console.WriteLine("Using refresh token: {0}", refreshToken);

    var disco = await cache.GetAsync();
    var response = await tokenClient.RequestRefreshTokenAsync(new RefreshTokenRequest
    {
        Address = disco.TokenEndpoint,

        ClientId = "roclient",
        ClientSecret = "secret",
        RefreshToken = refreshToken
    });

    if (response.IsError) throw new Exception(response.Error);
    return response;
}

async Task CallServiceAsync(string token)
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

static void ShowResponse(TokenResponse response)
{
    if (!response.IsError)
    {
        "Token response:".ConsoleGreen();
        Console.WriteLine(response.Json);

        if (response.AccessToken.Contains("."))
        {
            "\nAccess Token (decoded):".ConsoleGreen();

            var parts = response.AccessToken.Split('.');
            var header = parts[0];
            var claims = parts[1];

            var headerJson = Encoding.UTF8.GetString(Base64Url.Decode(header));
            Console.WriteLine(JsonSerializer.Deserialize<JsonElement>(header));
            var claimsJson = Encoding.UTF8.GetString(Base64Url.Decode(claims));
            Console.WriteLine(JsonSerializer.Deserialize<JsonElement>(claims));
        }
    }
    else
    {
        if (response.ErrorType == ResponseErrorType.Http)
        {
            "HTTP error: ".ConsoleGreen();
            Console.WriteLine(response.Error);
            "HTTP status code: ".ConsoleGreen();
            Console.WriteLine(response.HttpStatusCode);
        }
        else
        {
            "Protocol error response:".ConsoleGreen();
            Console.WriteLine(response.Json);
        }
    }
}
