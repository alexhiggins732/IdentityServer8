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
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using IdentityServer8.ResponseHandling;

namespace IdentityServer8.Endpoints.Results
{
    internal class TokenErrorResult : IEndpointResult
    {
        public TokenErrorResponse Response { get; }

        public TokenErrorResult(TokenErrorResponse error)
        {
            if (error.Error.IsMissing()) throw new ArgumentNullException(nameof(error.Error), "Error must be set");

            Response = error;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = 400;
            context.Response.SetNoCache();

            var dto = new ResultDto
            {
                error = Response.Error,
                error_description = Response.ErrorDescription,
                
                custom = Response.Custom
            };

            await context.Response.WriteJsonAsync(dto);
        }

        internal class ResultDto
        {
            public string error { get; set; }
            public string error_description { get; set; }

            [JsonExtensionData]
            public Dictionary<string, object> custom { get; set; }
        }    
    }
}
