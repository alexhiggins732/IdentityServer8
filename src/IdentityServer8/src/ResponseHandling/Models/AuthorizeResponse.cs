/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

#pragma warning disable 1591

namespace IdentityServer8.ResponseHandling;

public class AuthorizeResponse
{
    public ValidatedAuthorizeRequest Request { get; set; }
    public string RedirectUri => Request?.RedirectUri;
    public string State => Request?.State;
    public string Scope => Request?.ValidatedResources?.RawScopeValues.ToSpaceSeparatedString();

    public string IdentityToken { get; set; }
    public string AccessToken { get; set; }
    public int AccessTokenLifetime { get; set; }
    public string Code { get; set; }
    public string SessionState { get; set; }

    public string Error { get; set; }
    public string ErrorDescription { get; set; }
    public bool IsError => Error.IsPresent();
}
