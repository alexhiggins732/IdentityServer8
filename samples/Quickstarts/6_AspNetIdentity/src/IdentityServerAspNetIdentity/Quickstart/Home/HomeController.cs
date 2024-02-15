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
[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger _logger;

    public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger)
    {
        _interaction = interaction;
        _environment = environment;
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (_environment.IsDevelopment())
        {
            // only show in development
            return View();
        }

        _logger.LogInformation("Homepage is disabled in production. Returning 404.");
        return NotFound();
    }

    /// <summary>
    /// Shows the error page
    /// </summary>
    public async Task<IActionResult> Error(string errorId)
    {
        var vm = new ErrorViewModel();

        // retrieve error details from identityserver
        var message = await _interaction.GetErrorContextAsync(errorId);
        if (message != null)
        {
            vm.Error = message;

            if (!_environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }
        }

        return View("Error", vm);
    }
}
