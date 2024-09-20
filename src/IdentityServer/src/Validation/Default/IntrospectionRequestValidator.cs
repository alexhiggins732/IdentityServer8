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
/// The introspection request validator
/// </summary>
/// <seealso cref="IdentityServer.Validation.IIntrospectionRequestValidator" />
internal class IntrospectionRequestValidator : IIntrospectionRequestValidator
{
    private readonly ILogger _logger;
    private readonly ITokenValidator _tokenValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntrospectionRequestValidator"/> class.
    /// </summary>
    /// <param name="tokenValidator">The token validator.</param>
    /// <param name="logger">The logger.</param>
    public IntrospectionRequestValidator(ITokenValidator tokenValidator, ILogger<IntrospectionRequestValidator> logger)
    {
        _tokenValidator = tokenValidator;
        _logger = logger;
    }

    /// <summary>
    /// Validates the request.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <param name="api">The API.</param>
    /// <returns></returns>
    public async Task<IntrospectionRequestValidationResult> ValidateAsync(NameValueCollection parameters, ApiResource api)
    {
        _logger.LogDebug("Introspection request validation started.");

        // retrieve required token
        var token = parameters.Get("token");
        if (token == null)
        {
            _logger.LogError("Token is missing");

            return new IntrospectionRequestValidationResult
            {
                IsError = true,
                Api = api,
                Error = "missing_token",
                Parameters = parameters
            };
        }

        // validate token
        var tokenValidationResult = await _tokenValidator.ValidateAccessTokenAsync(token);

        // invalid or unknown token
        if (tokenValidationResult.IsError)
        {
            _logger.LogDebug("Token is invalid.");

            return new IntrospectionRequestValidationResult
            {
                IsActive = false,
                IsError = false,
                Token = token,
                Api = api,
                Parameters = parameters
            };
        }

        _logger.LogDebug("Introspection request validation successful.");

        // valid token
        return new IntrospectionRequestValidationResult
        {
            IsActive = true,
            IsError = false,
            Token = token,
            Claims = tokenValidationResult.Claims,
            Api = api,
            Parameters = parameters
        };
    }
}
