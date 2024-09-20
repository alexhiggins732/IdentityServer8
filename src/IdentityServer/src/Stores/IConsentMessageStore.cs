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
/// Interface for consent messages that are sent from the consent UI to the authorization endpoint.
/// </summary>
public interface IConsentMessageStore
{
    /// <summary>
    /// Writes the consent response message.
    /// </summary>
    /// <param name="id">The id for the message.</param>
    /// <param name="message">The message.</param>
    Task WriteAsync(string id, Message<ConsentResponse> message);

    /// <summary>
    /// Reads the consent response message.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<Message<ConsentResponse>> ReadAsync(string id);

    /// <summary>
    /// Deletes the consent response message.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task DeleteAsync(string id);
}
