Windows Authentication
======================
There are several ways how you can enable Windows authentication in ASP.NET Core (and thus in IdentityServer).

* On Windows using IIS hosting (both in- and out-of process)
* On Windows using HTTP.SYS hosting
* On any platform using the Negotiate authentication handler (added in ASP.NET Core 3.0)

.. Note:: We only have documentation for IIS hosting. If you want to contribute to the docs, please open a PR. thanks!

On Windows using IIS hosting
^^^^^^^^^^^^^^^^^^^^^^^^^^^^
The typical ``CreateDefaultBuilder`` host setup enables support for IIS-based Windows authentication when hosting in IIS.
Make sure that Windows authentication is enabled in ``launchSettings.json`` or your IIS configuration.

The IIS integration layer will configure a Windows authentication handler into DI that can be invoked via the authentication service.
Typically in IdentityServer it is advisable to disable the automatic behavior. 

This is done in ``ConfigureServices`` (details vary depending on in-proc vs out-of-proc hosting)::

    // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
    services.Configure<IISOptions>(iis =>
    {
        iis.AuthenticationDisplayName = "Windows";
        iis.AutomaticAuthentication = false;
    });

    // ..or configures IIS in-proc settings
    services.Configure<IISServerOptions>(iis =>
    {
        iis.AuthenticationDisplayName = "Windows";
        iis.AutomaticAuthentication = false;
    });

You trigger Windows authentication by calling ``ChallengeAsync`` on the ``Windows`` scheme (or if you want to use a constant: ``Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme``).

This will send the ``Www-Authenticate`` header back to the browser which will then re-load the current URL including the Windows identity.
You can tell that Windows authentication was successful, when you call ``AuthenticateAsync`` on the ``Windows`` scheme and the principal returned
is of type ``WindowsPrincipal``.

The principal will have information like user and group SID and the Windows account name. The following snippet shows how to
trigger authentication, and if successful convert the information into a standard ``ClaimsPrincipal`` for the temp-Cookie approach::

    private async Task<IActionResult> ChallengeWindowsAsync(string returnUrl)
    {
        // see if windows auth has already been requested and succeeded
        var result = await HttpContext.AuthenticateAsync("Windows");
        if (result?.Principal is WindowsPrincipal wp)
        {
            // we will issue the external cookie and then redirect the
            // user back to the external callback, in essence, treating windows
            // auth the same as any other external authentication mechanism
            var props = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("Callback"),
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", "Windows" },
                }
            };

            var id = new ClaimsIdentity("Windows");

            // the sid is a good sub value
            id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.FindFirst(ClaimTypes.PrimarySid).Value));

            // the account name is the closest we have to a display name
            id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

            // add the groups as claims -- be careful if the number of groups is too large
            var wi = wp.Identity as WindowsIdentity;

            // translate group SIDs to display names
            var groups = wi.Groups.Translate(typeof(NTAccount));
            var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
            id.AddClaims(roles);
            

            await HttpContext.SignInAsync(
                IdentityServerConstants.ExternalCookieAuthenticationScheme,
                new ClaimsPrincipal(id),
                props);
            return Redirect(props.RedirectUri);
        }
        else
        {
            // trigger windows auth
            // since windows auth don't support the redirect uri,
            // this URL is re-triggered when we call challenge
            return Challenge("Windows");
        }
    }
