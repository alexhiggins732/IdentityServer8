# Change Log
All notable changes to this project will be documented in this file.
 
The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning 2](http://semver.org/).
 
## [Unreleased] - 2024-02-11


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