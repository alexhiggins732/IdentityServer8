/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System.Collections.Generic;
using System.Linq;
using IdentityModel;
using IdentityServer8.Extensions;
using IdentityServer8.Validation;

namespace IdentityServer8.Logging.Models
{
    internal class AuthorizeRequestValidationLog
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string RedirectUri { get; set; }
        public IEnumerable<string> AllowedRedirectUris { get; set; }
        public string SubjectId { get; set; }

        public string ResponseType { get; set; }
        public string ResponseMode { get; set; }
        public string GrantType { get; set; }
        public string RequestedScopes { get; set; }

        public string State { get; set; }
        public string UiLocales { get; set; }
        public string Nonce { get; set; }
        public IEnumerable<string> AuthenticationContextReferenceClasses { get; set; }
        public string DisplayMode { get; set; }
        public string PromptMode { get; set; }
        public int? MaxAge { get; set; }
        public string LoginHint { get; set; }
        public string SessionId { get; set; }

        public Dictionary<string, string> Raw { get; set; }

        public AuthorizeRequestValidationLog(ValidatedAuthorizeRequest request, IEnumerable<string> sensitiveValuesFilter)
        {
            Raw = request.Raw.ToScrubbedDictionary(sensitiveValuesFilter.ToArray());

            if (request.Client != null)
            {
                ClientId = request.Client.ClientId;
                ClientName = request.Client.ClientName;

                AllowedRedirectUris = request.Client.RedirectUris;
            }

            if (request.Subject != null)
            {
                var subjectClaim = request.Subject.FindFirst(JwtClaimTypes.Subject);
                if (subjectClaim != null)
                {
                    SubjectId = subjectClaim.Value;
                }
                else
                {
                    SubjectId = "anonymous";
                }
            }

            if (request.AuthenticationContextReferenceClasses.Any())
            {
                AuthenticationContextReferenceClasses = request.AuthenticationContextReferenceClasses;
            }

            RedirectUri = request.RedirectUri;
            ResponseType = request.ResponseType;
            ResponseMode = request.ResponseMode;
            GrantType = request.GrantType;
            RequestedScopes = request.RequestedScopes.ToSpaceSeparatedString();
            State = request.State;
            UiLocales = request.UiLocales;
            Nonce = request.Nonce;

            DisplayMode = request.DisplayMode;
            PromptMode = request.PromptModes.ToSpaceSeparatedString();
            LoginHint = request.LoginHint;
            MaxAge = request.MaxAge;
            SessionId = request.SessionId;
        }

        public override string ToString()
        {
            return LogSerializer.Serialize(this);
        }
    }
}