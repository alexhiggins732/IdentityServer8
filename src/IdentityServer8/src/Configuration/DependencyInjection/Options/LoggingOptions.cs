/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Configuration;

/// <summary>
/// Options for configuring logging behavior
/// </summary>
public class LoggingOptions
{
    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> TokenRequestSensitiveValuesFilter { get; set; } = 
        new HashSet<string>
        {
            OidcConstants.TokenRequest.ClientSecret,
            OidcConstants.TokenRequest.Password,
            OidcConstants.TokenRequest.ClientAssertion,
            OidcConstants.TokenRequest.RefreshToken,
            OidcConstants.TokenRequest.DeviceCode
        };

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> AuthorizeRequestSensitiveValuesFilter { get; set; } = 
        new HashSet<string>
        {
            OidcConstants.AuthorizeRequest.IdTokenHint
        };
}
