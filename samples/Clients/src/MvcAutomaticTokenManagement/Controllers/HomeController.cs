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

    [AllowAnonymous]
    public IActionResult Index() => View();

    public IActionResult Secure() => View();

    public IActionResult Logout() => SignOut("oidc");

    public async Task<IActionResult> CallApi()
    {
        var client = _httpClientFactory.CreateClient("client");

        var response = await client.GetStringAsync("identity");
        ViewBag.Json = JsonSerializer.Deserialize<JsonElement>(response).ToString();

        return View();
    }
}
