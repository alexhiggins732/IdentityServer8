/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.Models;
using IdentityServer8.Services;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.UnitTests.Common
{
    class MockKeyMaterialService : IKeyMaterialService
    {
        public List<SigningCredentials> SigningCredentials = new List<SigningCredentials>();
        public List<SecurityKeyInfo> ValidationKeys = new List<SecurityKeyInfo>();

        public Task<IEnumerable<SigningCredentials>> GetAllSigningCredentialsAsync()
        {
            return Task.FromResult(SigningCredentials.AsEnumerable());
        }

        public Task<SigningCredentials> GetSigningCredentialsAsync(IEnumerable<string> allowedAlgorithms = null)
        {
            return Task.FromResult(SigningCredentials.FirstOrDefault());
        }

        public Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
        {
            return Task.FromResult(ValidationKeys.AsEnumerable());
        }
    }
}
