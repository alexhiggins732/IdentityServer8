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
/// Models the result of JWT request validation.
/// </summary>
public class JwtRequestValidationResult : ValidationResult
{
    /// <summary>
    /// The key/value pairs from the JWT payload of a successfuly validated request.
    /// </summary>
    public Dictionary<string, string> Payload { get; set; }
}
