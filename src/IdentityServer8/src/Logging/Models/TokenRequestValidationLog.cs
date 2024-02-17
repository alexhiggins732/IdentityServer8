/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Logging.Models;

internal class TokenRequestValidationLog
{
    public string ClientId { get; set; }
    public string ClientName { get; set; }
    public string GrantType { get; set; }
    public string Scopes { get; set; }

    public string AuthorizationCode { get; set; }
    public string RefreshToken { get; set; }

    public string UserName { get; set; }
    public IEnumerable<string> AuthenticationContextReferenceClasses { get; set; }
    public string Tenant { get; set; }
    public string IdP { get; set; }

    public Dictionary<string, string> Raw { get; set; }

    public TokenRequestValidationLog(ValidatedTokenRequest request, IEnumerable<string> sensitiveValuesFilter)
    {
        Raw = request.Raw.ToScrubbedDictionary(sensitiveValuesFilter.ToArray());

        if (request.Client != null)
        {
            ClientId = request.Client.ClientId;
            ClientName = request.Client.ClientName;
        }

        if (request.RequestedScopes != null)
        {
            Scopes = request.RequestedScopes.ToSpaceSeparatedString();
        }

        GrantType = request.GrantType;
        AuthorizationCode = request.AuthorizationCodeHandle.Obfuscate();
        RefreshToken = request.RefreshTokenHandle.Obfuscate();
        UserName = request.UserName;
    }

    public override string ToString()
    {
        return LogSerializer.Serialize(this);
    }
}
