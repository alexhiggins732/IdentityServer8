.. _refAspNetCoreAndApis:
ASP.NET Core and API access
===========================
In the previous quickstarts we explored both API access and user authentication. 
Now we want to bring the two parts together.

The beauty of the OpenID Connect & OAuth 2.0 combination is, that you can achieve both with a single protocol and a single exchange with the token service.

So far we only asked for identity resources during the token request, once we start also including API resources, IdentityServer will return two tokens:
the identity token containing the information about the authentication and session, and the access token to access APIs on behalf of the logged on user.

Modifying the client configuration
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Updating the client configuration in IdentityServer is straightforward - we simply need to add the ``api1`` resource to the allowed scopes list.
In addition we enable support for refresh tokens via the ``AllowOfflineAccess`` property::

    new Client
    {
        ClientId = "mvc",
        ClientSecrets = { new Secret("secret".Sha256()) },

        AllowedGrantTypes = GrantTypes.Code,
                
        // where to redirect to after login
        RedirectUris = { "https://localhost:5002/signin-oidc" },

        // where to redirect to after logout
        PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
        
        AllowOfflineAccess = true,

        AllowedScopes = new List<string>
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            "api1"
        }
    }

Modifying the MVC client
^^^^^^^^^^^^^^^^^^^^^^^^
All that's left to do now in the client is to ask for the additional resources via the scope parameter. This is done in the OpenID Connect handler configuration::

    services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
        .AddCookie("Cookies")
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = "https://localhost:5001";

            options.ClientId = "mvc";
            options.ClientSecret = "secret";
            options.ResponseType = "code";

            options.SaveTokens = true;

            options.Scope.Add("api1");
            options.Scope.Add("offline_access");
        });

Since ``SaveTokens`` is enabled, ASP.NET Core will automatically store the resulting access and refresh token in the authentication session.
You should be able to inspect the data on the page that prints out the contents of the session that you created earlier.

Using the access token
^^^^^^^^^^^^^^^^^^^^^^
You can access the tokens in the session using the standard ASP.NET Core extension methods 
that you can find in the ``Microsoft.AspNetCore.Authentication`` namespace::

    var accessToken = await HttpContext.GetTokenAsync("access_token");

For accessing the API using the access token, all you need to do is retrieve the token, and set it on your HttpClient::

    public async Task<IActionResult> CallApi()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var content = await client.GetStringAsync("https://localhost:6001/identity");

        ViewBag.Json = JArray.Parse(content).ToString();
        return View("json");
    }

Create a view called json.cshtml that outputs the json like this::

    <pre>@ViewBag.Json</pre>

Make sure the API is running, start the MVC client and call ``/home/CallApi`` after authentication.

Managing the access token
^^^^^^^^^^^^^^^^^^^^^^^^^
By far the most complex task for a typical client is to manage the access token. You typically want to 

* request the access and refresh token at login time
* cache those tokens
* use the access token to call APIs until it expires
* use the refresh token to get a new access token
* start over

ASP.NET Core has many built-in facility that can help you with those tasks (like caching or sessions), 
but there is still quite some work left to do. 
Feel free to have a look at `this <https://github.com/IdentityModel/IdentityModel.AspNetCore>`_ library, which can automate 
many of the boilerplate tasks.
