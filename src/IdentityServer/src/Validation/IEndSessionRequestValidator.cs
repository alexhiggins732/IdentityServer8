/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Validation;

/// <summary>
///  Validates end session requests.
/// </summary>
public interface IEndSessionRequestValidator
{
    /// <summary>
    /// Validates end session endpoint requests.
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="subject"></param>
    /// <returns></returns>
    Task<EndSessionValidationResult> ValidateAsync(NameValueCollection parameters, ClaimsPrincipal subject);

    /// <summary>
    ///  Validates requests from logout page iframe to trigger single signout.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<EndSessionCallbackValidationResult> ValidateCallbackAsync(NameValueCollection parameters);
}
