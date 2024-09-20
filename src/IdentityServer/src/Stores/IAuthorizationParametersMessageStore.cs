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
/// Interface for authorization request messages that are sent from the authorization endpoint to the login and consent UI.
/// </summary>
public interface IAuthorizationParametersMessageStore
{
    /// <summary>
    /// Writes the authorization parameters.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>The identifier for the stored message.</returns>
    Task<string> WriteAsync(Message<IDictionary<string, string[]>> message);

    /// <summary>
    /// Reads the authorization parameters.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<Message<IDictionary<string, string[]>>> ReadAsync(string id);

    /// <summary>
    /// Deletes the authorization parameters.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task DeleteAsync(string id);
}
