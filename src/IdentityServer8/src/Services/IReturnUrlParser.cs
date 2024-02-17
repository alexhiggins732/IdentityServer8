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
/// Interface for the return URL parser
/// </summary>
public interface IReturnUrlParser
{
    /// <summary>
    /// Parses a return URL.
    /// </summary>
    /// <param name="returnUrl">The return URL.</param>
    /// <returns></returns>
    Task<AuthorizationRequest> ParseAsync(string returnUrl);

    /// <summary>
    /// Determines whether the return URL is valid.
    /// </summary>
    /// <param name="returnUrl">The return URL.</param>
    /// <returns>
    ///   <c>true</c> if the return URL is valid; otherwise, <c>false</c>.
    /// </returns>
    bool IsValidReturnUrl(string returnUrl);
}
