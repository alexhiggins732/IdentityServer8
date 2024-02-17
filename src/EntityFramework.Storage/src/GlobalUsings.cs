/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

global using AutoMapper;
global using IdentityModel;
global using IdentityServer8.EntityFramework.DbContexts;
global using IdentityServer8.EntityFramework.Extensions;
global using IdentityServer8.EntityFramework.Interfaces;
//global using IdentityServer8.EntityFramework.Entities;
global using IdentityServer8.EntityFramework.Mappers;
global using IdentityServer8.EntityFramework.Options;
global using IdentityServer8.Extensions;
//global using IdentityServer8.Models;
global using IdentityServer8.Stores;
global using IdentityServer8.Stores.Serialization;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using System.Buffers;
global using System.Runtime.CompilerServices;
global using ClaimValueTypes = System.Security.Claims.ClaimValueTypes;
