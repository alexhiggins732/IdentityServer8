/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

OidcClient oidcClient = null!;
HttpClient apiClient = new HttpClient { BaseAddress = new Uri(Constants.SampleApi) };


Console.WriteLine("+-----------------------+");
Console.WriteLine("|  Sign in with OIDC    |");
Console.WriteLine("+-----------------------+");
Console.WriteLine("");
Console.WriteLine("Press any key to sign in...");
Console.ReadKey();

await SignIn();

 async Task SignIn()
{
    // create a redirect URI using an available port on the loopback address.
    // requires the OP to allow random ports on 127.0.0.1 - otherwise set a static port
    var browser = new SystemBrowser();
    string redirectUri = string.Format($"http://127.0.0.1:{browser.Port}");

    var options = new OidcClientOptions
    {
        Authority = Constants.Authority,

        ClientId = "console.pkce",

        RedirectUri = redirectUri,
        Scope = "openid profile resource1.scope1",
        FilterClaims = false,
        Browser = browser
    };

    var serilog = new LoggerConfiguration()
        .MinimumLevel.Error()
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message}{NewLine}{Exception}{NewLine}")
        .CreateLogger();

    options.LoggerFactory.AddSerilog(serilog);

    oidcClient = new OidcClient(options);
    var result = await oidcClient.LoginAsync(new LoginRequest());

    ShowResult(result);
    await NextSteps(result);
}

void ShowResult(LoginResult result)
{
    if (result.IsError)
    {
        Console.WriteLine("\n\nError:\n{0}", result.Error);
        return;
    }

    Console.WriteLine("\n\nClaims:");
    foreach (var claim in result.User.Claims)
    {
        Console.WriteLine("{0}: {1}", claim.Type, claim.Value);
    }

    Console.WriteLine($"\nidentity token: {result.IdentityToken}");
    Console.WriteLine($"access token:   {result.AccessToken}");
    Console.WriteLine($"refresh token:  {result?.RefreshToken ?? "none"}");
}

async Task NextSteps(LoginResult result)
{
    var currentAccessToken = result.AccessToken;
    var currentRefreshToken = result.RefreshToken;

    var menu = "  x...exit  c...call api   ";
    if (currentRefreshToken != null) menu += "r...refresh token   ";

    while (true)
    {
        Console.WriteLine("\n\n");

        Console.Write(menu);
        var key = Console.ReadKey();

        if (key.Key == ConsoleKey.X) return;
        if (key.Key == ConsoleKey.C) await CallApi(currentAccessToken);
        if (key.Key == ConsoleKey.R)
        {
            var refreshResult = await oidcClient.RefreshTokenAsync(currentRefreshToken);
            if (result.IsError)
            {
                Console.WriteLine($"Error: {refreshResult.Error}");
            }
            else
            {
                currentRefreshToken = refreshResult.RefreshToken;
                currentAccessToken = refreshResult.AccessToken;

                Console.WriteLine("\n\n");
                Console.WriteLine($"access token:   {result.AccessToken}");
                Console.WriteLine($"refresh token:  {result?.RefreshToken ?? "none"}");
            }
        }
    }
}

async Task CallApi(string currentAccessToken)
{
    apiClient.SetBearerToken(currentAccessToken);
    var response = await apiClient.GetAsync("identity");

    if (response.IsSuccessStatusCode)
    {
        var json = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
        Console.WriteLine("\n\n");
        Console.WriteLine(json);
    }
    else
    {
        Console.WriteLine($"Error: {response.ReasonPhrase}");
    }
}
