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
/// Default user consent store.
/// </summary>
public class DefaultUserConsentStore : DefaultGrantStore<Consent>, IUserConsentStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultUserConsentStore"/> class.
    /// </summary>
    /// <param name="store">The store.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="handleGenerationService">The handle generation service.</param>
    /// <param name="logger">The logger.</param>
    public DefaultUserConsentStore(
        IPersistedGrantStore store, 
        IPersistentGrantSerializer serializer,
        IHandleGenerationService handleGenerationService,
        ILogger<DefaultUserConsentStore> logger) 
        : base(IdentityServerConstants.PersistedGrantTypes.UserConsent, store, serializer, handleGenerationService, logger)
    {
    }

    private string GetConsentKey(string subjectId, string clientId)
    {
        return clientId + "|" + subjectId;
    }

    /// <summary>
    /// Stores the user consent asynchronous.
    /// </summary>
    /// <param name="consent">The consent.</param>
    /// <returns></returns>
    public Task StoreUserConsentAsync(Consent consent)
    {
        var key = GetConsentKey(consent.SubjectId, consent.ClientId);
        return StoreItemAsync(key, consent, consent.ClientId, consent.SubjectId, null, null, consent.CreationTime, consent.Expiration);
    }

    /// <summary>
    /// Gets the user consent asynchronous.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <returns></returns>
    public Task<Consent> GetUserConsentAsync(string subjectId, string clientId)
    {
        var key = GetConsentKey(subjectId, clientId);
        return GetItemAsync(key);
    }

    /// <summary>
    /// Removes the user consent asynchronous.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <returns></returns>
    public Task RemoveUserConsentAsync(string subjectId, string clientId)
    {
        var key = GetConsentKey(subjectId, clientId);
        return RemoveItemAsync(key);
    }
}
