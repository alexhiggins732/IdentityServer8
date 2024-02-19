/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer8.Models;
using IdentityServer8.Validation;

namespace IdentityServer.IntegrationTests.Clients.Setup;

public class CustomResponseExtensionGrantValidator : IExtensionGrantValidator
{
    public Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        var response = new Dictionary<string, object>
        {
            { "string_value", "some_string" },
            { "int_value", 42 },
            { "dto",  CustomResponseDto.Create }
        };

        var credential = context.Request.Raw.Get("outcome");

        if (credential == "succeed")
        {
            context.Result = new GrantValidationResult("bob", "custom", customResponse: response);
        }
        else
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid_credential", response);
        }

        return Task.CompletedTask;
    }

    public string GrantType
    {
        get { return "custom"; }
    }
}
