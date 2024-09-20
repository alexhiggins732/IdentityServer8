/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Endpoints.Results;

/// <summary>
/// Result for introspection
/// </summary>
/// <seealso cref="IdentityServer8.Hosting.IEndpointResult" />
public class IntrospectionResult : IEndpointResult
{
    /// <summary>
    /// Gets the result.
    /// </summary>
    /// <value>
    /// The result.
    /// </value>
    public Dictionary<string, object> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IntrospectionResult"/> class.
    /// </summary>
    /// <param name="entries">The result.</param>
    /// <exception cref="System.ArgumentNullException">result</exception>
    public IntrospectionResult(Dictionary<string, object> entries)
    {
        Entries = entries ?? throw new ArgumentNullException(nameof(entries));
    }

    /// <summary>
    /// Executes the result.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns></returns>
    public Task ExecuteAsync(HttpContext context)
    {
        context.Response.SetNoCache();
        
        return context.Response.WriteJsonAsync(Entries);
    }
}
