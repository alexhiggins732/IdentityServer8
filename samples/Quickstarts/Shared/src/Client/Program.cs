/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityModel.Client;
using System.Diagnostics;
using System.Text.Json;

// discover endpoints from metadata
using HttpClient client = new();

var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
    Exit(disco.Error);

// request token
var tokenResponse = await client.RequestClientCredentialsTokenAsync(
    new()
{
    Address = disco.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",

    Scope = "api1"
});

if (tokenResponse.IsError)
    Exit(tokenResponse.Error);

Console.WriteLine(tokenResponse.Json);
Console.WriteLine("\n\n");

// call api
client.SetBearerToken(tokenResponse.AccessToken);

var response = await client.GetAsync("https://localhost:6001/identity");
if (!response.IsSuccessStatusCode)
    Console.WriteLine(response.StatusCode);
else
{
    var responseJson = await response.Content.ReadAsStringAsync();
    var obj = JsonSerializer.Deserialize<JsonElement>(responseJson);
    Console.WriteLine(obj);
}

Exit("Done!", 0);

void Exit(string message, int exitCode = 1)
{
    Console.WriteLine(message);
    if (Debugger.IsAttached)
        Debugger.Break();
    Environment.Exit(exitCode);
}
