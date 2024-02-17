/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

IDiscoveryCache cache = new DiscoveryCache(Constants.Authority);

Console.Title = "Console Device Flow";

var authorizeResponse = await RequestAuthorizationAsync();

var tokenResponse = await RequestTokenAsync(authorizeResponse);
tokenResponse.Show();

Console.ReadLine();
await CallServiceAsync(tokenResponse.AccessToken);


async Task<DeviceAuthorizationResponse> RequestAuthorizationAsync()
{
    var disco = await cache.GetAsync();
    if (disco.IsError) throw new Exception(disco.Error);

    var client = new HttpClient();
    var response = await client.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
    {
        Address = disco.DeviceAuthorizationEndpoint,
        ClientId = "device"
    });

    if (response.IsError) throw new Exception(response.Error);

    Console.WriteLine($"user code   : {response.UserCode}");
    Console.WriteLine($"device code : {response.DeviceCode}");
    Console.WriteLine($"URL         : {response.VerificationUri}");
    Console.WriteLine($"Complete URL: {response.VerificationUriComplete}");

    Console.WriteLine($"\nPress enter to launch browser ({response.VerificationUri})");
    Console.ReadLine();

    Process.Start(new ProcessStartInfo(response.VerificationUriComplete) { UseShellExecute = true });
    return response;
}

async Task<TokenResponse> RequestTokenAsync(DeviceAuthorizationResponse authorizeResponse)
{
    var disco = await cache.GetAsync();
    if (disco.IsError) throw new Exception(disco.Error);

    var client = new HttpClient();

    while (true)
    {
        var response = await client.RequestDeviceTokenAsync(new DeviceTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "device",
            DeviceCode = authorizeResponse.DeviceCode
        });

        if (response.IsError)
        {
            if (response.Error == OidcConstants.TokenErrors.AuthorizationPending || response.Error == OidcConstants.TokenErrors.SlowDown)
            {
                Console.WriteLine($"{response.Error}...waiting.");
                Thread.Sleep(authorizeResponse.Interval * 1000);
            }
            else
            {
                throw new Exception(response.Error);
            }
        }
        else
        {
            return response;
        }
    }
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
