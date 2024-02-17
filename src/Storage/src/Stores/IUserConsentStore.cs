/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Stores;

/// <summary>
/// Interface for user consent storage
/// </summary>
public interface IUserConsentStore
{
    /// <summary>
    /// Stores the user consent.
    /// </summary>
    /// <param name="consent">The consent.</param>
    /// <returns></returns>
    Task StoreUserConsentAsync(Consent consent);

    /// <summary>
    /// Gets the user consent.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <returns></returns>
    Task<Consent> GetUserConsentAsync(string subjectId, string clientId);

    /// <summary>
    /// Removes the user consent.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <returns></returns>
    Task RemoveUserConsentAsync(string subjectId, string clientId);
}
