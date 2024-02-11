.. _refCors:
CORS
====

Many endpoints in IdentityServer will be accessed via Ajax calls from JavaScript-based clients.
Given that IdentityServer will most likely be hosted on a different origin than these clients, this implies that `Cross-Origin Resource Sharing <http://www.html5rocks.com/en/tutorials/cors/>`_ (CORS) will need to be configured.

Client-based CORS Configuration
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

One approach to configuring CORS is to use the ``AllowedCorsOrigins`` collection on the :ref:`client configuration <refClient>`.
Simply add the origin of the client to the collection and the default configuration in IdentityServer will consult these values to allow cross-origin calls from the origins.

.. Note:: Be sure to use an origin (not a URL) when configuring CORS. For example: ``https://foo:123/`` is a URL, whereas ``https://foo:123`` is an origin.

This default CORS implementation will be in use if you are using either the "in-memory" or EF-based client configuration that we provide.
If you define your own ``IClientStore``, then you will need to implement your own custom CORS policy service (see below).

Custom Cors Policy Service
^^^^^^^^^^^^^^^^^^^^^^^^^^

IdentityServer allows the hosting application to implement the ``ICorsPolicyService`` to completely control the CORS policy.

The single method to implement is: ``Task<bool> IsOriginAllowedAsync(string origin)``. 
Return ``true`` if the `origin` is allowed, ``false`` otherwise.

Once implemented, simply register the implementation in DI and IdentityServer will then use your custom implementation.

**DefaultCorsPolicyService**

If you simply wish to hard-code a set of allowed origins, then there is a pre-built ``ICorsPolicyService`` implementation you can use called ``DefaultCorsPolicyService``.
This would be configured as a singleton in DI, and hard-coded with its ``AllowedOrigins`` collection, or setting the flag ``AllowAll`` to ``true`` to allow all origins.
For example, in ``ConfigureServices``::

    services.AddSingleton<ICorsPolicyService>((container) => {
        var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
        return new DefaultCorsPolicyService(logger) {
            AllowedOrigins = { "https://foo", "https://bar" }
        };
    });

.. Note:: Use ``AllowAll`` with caution.


Mixing IdentityServer's CORS policy with ASP.NET Core's CORS policies
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

IdentityServer uses the CORS middleware from ASP.NET Core to provide its CORS implementation.
It is possible that your application that hosts IdentityServer might also require CORS for its own custom endpoints.
In general, both should work together in the same application.

Your code should use the documented CORS features from ASP.NET Core without regard to IdentityServer.
This means you should define policies and register the middleware as normal.
If your application defines policies in ``ConfigureServices``, then those should continue to work in the same places you are using them (either where you configure the CORS middleware or where you use the MVC ``EnableCors`` attributes in your controller code).
If instead you define an inline policy in the use of the CORS middleware (via the policy builder callback), then that too should continue to work normally.

The one scenario where there might be a conflict between your use of the ASP.NET Core CORS services and IdentityServer is if you decide to create a custom ``ICorsPolicyProvider``.
Given the design of the ASP.NET Core's CORS services and middleware, IdentityServer implements its own custom ``ICorsPolicyProvider`` and registers it in the DI system.
Fortunately, the IdentityServer implementation is designed to use the decorator pattern to wrap any existing  ``ICorsPolicyProvider`` that is already registered in DI.
What this means is that you can also implement the ``ICorsPolicyProvider``, but it simply needs to be registered prior to IdentityServer in DI (e.g. in ``ConfigureServices``).
