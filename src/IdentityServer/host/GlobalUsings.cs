/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

global using IdentityModel;
global using IdentityServer8;
global using IdentityServer8.Configuration;
global using IdentityServer8.Events;
global using IdentityServer8.Extensions;
global using IdentityServer8.Models;
global using IdentityServer8.Services;
global using IdentityServer8.Stores;
global using IdentityServer8.Test;
global using IdentityServer8.Validation;
global using IdentityServerHost.Configuration;
global using IdentityServerHost.Extensions;
global using IdentityServerHost.Quickstart.UI;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.Certificate;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.HttpOverrides;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Logging;
global using Microsoft.IdentityModel.Tokens;
global using Newtonsoft.Json;
global using Serilog;
global using Serilog.Events;
global using Serilog.Sinks.SystemConsole.Themes;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics;
global using System.Security.Claims;
global using System.Security.Cryptography.X509Certificates;
global using System.Text;
global using static IdentityServer8.IdentityServerConstants;
global using ILogger = Microsoft.Extensions.Logging.ILogger;
global using ClaimValueTypes = System.Security.Claims.ClaimValueTypes;
