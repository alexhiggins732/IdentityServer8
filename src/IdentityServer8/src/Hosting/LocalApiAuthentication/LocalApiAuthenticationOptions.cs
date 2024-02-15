/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Hosting.LocalApiAuthentication;

/// <summary>
/// Options for local API authentication
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions" />
public class LocalApiAuthenticationOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Allows setting a specific required scope (optional)
    /// </summary>
    public string ExpectedScope { get; set; }

    /// <summary>
    /// Specifies whether the token should be saved in the authentication properties
    /// </summary>
    public bool SaveToken { get; set; } = true;

    /// <summary>
    /// Allows implementing events
    /// </summary>
    public new LocalApiAuthenticationEvents Events
    {
        get { return (LocalApiAuthenticationEvents)base.Events; }
        set { base.Events = value; }
    }
}
