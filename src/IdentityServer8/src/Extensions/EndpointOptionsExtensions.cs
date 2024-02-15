/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using Endpoint = IdentityServer8.Hosting.Endpoint;

namespace IdentityServer8.Extensions;

internal static class EndpointOptionsExtensions
{
    public static bool IsEndpointEnabled(this EndpointsOptions options, Endpoint endpoint)
    {
        return endpoint?.Name switch
        {
            EndpointNames.Authorize => options.EnableAuthorizeEndpoint,
            EndpointNames.CheckSession => options.EnableCheckSessionEndpoint,
            EndpointNames.DeviceAuthorization => options.EnableDeviceAuthorizationEndpoint,
            EndpointNames.Discovery => options.EnableDiscoveryEndpoint,
            EndpointNames.EndSession => options.EnableEndSessionEndpoint,
            EndpointNames.Introspection => options.EnableIntrospectionEndpoint,
            EndpointNames.Revocation => options.EnableTokenRevocationEndpoint,
            EndpointNames.Token => options.EnableTokenEndpoint,
            EndpointNames.UserInfo => options.EnableUserInfoEndpoint,
            _ => true
        };
    }
}
