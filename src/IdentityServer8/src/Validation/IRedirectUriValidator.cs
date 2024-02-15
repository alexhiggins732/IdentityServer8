/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Validation;

/// <summary>
/// Models the logic when validating redirect and post logout redirect URIs.
/// </summary>
public interface IRedirectUriValidator
{
    /// <summary>
    /// Determines whether a redirect URI is valid for a client.
    /// </summary>
    /// <param name="requestedUri">The requested URI.</param>
    /// <param name="client">The client.</param>
    /// <returns><c>true</c> is the URI is valid; <c>false</c> otherwise.</returns>
    Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client);
    
    /// <summary>
    /// Determines whether a post logout URI is valid for a client.
    /// </summary>
    /// <param name="requestedUri">The requested URI.</param>
    /// <param name="client">The client.</param>
    /// <returns><c>true</c> is the URI is valid; <c>false</c> otherwise.</returns>
    Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client);
}
