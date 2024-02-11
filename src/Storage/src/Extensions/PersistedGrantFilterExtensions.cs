/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityServer8.Stores;
using System;

namespace IdentityServer8.Extensions
{
    /// <summary>
    /// Extensions for PersistedGrantFilter.
    /// </summary>
    public static class PersistedGrantFilterExtensions
    {
        /// <summary>
        /// Validates the PersistedGrantFilter and throws if invalid.
        /// </summary>
        /// <param name="filter"></param>
        public static void Validate(this PersistedGrantFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            if (String.IsNullOrWhiteSpace(filter.ClientId) &&
                String.IsNullOrWhiteSpace(filter.SessionId) &&
                String.IsNullOrWhiteSpace(filter.SubjectId) &&
                String.IsNullOrWhiteSpace(filter.Type))
            {
                throw new ArgumentException("No filter values set.", nameof(filter));
            }
        }
    }
}