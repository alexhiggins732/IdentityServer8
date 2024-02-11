.. _refExternalIdentityProviders:
Sign-in with External Identity Providers
========================================

ASP.NET Core has a flexible way to deal with external authentication. This involves a couple of steps.

.. Note:: If you are using ASP.NET Identity, many of the underlying technical details are hidden from you. It is recommended that you also read the Microsoft `docs <https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/>`_ and do the ASP.NET Identity :ref:`quickstart <refAspNetIdentityQuickstart>`.

Adding authentication handlers for external providers
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
The protocol implementation that is needed to talk to an external provider is encapsulated in an *authentication handler*.
Some providers use proprietary protocols (e.g. social providers like Facebook) and some use standard protocols, e.g. OpenID Connect, WS-Federation or SAML2p.

See this :ref:`quickstart <refExternalAuthenticationQuickstart>` for step-by-step instructions for adding external authentication and configuring it.

The role of cookies
^^^^^^^^^^^^^^^^^^^
One option on an external authentication handlers is called ``SignInScheme``, e.g.::

    services.AddAuthentication()
        .AddGoogle("Google", options =>
        {
            options.SignInScheme = "scheme of cookie handler to use";

            options.ClientId = "...";
            options.ClientSecret = "...";
        })

The signin scheme specifies the name of the cookie handler that will temporarily store the outcome of the external authentication, 
e.g. the claims that got sent by the external provider. This is necessary, since there are typically a couple of redirects involved until you are done with the 
external authentication process.

Given that this is such a common practise, IdentityServer registers a cookie handler specifically for this external provider workflow.
The scheme is represented via the ``IdentityServerConstants.ExternalCookieAuthenticationScheme`` constant.
If you were to use our external cookie handler, then for the ``SignInScheme`` above you'd assign the value to be the ``IdentityServerConstants.ExternalCookieAuthenticationScheme`` constant::

    services.AddAuthentication()
        .AddGoogle("Google", options =>
        {
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            options.ClientId = "...";
            options.ClientSecret = "...";
        })

You can also register your own custom cookie handler instead, like this::

    services.AddAuthentication()
        .AddCookie("YourCustomScheme")
        .AddGoogle("Google", options =>
        {
            options.SignInScheme = "YourCustomScheme";

            options.ClientId = "...";
            options.ClientSecret = "...";
        })

.. Note:: For specialized scenarios, you can also short-circuit the external cookie mechanism and forward the external user directly to the main cookie handler. This typically involves handling events on the external handler to make sure you do the correct claims transformation from the external identity source.

Triggering the authentication handler
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
You invoke an external authentication handler via the ``ChallengeAsync`` extension method on the ``HttpContext`` (or using the MVC ``ChallengeResult``).

You typically want to pass in some options to the challenge operation, e.g. the path to your callback page and the name of the provider for bookkeeping, e.g.::

    var callbackUrl = Url.Action("ExternalLoginCallback");
    
    var props = new AuthenticationProperties
    {
        RedirectUri = callbackUrl,
        Items = 
        { 
            { "scheme", provider },
            { "returnUrl", returnUrl }
        }
    };
    
    return Challenge(provider, props);

Handling the callback and signing in the user
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
On the callback page your typical tasks are:

* inspect the identity returned by the external provider.
* make a decision how you want to deal with that user. This might be different based on the fact if this is a new user or a returning user.
* new users might need additional steps and UI before they are allowed in.
* probably create a new internal user account that is linked to the external provider.
* store the external claims that you want to keep.
* delete the temporary cookie
* sign-in the user

**Inspecting the external identity**::

    // read external identity from the temporary cookie
    var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
    if (result?.Succeeded != true)
    {
        throw new Exception("External authentication error");
    }

    // retrieve claims of the external user
    var externalUser = result.Principal;
    if (externalUser == null)
    {
        throw new Exception("External authentication error");
    }

    // retrieve claims of the external user
    var claims = externalUser.Claims.ToList();

    // try to determine the unique id of the external user - the most common claim type for that are the sub claim and the NameIdentifier
    // depending on the external provider, some other claim type might be used
    var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
    if (userIdClaim == null)
    {
        userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
    }
    if (userIdClaim == null)
    {
        throw new Exception("Unknown userid");
    }
    
    var externalUserId = userIdClaim.Value;
    var externalProvider = userIdClaim.Issuer;

    // use externalProvider and externalUserId to find your user, or provision a new user

**Clean-up and sign-in**::

    // issue authentication cookie for user
    await HttpContext.SignInAsync(new IdentityServerUser(user.SubjectId) {
        DisplayName = user.Username,
        IdentityProvider = provider,
        AdditionalClaims = additionalClaims,
        AuthenticationTime = DateTime.Now
    });

    // delete temporary cookie used during external authentication
    await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

    // validate return URL and redirect back to authorization endpoint or a local page
    if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
    {
        return Redirect(returnUrl);
    }

    return Redirect("~/");

State, URL length, and ISecureDataFormat
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
When redirecting to an external provider for sign-in, frequently state from the client application must be round-tripped.
This means that state is captured prior to leaving the client and preserved until the user has returned to the client application.
Many protocols, including OpenID Connect, allow passing some sort of state as a parameter as part of the request, and the identity provider will return that state on the response.
The OpenID Connect authentication handler provided by ASP.NET Core utilizes this feature of the protocol, and that is how it implements the ``returnUrl`` feature mentioned above.

The problem with storing state in a request parameter is that the request URL can get too large (over the common limit of 2000 characters).
The OpenID Connect authentication handler does provide an extensibility point to store the state in your server, rather than in the request URL. 
You can implement this yourself by implementing ``ISecureDataFormat<AuthenticationProperties>`` and configuring it on the `OpenIdConnectOptions <https://github.com/aspnet/AspNetCore/blob/main/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectOptions.cs#L249>`_.

Fortunately, IdentityServer provides an implementation of this for you, backed by the ``IDistributedCache`` implementation registered in the DI container (e.g. the standard ``MemoryDistributedCache``).
To use the IdentityServer provided secure data format implementation, simply call the ``AddOidcStateDataFormatterCache`` extension method on the ``IServiceCollection`` when configuring DI.
If no parameters are passed, then all OpenID Connect handlers configured will use the IdentityServer provided secure data format implementation::

    public void ConfigureServices(IServiceCollection services)
    {
        // configures the OpenIdConnect handlers to persist the state parameter into the server-side IDistributedCache.
        services.AddOidcStateDataFormatterCache();

        services.AddAuthentication()
            .AddOpenIdConnect("demoidsrv", "IdentityServer", options =>
            {
                // ...
            })
            .AddOpenIdConnect("aad", "Azure AD", options =>
            {
                // ...
            })
            .AddOpenIdConnect("adfs", "ADFS", options =>
            {
                // ...
            });
    }


If only particular schemes are to be configured, then pass those schemes as parameters::

    public void ConfigureServices(IServiceCollection services)
    {
        // configures the OpenIdConnect handlers to persist the state parameter into the server-side IDistributedCache.
        services.AddOidcStateDataFormatterCache("aad", "demoidsrv");

        services.AddAuthentication()
            .AddOpenIdConnect("demoidsrv", "IdentityServer", options =>
            {
                // ...
            })
            .AddOpenIdConnect("aad", "Azure AD", options =>
            {
                // ...
            })
            .AddOpenIdConnect("adfs", "ADFS", options =>
            {
                // ...
            });
    }

