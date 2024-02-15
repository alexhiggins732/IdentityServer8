/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Options for the IdentityServer middleware
/// </summary>
public class IdentityServerMiddlewareOptions
{
    /// <summary>
    /// Callback to wire up an authentication middleware
    /// </summary>
    public Action<IApplicationBuilder> AuthenticationMiddleware { get; set; } = (app) => app.UseAuthentication();
}
