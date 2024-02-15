/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Models;

internal static class AuthorizeResponseExtensions
{
    public static NameValueCollection ToNameValueCollection(this AuthorizeResponse response)
    {
        var collection = new NameValueCollection();

        if (response.IsError)
        {
            if (response.Error.IsPresent())
            {
                collection.Add("error", response.Error);
            }
            if (response.ErrorDescription.IsPresent())
            {
                collection.Add("error_description", response.ErrorDescription);
            }
        }
        else
        {
            if (response.Code.IsPresent())
            {
                collection.Add("code", response.Code);
            }

            if (response.IdentityToken.IsPresent())
            {
                collection.Add("id_token", response.IdentityToken);
            }

            if (response.AccessToken.IsPresent())
            {
                collection.Add("access_token", response.AccessToken);
                collection.Add("token_type", "Bearer");
                collection.Add("expires_in", response.AccessTokenLifetime.ToString());
            }

            if (response.Scope.IsPresent())
            {
                collection.Add("scope", response.Scope);
            }
        }

        if (response.State.IsPresent())
        {
            collection.Add("state", response.State);
        }
        
        if (response.SessionState.IsPresent())
        {
            collection.Add("session_state", response.SessionState);
        }

        return collection;
    }
}
