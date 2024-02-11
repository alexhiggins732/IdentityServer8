.. _refQuickstartOverview:
Overview
========
The quickstarts provide step by step instructions for various common IdentityServer scenarios.
They start with the absolute basics and become more complex - 
it is recommended you do them in order.

* adding IdentityServer to an ASP.NET Core application
* configuring IdentityServer
* issuing tokens for various clients
* securing web applications and APIs
* adding support for EntityFramework based configuration
* adding support for ASP.NET Identity

Every quickstart has a reference solution - you can find the code in the 
`samples <https://github.com/IdentityServer/IdentityServer8/tree/main/samples/Quickstarts>`_ folder.

Preparation
^^^^^^^^^^^
The first thing you should do is install our templates::

    dotnet new -i IdentityServer8.Templates

They will be used as a starting point for the various tutorials.

.. note:: If you are using private NuGet sources do not forget to add the --nuget-source parameter: --nuget-source https://api.nuget.org/v3/index.json

OK - let's get started!

.. note:: The quickstarts target the IdentityServer 4.x and ASP.NET Core 3.1.x - there are also quickstarts for `ASP.NET Core 2 <http://docs.identityserver.io/en/aspnetcore2/quickstarts/0_overview.html>`_ and `ASP.NET Core 1 <http://docs.identityserver.io/en/aspnetcore1/quickstarts/0_overview.html>`_.
