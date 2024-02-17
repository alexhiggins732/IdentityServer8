/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Hosting.LocalApiAuthentication;

/// <summary>
/// Events for local API authentication
/// </summary>
public class LocalApiAuthenticationEvents
{
    /// <summary>
    /// Invoked after the security token has passed validation and a ClaimsIdentity has been generated.
    /// </summary>
    public Func<ClaimsTransformationContext, Task> OnClaimsTransformation { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Invoked after the security token has passed validation and a ClaimsIdentity has been generated.
    /// </summary>
    public virtual Task ClaimsTransformation(ClaimsTransformationContext context) => OnClaimsTransformation(context);

}

/// <summary>
/// Context class for local API claims transformation
/// </summary>
public class ClaimsTransformationContext
{
    /// <summary>
    /// The principal
    /// </summary>
    public ClaimsPrincipal Principal { get; set; }

    /// <summary>
    /// the HTTP context
    /// </summary>
    public HttpContext HttpContext { get; internal set; }
}
