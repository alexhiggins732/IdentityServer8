/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDiscoveryCache _discoveryCache;

    public HomeController(IHttpClientFactory httpClientFactory, IDiscoveryCache discoveryCache)
    {
        _httpClientFactory = httpClientFactory;
        _discoveryCache = discoveryCache;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Secure()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> CallApi()
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var client = _httpClientFactory.CreateClient();
        client.SetBearerToken(token);

        var response = await client.GetStringAsync(Constants.SampleApi + "identity");
        ViewBag.Json = JsonSerializer.Deserialize<JsonElement>(response).ToString();

        return View();
    }

    public async Task<IActionResult> RenewTokens()
    {
        var disco = await _discoveryCache.GetAsync();
        if (disco.IsError) throw new Exception(disco.Error);

        var rt = await HttpContext.GetTokenAsync("refresh_token");
        var tokenClient = _httpClientFactory.CreateClient();

        var tokenResult = await tokenClient.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = disco.TokenEndpoint,

            ClientId = "mvc.hybrid.backchannel",
            ClientSecret = "secret",
            RefreshToken = rt
        });

        if (!tokenResult.IsError)
        {
            var old_id_token = await HttpContext.GetTokenAsync("id_token");
            var new_access_token = tokenResult.AccessToken;
            var new_refresh_token = tokenResult.RefreshToken;
            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);

            var info = await HttpContext.AuthenticateAsync("Cookies");

            info.Properties.UpdateTokenValue("refresh_token", new_refresh_token);
            info.Properties.UpdateTokenValue("access_token", new_access_token);
            info.Properties.UpdateTokenValue("expires_at", expiresAt.ToString("o", CultureInfo.InvariantCulture));

            await HttpContext.SignInAsync("Cookies", info.Principal, info.Properties);
            return Redirect("~/Home/Secure");
        }

        ViewData["Error"] = tokenResult.Error;
        return View("Error");
    }

    public IActionResult Logout()
    {
        return new SignOutResult(new[] { "Cookies", "oidc" });
    }

    public IActionResult Error()
    {
        return View();
    }
}
