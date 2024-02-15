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
/// Result for revocation error
/// </summary>
/// <seealso cref="IdentityServer8.Hosting.IEndpointResult" />
public class TokenRevocationErrorResult : IEndpointResult
{
    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    /// <value>
    /// The error.
    /// </value>
    public string Error { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenRevocationErrorResult"/> class.
    /// </summary>
    /// <param name="error">The error.</param>
    public TokenRevocationErrorResult(string error)
    {
        Error = error;
    }

    /// <summary>
    /// Executes the result.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns></returns>
    public Task ExecuteAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return context.Response.WriteJsonAsync(new { error = Error });
    }
}
