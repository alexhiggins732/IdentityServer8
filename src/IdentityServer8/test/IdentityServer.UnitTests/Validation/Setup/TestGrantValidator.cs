/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Threading.Tasks;
using IdentityServer8.Models;
using IdentityServer8.Validation;

namespace IdentityServer.UnitTests.Validation.Setup
{
    internal class TestGrantValidator : IExtensionGrantValidator
    {
        private readonly bool _isInvalid;
        private readonly string _errorDescription;

        public TestGrantValidator(bool isInvalid = false, string errorDescription = null)
        {
            _isInvalid = isInvalid;
            _errorDescription = errorDescription;
        }

        public Task<GrantValidationResult> ValidateAsync(ValidatedTokenRequest request)
        {
            if (_isInvalid)
            {
                return Task.FromResult(new GrantValidationResult(TokenRequestErrors.InvalidGrant, _errorDescription));
            }

            return Task.FromResult(new GrantValidationResult("bob", "CustomGrant"));
        }

        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            if (_isInvalid)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, _errorDescription);
            }
            else
            {
                context.Result = new GrantValidationResult("bob", "CustomGrant");
            }

            return Task.CompletedTask;
        }

        public string GrantType
        {
            get { return "custom_grant"; }
        }
    }
}
