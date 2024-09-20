/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.Validation;
using Xunit;

namespace IdentityServer.UnitTests.Extensions
{
    public class ValidatedAuthorizeRequestExtensionsTests
    {
        [Fact]
        public void GetAcrValues_should_return_snapshot_of_values()
        {
            var request = new ValidatedAuthorizeRequest()
            {
                Raw = new System.Collections.Specialized.NameValueCollection()
            };
            request.AuthenticationContextReferenceClasses.Add("a");
            request.AuthenticationContextReferenceClasses.Add("b");
            request.AuthenticationContextReferenceClasses.Add("c");

            var acrs = request.GetAcrValues();
            foreach(var acr in acrs)
            {
                request.RemoveAcrValue(acr);
            }
        }
    }
}
