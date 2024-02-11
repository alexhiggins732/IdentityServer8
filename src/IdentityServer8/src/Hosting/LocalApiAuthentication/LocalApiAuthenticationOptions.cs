/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using Microsoft.AspNetCore.Authentication;

namespace IdentityServer8.Hosting.LocalApiAuthentication
{
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
}