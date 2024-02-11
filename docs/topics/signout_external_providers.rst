.. _refSignOutExternal:
Sign-out of External Identity Providers
=======================================

When a user is :ref:`signing-out <refSignOut>` of IdentityServer, and they have used an :ref:`external identity provider <refExternalIdentityProviders>` to sign-in then it is likely that they should be redirected to also sign-out of the external provider.
Not all external providers support sign-out, as it depends on the protocol and features they support.

To detect that a user must be redirected to an external identity provider for sign-out is typically done by using a ``idp`` claim issued into the cookie at IdentityServer.
The value set into this claim is the ``AuthenticationScheme`` of the corresponding authentication middleware.
At sign-out time this claim is consulted to know if an external sign-out is required.

Redirecting the user to an external identity provider is problematic due to the cleanup and state management already required by the normal sign-out workflow.
The only way to then complete the normal sign-out and cleanup process at IdentityServer is to then request from the external identity provider that after its logout that the user be redirected back to IdentityServer.
Not all external providers support post-logout redirects, as it depends on the protocol and features they support.

The workflow at sign-out is then to revoke IdentityServer's authentication cookie, and then redirect to the external provider requesting a post-logout redirect.
The post-logout redirect should maintain the necessary sign-out state described :ref:`here <refSignOut>` (i.e. the ``logoutId`` parameter value).
To redirect back to IdentityServer after the external provider sign-out, the ``RedirectUri`` should be used on the ``AuthenticationProperties`` when using ASP.NET Core's ``SignOutAsync`` API, for example::

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutInputModel model)
    {
        // build a model so the logged out page knows what to display
        var vm = await _account.BuildLoggedOutViewModelAsync(model.LogoutId);

        var user = HttpContext.User;
        if (user?.Identity.IsAuthenticated == true)
        {
            // delete local authentication cookie
            await HttpContext.SignOutAsync();

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetName()));
        }

        // check if we need to trigger sign-out at an upstream identity provider
        if (vm.TriggerExternalSignout)
        {
            // build a return URL so the upstream provider will redirect back
            // to us after the user has logged out. this allows us to then
            // complete our single sign-out processing.
            string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

            // this triggers a redirect to the external provider for sign-out
            return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
        }

        return View("LoggedOut", vm);
    }

Once the user is signed-out of the external provider and then redirected back, the normal sign-out processing at IdentityServer should execute which involves processing the ``logoutId`` and doing all necessary cleanup.
