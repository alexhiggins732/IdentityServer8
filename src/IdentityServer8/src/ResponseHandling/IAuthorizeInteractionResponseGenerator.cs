/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.ResponseHandling;

/// <summary>
/// Interface for determining if user must login or consent when making requests to the authorization endpoint.
/// </summary>
public interface IAuthorizeInteractionResponseGenerator
{
    /// <summary>
    /// Processes the interaction logic.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="consent">The consent.</param>
    /// <returns></returns>
    Task<InteractionResponse> ProcessInteractionAsync(ValidatedAuthorizeRequest request, ConsentResponse consent = null);
}
