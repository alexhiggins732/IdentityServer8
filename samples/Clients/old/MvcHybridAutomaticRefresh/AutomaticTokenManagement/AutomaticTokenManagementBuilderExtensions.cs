/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace IdentityModel.AspNetCore
{
    public static class AutomaticTokenManagementBuilderExtensions
    {
        public static AuthenticationBuilder AddAutomaticTokenManagement(this AuthenticationBuilder builder, Action<AutomaticTokenManagementOptions> options)
        {
            builder.Services.Configure(options);
            return builder.AddAutomaticTokenManagement();
        }

        public static AuthenticationBuilder AddAutomaticTokenManagement(this AuthenticationBuilder builder)
        {
            builder.Services.AddHttpClient("tokenClient");
            builder.Services.AddTransient<TokenEndpointService>();

            builder.Services.AddTransient<AutomaticTokenManagementCookieEvents>();
            builder.Services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, AutomaticTokenManagementConfigureCookieOptions>();

            return builder;
        }
    }
}
