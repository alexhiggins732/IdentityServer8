/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer8.Models;
using IdentityServer8.Validation;

namespace IdentityServer.IntegrationTests.Clients.Setup;

public class ExtensionGrantValidator : IExtensionGrantValidator
{
    public Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        var credential = context.Request.Raw.Get("custom_credential");
        var extraClaim = context.Request.Raw.Get("extra_claim");

        if (credential != null)
        {
            if (extraClaim != null)
            {
                context.Result = new GrantValidationResult(
                    subject: "818727",
                    claims: new[] { new Claim("extra_claim", extraClaim) },
                    authenticationMethod: GrantType);
            }
            else
            {
                context.Result = new GrantValidationResult(subject: "818727", authenticationMethod: GrantType);
            }
        }
        else
        {
            // custom error message
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid_custom_credential");
        }

        return Task.CompletedTask;
    }

    public string GrantType =>  "custom";
}
