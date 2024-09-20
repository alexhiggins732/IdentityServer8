# Identity Server 8 update
This project is a .NET 8 revival of the IdentityServer 4 and IdentityServer 4 Admin UI, for Open ID Connect (OIDC) and OAuth, which was archived when .NET Core 3.1 reached end of support.

The latest verion, 8.0.4, is now available on NuGet. It contains [hundreds of security and bug fixes](docs/CHANGELOG.md) from the original IdentityServer 4 project.

It is recommend you update all previous version, 4 or 8, to the latest version to ensure you have the latest security updates. 

- [Documentation](http://IdentityServer.readthedocs.io/)
- [Support](https://IdentityServer.readthedocs.io/en/latest/into/support.html)
- [Gitter Chat](https://app.gitter.im/#/room/#IdentityServer:gitter.im)

[IdentityServer and Admin UI Nuget Packages](https://www.nuget.org/packages?q=IdentityServer) are available here for use in DotNet 8.

All new development in the archived repository has moved to a paid commercial version in the [Duende Software](https://github.com/duendesoftware) organization. 

See [here](https://duendesoftware.com/products/identityserver) for more details on the commerical version.

This repository will be maintained for bug fixes and security updates for the .NET 8 version of Identity Server 4.

The source code and unit tests will be updated to use the latest .NET 8 features and best practices.

Once the source code and unit tests are stabilized, the documentation will be updated to reflect the changes.

General speaking, the existing documentation for Identity Server 4 will be valid for this repository, but the new features and changes will be documented in the new documentation.

## Build Status And Stats

[![Master | Build](https://github.com/mvput/IdentityServer/actions/workflows/master.yml/badge.svg)](https://github.com/mvput/IdentityServer/actions/workflows/master.yml)
[![Release|Build](https://github.com/mvput/IdentityServer/actions/workflows/release.yml/badge.svg)](https://github.com/mvput/IdentityServer/actions/workflows/release.yml)

[![Develop|Build](https://github.com/mvput/IdentityServer/actions/workflows/develop.yml/badge.svg)](https://github.com/mvput/IdentityServer/actions/workflows/develop.yml)
[![CI/CD|Build](https://github.com/mvput/IdentityServer/actions/workflows/pre-release.yml/badge.svg)](https://github.com/mvput/IdentityServer/actions/workflows/pre-release.yml)

## Code Coverage
[![Master | Build](https://github.com/mvput/IdentityServer/actions/workflows/master.yml/badge.svg)](https://img.shields.io/codecov/c/github/alexhiggins732/IdentityServer) [![Master|CodeQL](https://github.com/mvput/IdentityServer/actions/workflows/codeql.yml/badge.svg)](https://github.com/mvput/IdentityServer/actions/workflows/codeql.yml)

[![Develop|Build](https://github.com/mvput/IdentityServer/actions/workflows/develop.yml/badge.svg)](https://img.shields.io/codecov/c/github/alexhiggins732/IdentityServer/tree/develop)
[![Master|CodeQL](https://github.com/mvput/IdentityServer/actions/workflows/codeql.yml/badge.svg?branch=develop)](https://github.com/mvput/IdentityServer/actions/workflows/codeql.yml?branch=develop)

## Documentation
[![Documentation Status](https://readthedocs.org/projects/IdentityServer/badge/?version=latest)](https://IdentityServer.readthedocs.io/en/latest/?badge=latest)
[Read the docs - IdentityServer.readthedocs.io ](http://IdentityServer.readthedocs.io/)

## Nuget Packages

### Identity Server 8
|Package||
| ------------- | ------------- |
|[IdentityServer](https://www.nuget.org/packages?q=IdentityServer)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer)|
|[IdentityServer.Storage](https://www.nuget.org/packages?q=IdentityServer.Storage)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Storage)|
|[IdentityServer.EntityFramework](https://www.nuget.org/packages?q=IdentityServer.EntityFramework)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.EntityFramework)|
|[IdentityServer.EntityFramework.Storage](https://www.nuget.org/packages?q=IdentityServer.EntityFramework.Storage)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.EntityFramework.Storage)|
|[IdentityServer.AspNetIdentity](https://www.nuget.org/packages?q=IdentityServer.AspNetIdentity)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.AspNetIdentity)|
| | |

### Identity Server 8 Administration UI
|Package||
| ------------- | ------------- |
|[IdentityServer.Shared](https://www.nuget.org/packages?q=IdentityServer.Shared)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Shared)|
|[IdentityServer.Admin.UI](https://www.nuget.org/packages?q=IdentityServer.Admin.UI)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.UI)|
|[IdentityServer.Shared.Configuration](https://www.nuget.org/packages?q=IdentityServer.Shared.Configuration)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Shared.Configuration)|
|[IdentityServer.Admin.Api](https://www.nuget.org/packages?q=IdentityServer.Admin.Api)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.Api)|
|[IdentityServer.Admin](https://www.nuget.org/packages?q=IdentityServer.Admin)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin)|
|[IdentityServer.Admin.BusinessLogic.Identity](https://www.nuget.org/packages?q=IdentityServer.Admin.BusinessLogic.Identity)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.BusinessLogic.Identity)|
|[IdentityServer.Admin.EntityFramework](https://www.nuget.org/packages?q=IdentityServer.Admin.EntityFramework)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.EntityFramework)|
|[IdentityServer.Admin.EntityFramework.Identity](https://www.nuget.org/packages?q=IdentityServer.Admin.EntityFramework.Identity)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.EntityFramework.Identity)|
|[IdentityServer.Admin.BusinessLogic.Shared](https://www.nuget.org/packages?q=IdentityServer.Admin.BusinessLogic.Shared)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.BusinessLogic.Shared)|
|[IdentityServer.Admin.EntityFramework.Extensions](https://www.nuget.org/packages?q=IdentityServer.Admin.EntityFramework.Extensions)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.EntityFramework.Extensions)|
|[IdentityServer.Admin.EntityFramework.PostgreSQL](https://www.nuget.org/packages?q=IdentityServer.Admin.EntityFramework.PostgreSQL)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.EntityFramework.PostgreSQL)|
|[IdentityServer.Admin.EntityFramework.MySql](https://www.nuget.org/packages?q=IdentityServer.Admin.EntityFramework.MySql)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.EntityFramework.MySql)|
|[IdentityServer.Admin.EntityFramework.SqlServer](https://www.nuget.org/packages?q=IdentityServer.Admin.EntityFramework.SqlServer)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.EntityFramework.SqlServer)|
|[IdentityServer.Admin.EntityFramework.Shared](https://www.nuget.org/packages?q=IdentityServer.Admin.EntityFramework.Shared)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.EntityFramework.Shared)|
|[IdentityServer.Admin.BusinessLogic](https://www.nuget.org/packages?q=IdentityServer.Admin.BusinessLogic)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.BusinessLogic)|
|[IdentityServer.Admin.EntityFramework.Configuration](https://www.nuget.org/packages?q=IdentityServer.Admin.EntityFramework.Configuration)|![NuGet Downloads](https://img.shields.io/nuget/dt/IdentityServer.Admin.EntityFramework.Configuration)|
| | |


## What's New

View the [CHANGELOG](docs/CHANGELOG.md) for the latest changes.

## About IdentityServer
[<img align="right" width="100px" src="https://dotnetfoundation.org/img/logo_big.svg" />](https://dotnetfoundation.org/projects?searchquery=IdentityServer&type=project)

IdentityServer is a free, open source [OpenID Connect](http://openid.net/connect/) and [OAuth 2.0](https://tools.ietf.org/html/rfc6749) framework for ASP.NET Core.
Founded and maintained by [Dominick Baier](https://twitter.com/leastprivilege) and [Brock Allen](https://twitter.com/brocklallen), IdentityServer incorporates all the protocol implementations and extensibility points needed to integrate token-based authentication, single-sign-on and API access control in your applications.
IdentityServer is officially [certified](https://openid.net/certification/) by the [OpenID Foundation](https://openid.net) and thus spec-compliant and interoperable.
It is part of the [.NET Foundation](https://www.dotnetfoundation.org/), and operates under their [code of conduct](https://www.dotnetfoundation.org/code-of-conduct). It is licensed under [Apache 2](https://opensource.org/licenses/Apache-2.0) (an OSI approved license).

For project documentation, please visit [readthedocs](https://IdentityServer.readthedocs.io).

## Branch structure
Active development happens on the main branch. This always contains the latest version. Each (pre-) release is tagged with the corresponding version. The [aspnetcore1](https://github.com/mvput/IdentityServer/tree/aspnetcore1) and [aspnetcore2](https://github.com/mvput/IdentityServer/tree/aspnetcore2) branches contain the latest versions of the older ASP.NET Core based versions.

## How to build

* [Install]([https://www.microsoft.com/net/download/core#/current](https://dotnet.microsoft.com/en-us/download#/current) the latest .NET 8 SDK
* Install Git
* Clone this repo
* Run `dotnet build src/IdentityServer.slm` or `build.sh` in the root of the cloned repo.

## Documentation
For project documentation, please visit [readthedocs](https://IdentityServer.readthedocs.io).

See [here](http://docs.IdentityServer.io/en/aspnetcore1/) for the 1.x docs, and [here](http://docs.IdentityServer.io/en/aspnetcore2/) for the 2.x docs.

## Bug reports and feature requests
Please use the [issue tracker](https://github.com/mvput/IdentityServer/issues) for that. We only support the latest version for free. For older versions, you can get a commercial support agreement with us.

## Commercial and Community Support
If you need help with implementing IdentityServer or your security architecture in general, there are both free and commercial support options.
See [here](https://IdentityServer.readthedocs.io/en/latest/intro/support.html) for more details.

## Sponsorship
If you are a fan of the project or a company that relies on IdentityServer, you might want to consider sponsoring.
This will help us devote more time to answering questions and doing feature development. If you are interested please head to our [Patreon](https://www.patreon.com/identityserver) page which has further details.

### Platinum Sponsors
[<img src="https://user-images.githubusercontent.com/1454075/62819413-39550c00-bb55-11e9-8f2f-a268c3552c71.png" width="200">](https://udelt.no)

[<img src="https://user-images.githubusercontent.com/1454075/66454740-fb973580-ea68-11e9-9993-6c1014881528.png" width="200">](https://github.com/dotnet-at-microsoft)

### Corporate Sponsors
[Ritter Insurance Marketing](https://www.ritterim.com)  
[ExtraNetUserManager](https://www.extranetusermanager.com/)  
[Knab](https://www.knab.nl/)

You can see a list of our current sponsors [here](https://github.com/mvput/IdentityServer/blob/main/SPONSORS.md) - and for companies we have some nice advertisement options as well.

## Acknowledgements
IdentityServer is built using the following great open source projects and free services:

* [ASP.NET Core](https://github.com/dotnet/aspnetcore)
* [Bullseye](https://github.com/adamralph/bullseye)
* [SimpleExec](https://github.com/adamralph/simple-exec)
* [MinVer](https://github.com/adamralph/minver)
* [Json.Net](http://www.newtonsoft.com/json)
* [XUnit](https://xunit.github.io/)
* [Fluent Assertions](http://www.fluentassertions.com/)
* [GitReleaseManager](https://github.com/GitTools/GitReleaseManager)

..and last but not least a big thanks to all our [contributors](https://github.com/mvput/IdentityServer/graphs/contributors)!
