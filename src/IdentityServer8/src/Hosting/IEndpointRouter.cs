/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using Microsoft.AspNetCore.Http;

namespace IdentityServer8.Hosting
{
    /// <summary>
    /// The endpoint router
    /// </summary>
    public interface IEndpointRouter
    {
        /// <summary>
        /// Finds a matching endpoint.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        IEndpointHandler Find(HttpContext context);
    }
}