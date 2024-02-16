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
    
    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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

    public IActionResult Logout()
    {
        return new SignOutResult(new[] { "Cookies", "oidc" });
    }

    public IActionResult Error()
    {
        return View();
    }
}
