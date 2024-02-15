/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServerHost.Extensions;

public class ParameterizedScopeTokenRequestValidator : ICustomTokenRequestValidator
{
    public Task ValidateAsync(CustomTokenRequestValidationContext context)
    {
        var transaction = context.Result.ValidatedRequest.ValidatedResources.ParsedScopes.FirstOrDefault(x => x.ParsedName == "transaction");
        if (transaction?.ParsedParameter != null)
        {
            context.Result.ValidatedRequest.ClientClaims.Add(new Claim(transaction.ParsedName, transaction.ParsedParameter));
        }

        return Task.CompletedTask;
    }
}
