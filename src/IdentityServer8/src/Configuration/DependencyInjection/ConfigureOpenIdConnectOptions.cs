/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System;
using System.Linq;
using IdentityServer8.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace IdentityServer8.Configuration
{
    internal class ConfigureOpenIdConnectOptions : IPostConfigureOptions<OpenIdConnectOptions>
    {
        private string[] _schemes;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConfigureOpenIdConnectOptions(string[] schemes, IHttpContextAccessor httpContextAccessor)
        {
            _schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void PostConfigure(string name, OpenIdConnectOptions options)
        {
            // no schemes means configure them all
            if (_schemes.Length == 0 || _schemes.Contains(name))
            {
                options.StateDataFormat = new DistributedCacheStateDataFormatter(_httpContextAccessor, name);
            }
        }
    }
}