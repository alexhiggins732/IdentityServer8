/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Threading.Tasks;
using IdentityServer8.Hosting;
using Microsoft.AspNetCore.Http;
using IdentityServer8.Extensions;

namespace IdentityServer8.Endpoints.Results
{
    internal class BadRequestResult : IEndpointResult
    {
        public string Error { get; set; }
        public string ErrorDescription { get; set; }

        public BadRequestResult(string error = null, string errorDescription = null)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = 400;
            context.Response.SetNoCache();

            if (Error.IsPresent())
            {
                var dto = new ResultDto
                {
                    error = Error,
                    error_description = ErrorDescription
                };

                await context.Response.WriteJsonAsync(dto);
            }
        }

        internal class ResultDto
        {
            public string error { get; set; }
            public string error_description { get; set; }
        }    
    }
}