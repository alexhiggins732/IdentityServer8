/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Services;

/// <summary>
/// Service to retrieve and update consent.
/// </summary>
public interface IConsentService
{
    /// <summary>
    /// Checks if consent is required.
    /// </summary>
    /// <param name="subject">The user.</param>
    /// <param name="client">The client.</param>
    /// <param name="parsedScopes">The parsed scopes.</param>
    /// <returns>
    /// Boolean if consent is required.
    /// </returns>
    Task<bool> RequiresConsentAsync(ClaimsPrincipal subject, Client client, IEnumerable<ParsedScopeValue> parsedScopes);

    /// <summary>
    /// Updates the consent.
    /// </summary>
    /// <param name="subject">The subject.</param>
    /// <param name="client">The client.</param>
    /// <param name="parsedScopes">The parsed scopes.</param>
    /// <returns></returns>
    Task UpdateConsentAsync(ClaimsPrincipal subject, Client client, IEnumerable<ParsedScopeValue> parsedScopes);
}
