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
using IdentityServer8.ResponseHandling;

namespace IdentityServer8.Logging.Models
{
    internal class AuthorizeResponseLog
    {
        public string SubjectId { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }

        public string Scope { get; set; }
        public string Error { get; set; }
        public string ErrorDescription { get; set; }


        public AuthorizeResponseLog(AuthorizeResponse response)
        {
            ClientId = response.Request?.Client?.ClientId;
            SubjectId = response.Request?.Subject?.GetSubjectId();
            RedirectUri = response.RedirectUri;
            State = response.State;
            Scope = response.Scope;
            Error = response.Error;
            ErrorDescription = response.ErrorDescription;
        }

        public override string ToString()
        {
            return LogSerializer.Serialize(this);
        }
    }
}