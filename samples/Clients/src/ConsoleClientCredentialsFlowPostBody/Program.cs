/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

Console.Title = "Console ClientCredentials Flow PostBody";

var response = await RequestTokenAsync();
response.Show();

Console.ReadLine();
await CallServiceAsync(response.AccessToken);


async Task<TokenResponse> RequestTokenAsync()
{
    var client = new HttpClient();

    var disco = await client.GetDiscoveryDocumentAsync(Constants.Authority);
    if (disco.IsError) throw new Exception(disco.Error);

    var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
    {
        Address = disco.TokenEndpoint,
        ClientCredentialStyle = ClientCredentialStyle.PostBody,

        ClientId = "client",
        ClientSecret = "secret",
        Scope = "resource1.scope1"
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
    var obj = JsonSerializer.Deserialize<JsonElement>(response);
    Console.WriteLine(obj);
}