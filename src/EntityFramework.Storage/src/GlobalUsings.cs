/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

global using AutoMapper;
global using IdentityModel;
global using IdentityServer.EntityFramework.DbContexts;
global using IdentityServer.EntityFramework.Extensions;
global using IdentityServer.EntityFramework.Interfaces;
//global using IdentityServer.EntityFramework.Entities;
global using IdentityServer.EntityFramework.Mappers;
global using IdentityServer.EntityFramework.Options;
global using IdentityServer.Extensions;
//global using IdentityServer.Models;
global using IdentityServer.Stores;
global using IdentityServer.Stores.Serialization;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using System.Buffers;
global using System.Runtime.CompilerServices;
global using ClaimValueTypes = System.Security.Claims.ClaimValueTypes;
