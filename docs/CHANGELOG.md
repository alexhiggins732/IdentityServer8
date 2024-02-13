# Change Log
All notable changes to this project will be documented in this file.
 
The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning 2](http://semver.org/).
 
## [Unreleased] - 2024-02-13

Current templates and quickstarts will be added to seperate template and quickstart repositories to continue previous version functionality.

DotNet tool to install template currently under development.


## [8.0.3] - 2024-02-12

- Security Updates: Addtional priority critical security patches addressing issues outline in #9 and #10.
 - [Security: User-controlled bypass of sensitive method] - Login Controller and view have have explicit methods to handle login and cancel to address User-controlled bypass of sensitive method
 - [Security: Logging of user-controlled data] - Unsanitized user input could be used to forge logs and inject arbitrary commands, including server side includes, xss and sql injection into log files.
- [Maitenance]: Removed over half a million lines of code from the orginal Identity Server 4 code base using packages and libaries.
 - This will allow for easier maintenance and updates to the code base.
 - Developrs can now focus on the core functionality of Identity Server 8 and use LibMan to manage client side packages and keep packages up to date.
- Documentation Website: identityserver8.readthedocs.io has been created and is now the official documentation website for IdentityServer8
- Gitter: A Gitter chat room has been created for IdentityServer8. You can join the chat at https://app.gitter.im/#/room/#identityserver8:gitter.im
- Framework Upgrade: Upgrade Samples, including Clients, Quickstarts, and Key Management, to use DotNet 8 sdk style.
- [Quickstarts] (https://github.com/alexhiggins732/IdentityServer8/tree/master/samples/Quickstarts) - Updated Quickstart samples to use Dotnet 8 startup with implicit usings and minimal Api.
- [Clients] (https://github.com/alexhiggins732/IdentityServer8/tree/master/samples/Clients) - Updated client samples to use Dotnet 8 startup with implicit usings and minimal Api.
- [Key Management] (https://github.com/alexhiggins732/IdentityServer8/tree/master/samples/KeyManagement) - Updated Key management samples to use Dotnet 8 startup with implicit usings and minimal Api. Changed default Entity Framework storage to file system storage as original Key Management is a paid solution. Roadmap: Add DbContext implementation fof key management.
- Client Side Packages: Client Side packages have now been ignored in source and are now installed using LibMan during the build process. This will allow for easier updates and management of client side packages.

## [8.0.2] - 2024-02-12

- Security Updates: Addtional priority critical security patches addressing issues outline in #9 and #10.

## [8.0.1] - 2024-02-10
 
- Security Update: High priority critical security patches addressing issues outline in #9 and #10.

 
### Added
- `IdentityServer8.Security` nuget packages with services to sanitize user input including html, json, xml, javascript, scripts, urls, logs, css, and style sheets.

### Changed
- [Account Login Controller] (https://github.com/alexhiggins732/IdentityServer8/issues/9) 
- [Account Login View] (https://github.com/alexhiggins732/IdentityServer8/issues/9)  
 
### Fixed
- [Security: User-controlled bypass of sensitive method]
  Login Controller and view have have explicit methods to handle login and cancel to address User-controlled bypass of sensitive method
- [Security: Logging of user-controlled data]
  Unsanitized user input could be used to forge logs and inject arbitrary commands, including server side includes, xss and sql injection into log files.
  
## [8.0.1] - 2024-02-10
  
Updated build scripts to use Git Flow branching for SemVer2 compatible nuget packages.
 
### Added

- CodeQl Security scanning
- Dependabot Package scanning. 
### Changed
  
- [IdentityServer8 8.0.1 changes]https://github.com/alexhiggins732/IdentityServer8/pull/7) 

### Fixed
 
- Nuget Package version conflicts.
 
## [8.0.0] - 2024-02-09
 
### Added
Build scripts and readme documentation for initial port from Identity Server 4 and Identity Server 4 Admin   
### Changed
Upgraded Main Identity Server projects and Nuget packages to DotNet 8 
### Fixed
 
- Changed mixed dependencies on `System.Text.Json` and `Newtonsoft.Json` to use `System.Text.Json` which resolved several bugs.
- Change package dependencies and version requirements to run on the latest DotNet 8 packages, resolving many security vulnerablities.