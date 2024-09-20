.. _refProtectingApis:
Protecting APIs
===============
IdentityServer issues access tokens in the `JWT <https://tools.ietf.org/html/rfc7519>`_ (JSON Web Token) format by default.

Every relevant platform today has support for validating JWT tokens, a good list of JWT libraries can be found `here <https://jwt.io>`_.
Popular libraries are e.g.:

* `JWT bearer authentication handler <https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/>`_ for ASP.NET Core
* `JWT bearer authentication middleware <https://www.nuget.org/packages/Microsoft.Owin.Security.Jwt>`_ for Katana

Protecting an ASP.NET Core-based API is only a matter of adding the JWT bearer authentication handler::

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // base-address of your identityserver
                    options.Authority = "https://demo.identityserver.io";

                    // if you are using API resources, you can specify the name here
                    options.Audience = "resource1";

                    // IdentityServer emits a typ header by default, recommended extra check
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });
        }
    }

.. note:: If you are not using the audience claim, you can turn off the audience check via ``options.TokenValidationParameters.ValidateAudience = false;``. See :ref:`here <refApiResources>` for more information on resources, scopes, audiences and authorization.

Validating reference tokens
^^^^^^^^^^^^^^^^^^^^^^^^^^^
If you are using reference tokens, you need an authentication handler that implements `OAuth 2.0 token introspection <https://tools.ietf.org/html/rfc7662>`_, 
e.g. `this one <https://github.com/IdentityModel/IdentityModel.AspNetCore.OAuth2Introspection>`_:: 

    services.AddAuthentication("token")
        .AddOAuth2Introspection("token", options =>
        {
            options.Authority = Constants.Authority;

            // this maps to the API resource name and secret
            options.ClientId = "resource1";
            options.ClientSecret = "secret";
        });

Supporting both JWTs and reference tokens
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
You can setup ASP.NET Core to dispatch to the right handler based on the incoming token, see `this <https://leastprivilege.com/2020/07/06/flexible-access-token-validation-in-asp-net-core/>`_ blog post for more information.
In this case you setup one default handler, and some forwarding logic, e.g.::

    services.AddAuthentication("token")

        // JWT tokens
        .AddJwtBearer("token", options =>
        {
            options.Authority = Constants.Authority;
            options.Audience = "resource1";

            options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

            // if token does not contain a dot, it is a reference token
            options.ForwardDefaultSelector = Selector.ForwardReferenceToken("introspection");
        })

        // reference tokens
        .AddOAuth2Introspection("introspection", options =>
        {
            options.Authority = Constants.Authority;

            options.ClientId = "resource1";
            options.ClientSecret = "secret";
        });