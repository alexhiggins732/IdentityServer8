/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServerHost.Quickstart.UI;

[SecurityHeaders]
[Authorize]
public class DiagnosticsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // allow only local request from local host and AspNetCore test host which will not have a local IP address or Remote IP address
        var localAddresses = new string[] { "127.0.0.1", "::1", HttpContext.Connection.LocalIpAddress?.ToString() ?? "localhost" };
        if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "localhost"))
        {
            return NotFound();
        }

        var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
        return View(model);
    }
}
