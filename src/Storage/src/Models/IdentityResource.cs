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
using System.Collections.Generic;
using System.Diagnostics;

namespace IdentityServer8.Models
{
    /// <summary>
    /// Models a user identity resource.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class IdentityResource : Resource
    {
        private string DebuggerDisplay => Name ?? $"{{{typeof(IdentityResource)}}}";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityResource"/> class.
        /// </summary>
        public IdentityResource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityResource"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="userClaims">List of associated user claims that should be included when this resource is requested.</param>
        public IdentityResource(string name, IEnumerable<string> userClaims)
            : this(name, name, userClaims)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityResource"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="userClaims">List of associated user claims that should be included when this resource is requested.</param>
        /// <exception cref="System.ArgumentNullException">name</exception>
        /// <exception cref="System.ArgumentException">Must provide at least one claim type - claimTypes</exception>
        public IdentityResource(string name, string displayName, IEnumerable<string> userClaims)
        {
            if (name.IsMissing()) throw new ArgumentNullException(nameof(name));
            if (userClaims.IsNullOrEmpty()) throw new ArgumentException("Must provide at least one claim type", nameof(userClaims));

            Name = name;
            DisplayName = displayName;

            foreach(var type in userClaims)
            {
                UserClaims.Add(type);
            }
        }

        /// <summary>
        /// Specifies whether the user can de-select the scope on the consent screen (if the consent screen wants to implement such a feature). Defaults to false.
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// Specifies whether the consent screen will emphasize this scope (if the consent screen wants to implement such a feature). 
        /// Use this setting for sensitive or important scopes. Defaults to false.
        /// </summary>
        public bool Emphasize { get; set; } = false;
    }
}
