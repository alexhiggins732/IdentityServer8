Packaging and Builds
====================

IdentityServer consists of a number of nuget packages.

IdentityServer main repo
^^^^^^^^^^^^^^^
`github <https://github.com/mvput/IdentityServer>`_

Contains the core IdentityServer object model, services and middleware as well as the EntityFramework and ASP.NET Identity integration.

nugets:

* `IdentityServer <https://www.nuget.org/packages/IdentityServer/>`_
* `IdentityServer.EntityFramework <https://www.nuget.org/packages/IdentityServer.EntityFramework>`_
* `IdentityServer.AspNetIdentity <https://www.nuget.org/packages/IdentityServer.AspNetIdentity>`_

Quickstart UI
^^^^^^^^^^^^^
`github <https://github.com/mvput/IdentityServer.Quickstart.UI>`_

Contains a simple starter UI including login, logout and consent pages.

Access token validation handler
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
`nuget <https://www.nuget.org/packages/IdentityServer.AccessTokenValidation>`_ | `github <https://github.com/mvput/IdentityServer.AccessTokenValidation>`_

ASP.NET Core authentication handler for validating tokens in APIs. The handler allows supporting both JWT and reference tokens in the same API.

Templates
^^^^^^^^^
`nuget <https://www.nuget.org/packages/IdentityServer.Templates>`_ | `github <https://github.com/mvput/IdentityServer.Templates>`_

Contains templates for the dotnet CLI.

Dev builds
^^^^^^^^^^
In addition we publish CI builds to our package repository.
Add the following ``nuget.config`` to your project::

    <?xml version="1.0" encoding="utf-8"?>
        <configuration>
            <packageSources>
                <clear />
                <add key="IdentityServer CI" value="https://www.myget.org/F/identity/api/v3/index.json" />
            </packageSources>
        </configuration>
