/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer8.Services
{
    /// <summary>
    /// Default user code service implementation.
    /// </summary>
    /// <seealso cref="IdentityServer8.Services.IUserCodeService" />
    public class DefaultUserCodeService : IUserCodeService
    {
        private readonly IEnumerable<IUserCodeGenerator> _generators;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUserCodeService"/> class.
        /// </summary>
        /// <param name="generators">The generators.</param>
        /// <exception cref="ArgumentNullException">generators</exception>
        public DefaultUserCodeService(IEnumerable<IUserCodeGenerator> generators)
        {
            _generators = generators ?? throw new ArgumentNullException(nameof(generators));
        }

        /// <summary>
        /// Gets the user code generator.
        /// </summary>
        /// <param name="userCodeType">Type of user code.</param>
        /// <returns></returns>
        public Task<IUserCodeGenerator> GetGenerator(string userCodeType) =>
            Task.FromResult(_generators.FirstOrDefault(x => x.UserCodeType == userCodeType));
    }
}