/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityServer8.Extensions;
using System;
using System.Security.Claims;

namespace IdentityServer8.Models
{
    /// <summary>
    /// Context describing the is-active check
    /// </summary>
    public class IsActiveContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsActiveContext"/> class.
        /// </summary>
        public IsActiveContext(ClaimsPrincipal subject, Client client, string caller)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (caller.IsMissing()) throw new ArgumentNullException(nameof(caller));

            Subject = subject;
            Client = client;
            Caller = caller;
            
            IsActive = true;
        }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public ClaimsPrincipal Subject { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public Client Client { get; set; }

        /// <summary>
        /// Gets or sets the caller.
        /// </summary>
        /// <value>
        /// The caller.
        /// </value>
        public string Caller { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the subject is active and can recieve tokens.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the subject is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }
    }
}