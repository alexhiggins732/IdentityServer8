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
/// Models a user's authentication session
/// </summary>
public interface IUserSession
{
    /// <summary>
    /// Creates a session identifier for the signin context and issues the session id cookie.
    /// </summary>
    Task<string> CreateSessionIdAsync(ClaimsPrincipal principal, AuthenticationProperties properties);

    /// <summary>
    /// Gets the current authenticated user.
    /// </summary>
    Task<ClaimsPrincipal> GetUserAsync();

    /// <summary>
    /// Gets the current session identifier.
    /// </summary>
    /// <returns></returns>
    Task<string> GetSessionIdAsync();

    /// <summary>
    /// Ensures the session identifier cookie asynchronous.
    /// </summary>
    /// <returns></returns>
    Task EnsureSessionIdCookieAsync();

    /// <summary>
    /// Removes the session identifier cookie.
    /// </summary>
    Task RemoveSessionIdCookieAsync();

    /// <summary>
    /// Adds a client to the list of clients the user has signed into during their session.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <returns></returns>
    Task AddClientIdAsync(string clientId);

    /// <summary>
    /// Gets the list of clients the user has signed into during their session.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<string>> GetClientListAsync();
}
