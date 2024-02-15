/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Configuration;

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
