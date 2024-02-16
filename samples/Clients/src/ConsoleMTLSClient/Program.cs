/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/


Console.Title = "Console mTLS Client";
var response = await RequestTokenAsync();
response.Show();

Console.ReadLine();
await CallServiceAsync(response.AccessToken);
async Task<TokenResponse> RequestTokenAsync()
{
    var client = new HttpClient(GetHandler());

    var disco = await client.GetDiscoveryDocumentAsync("https://identityserver.local");
    if (disco.IsError) throw new Exception(disco.Error);

    var endpoint = disco.TokenEndpoint;
    //.TryGetValue(OidcConstants.Discovery.MtlsEndpointAliases)
    //.Value<string>(OidcConstants.Discovery.TokenEndpoint)
    //.ToString();

    var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
    {
        Address = endpoint,

        ClientId = "mtls",
        Scope = "resource1.scope1"
    });

    if (response.IsError) throw new Exception(response.Error);
    return response;
}

async Task CallServiceAsync(string token)
{
    var client = new HttpClient(GetHandler())
    {
        BaseAddress = new Uri(Constants.SampleApi)
    };

    client.SetBearerToken(token);
    var response = await client.GetStringAsync("identity");

    "\n\nService claims:".ConsoleGreen();
    var json = JsonSerializer.Deserialize<JsonElement>(response);
    Console.WriteLine(json);
}

SocketsHttpHandler GetHandler()
{
    var handler = new SocketsHttpHandler();

    var cert = new X509Certificate2("client.p12", "changeit");
    handler.SslOptions.ClientCertificates = new X509CertificateCollection { cert };

    return handler;
}
