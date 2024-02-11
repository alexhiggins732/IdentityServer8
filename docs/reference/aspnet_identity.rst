.. _refAspNetId:
ASP.NET Identity Support
========================

An ASP.NET Identity-based implementation is provided for managing the identity database for users of IdentityServer.
This implementation implements the extensibility points in IdentityServer needed to load identity data for your users to emit claims into tokens.

The repo for this support is located `here <https://github.com/IdentityServer/IdentityServer8.AspNetIdentity/>`_ and the NuGet package is `here <https://www.nuget.org/packages/IdentityServer8.AspNetIdentity>`_.

To use this library, configure ASP.NET Identity normally. 
Then use the ``AddAspNetIdentity`` extension method after the call to ``AddIdentityServer``::

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddIdentityServer()
            .AddAspNetIdentity<ApplicationUser>();
    }

``AddAspNetIdentity`` requires as a generic parameter the class that models your user for ASP.NET Identity (and the same one passed to ``AddIdentity`` to configure ASP.NET Identity).
This configures IdentityServer to use the ASP.NET Identity implementations of ``IUserClaimsPrincipalFactory``, ``IResourceOwnerPasswordValidator``, and ``IProfileService``.
It also configures some of ASP.NET Identity's options for use with IdentityServer (such as claim types to use and authentication cookie settings).

When using your own implementation of ``IUserClaimsPrincipalFactory``, make sure that you register it before calling the IdentityServer ``AddAspNetIdentity`` extension method.