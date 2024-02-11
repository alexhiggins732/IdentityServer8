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
    /// Models access to an API scope
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class ApiScope : Resource
    {
        private string DebuggerDisplay => Name ?? $"{{{typeof(ApiScope)}}}";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiScope"/> class.
        /// </summary>
        public ApiScope()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiScope"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ApiScope(string name)
            : this(name, name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiScope"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">The display name.</param>
        public ApiScope(string name, string displayName)
            : this(name, displayName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiScope"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="userClaims">List of associated user claims that should be included when this resource is requested.</param>
        public ApiScope(string name, IEnumerable<string> userClaims)
            : this(name, name, userClaims)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiScope"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="userClaims">List of associated user claims that should be included when this resource is requested.</param>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public ApiScope(string name, string displayName, IEnumerable<string> userClaims)
        {
            if (name.IsMissing()) throw new ArgumentNullException(nameof(name));

            Name = name;
            DisplayName = displayName;

            if (!userClaims.IsNullOrEmpty())
            {
                foreach (var type in userClaims)
                {
                    UserClaims.Add(type);
                }
            }
        }

        /// <summary>
        /// Specifies whether the user can de-select the scope on the consent screen. Defaults to false.
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// Specifies whether the consent screen will emphasize this scope. Use this setting for sensitive or important scopes. Defaults to false.
        /// </summary>
        public bool Emphasize { get; set; } = false;
    }
}
