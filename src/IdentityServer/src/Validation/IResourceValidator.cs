/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Validation;

/// <summary>
/// Validates requested resources (scopes and resource indicators)
/// </summary>
public interface IResourceValidator
{
    // todo: should this be used anywhere we re-create tokens? do we need to re-run scope validation?

    /// <summary>
    /// Validates the requested resources for the client.
    /// </summary>
    Task<ResourceValidationResult> ValidateRequestedResourcesAsync(ResourceValidationRequest request);
}
