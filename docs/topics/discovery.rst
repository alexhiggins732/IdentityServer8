Discovery
=========

The discovery document can be found at *https://baseaddress/.well-known/openid-configuration*. 
It contains information about the endpoints, key material and features of your IdentityServer.

By default all information is included in the discovery document, but by using configuration options, you can hide
individual sections, e.g.::

    services.AddIdentityServer(options =>
    {
        options.Discovery.ShowIdentityScopes = false;
        options.Discovery.ShowApiScopes = false;
        options.Discovery.ShowClaims = false;
        options.Discovery.ShowExtensionGrantTypes = false;
    });

Extending discovery
^^^^^^^^^^^^^^^^^^^
You can add custom entries to the discovery document, e.g::

    services.AddIdentityServer(options =>
    {
        options.Discovery.CustomEntries.Add("my_setting", "foo");
        options.Discovery.CustomEntries.Add("my_complex_setting", 
            new
            {
                foo = "foo",
                bar = "bar"
            });
    });

When you add a custom value that starts with ~/ it will be expanded to an absolute path below the IdentityServer base address, e.g.::

    options.Discovery.CustomEntries.Add("my_custom_endpoint", "~/custom");

If you want to take full control over the rendering of the discovery (and jwks) document, you can implement the ``IDiscoveryResponseGenerator``
interface (or derive from our default implementation).