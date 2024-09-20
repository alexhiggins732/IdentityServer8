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
/// Default implementation of redirect URI validator. Validates the URIs against
/// the client's configured URIs.
/// </summary>
public class StrictRedirectUriValidator : IRedirectUriValidator
{
    /// <summary>
    /// Checks if a given URI string is in a collection of strings (using ordinal ignore case comparison)
    /// </summary>
    /// <param name="uris">The uris.</param>
    /// <param name="requestedUri">The requested URI.</param>
    /// <returns></returns>
    protected bool StringCollectionContainsString(IEnumerable<string> uris, string requestedUri)
    {
        if (uris.EnumerableIsNullOrEmpty()) return false;

        return uris.Contains(requestedUri, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether a redirect URI is valid for a client.
    /// </summary>
    /// <param name="requestedUri">The requested URI.</param>
    /// <param name="client">The client.</param>
    /// <returns>
    ///   <c>true</c> is the URI is valid; <c>false</c> otherwise.
    /// </returns>
    public virtual Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
    {
        return Task.FromResult(StringCollectionContainsString(client.RedirectUris, requestedUri));
    }

    /// <summary>
    /// Determines whether a post logout URI is valid for a client.
    /// </summary>
    /// <param name="requestedUri">The requested URI.</param>
    /// <param name="client">The client.</param>
    /// <returns>
    ///   <c>true</c> is the URI is valid; <c>false</c> otherwise.
    /// </returns>
    public virtual Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
    {
        return Task.FromResult(StringCollectionContainsString(client.PostLogoutRedirectUris, requestedUri));
    }
}
