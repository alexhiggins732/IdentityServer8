/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.Models;
using IdentityServer8.Validation;

namespace IdentityServer.UnitTests.Validation.Setup
{ 
    public static class ValidationExtensions
    {
        public static ClientSecretValidationResult ToValidationResult(this Client client, ParsedSecret secret = null)
        {
            return new ClientSecretValidationResult
            {
                Client = client,
                Secret = secret
            };
        }
    }
}
