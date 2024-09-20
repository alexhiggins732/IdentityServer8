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
/// Validates API secrets using the registered secret validators and parsers
/// </summary>
public class ApiSecretValidator : IApiSecretValidator
{
    private readonly ILogger _logger;
    private readonly IResourceStore _resources;
    private readonly IEventService _events;
    private readonly ISecretsListParser _parser;
    private readonly ISecretsListValidator _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiSecretValidator"/> class.
    /// </summary>
    /// <param name="resources">The resources.</param>
    /// <param name="parsers">The parsers.</param>
    /// <param name="validator">The validator.</param>
    /// <param name="events">The events.</param>
    /// <param name="logger">The logger.</param>
    public ApiSecretValidator(IResourceStore resources, ISecretsListParser parsers, ISecretsListValidator validator, IEventService events, ILogger<ApiSecretValidator> logger)
    {
        _resources = resources;
        _parser = parsers;
        _validator = validator;
        _events = events;
        _logger = logger;
    }

    /// <summary>
    /// Validates the secret on the current request.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public async Task<ApiSecretValidationResult> ValidateAsync(HttpContext context)
    {
        _logger.LogTrace("Start API validation");

        var fail = new ApiSecretValidationResult
        {
            IsError = true
        };

        var parsedSecret = await _parser.ParseAsync(context);
        if (parsedSecret == null)
        {
            await RaiseFailureEventAsync("unknown", "No API id or secret found");

            _logger.LogError("No API secret found");
            return fail;
        }

        // load API resource
        var apis = await _resources.FindApiResourcesByNameAsync(new[] { parsedSecret.Id });
        if (apis == null || !apis.Any())
        {
            await RaiseFailureEventAsync(parsedSecret.Id, "Unknown API resource");

            _logger.LogError("No API resource with that name found. aborting");
            return fail;
        }

        if (apis.Count() > 1)
        {
            await RaiseFailureEventAsync(parsedSecret.Id, "Invalid API resource");

            _logger.LogError("More than one API resource with that name found. aborting");
            return fail;
        }

        var api = apis.Single();

        if (api.Enabled == false)
        {
            await RaiseFailureEventAsync(parsedSecret.Id, "API resource not enabled");

            _logger.LogError("API resource not enabled. aborting.");
            return fail;
        }

        var result = await _validator.ValidateAsync(api.ApiSecrets, parsedSecret);
        if (result.Success)
        {
            _logger.LogDebug("API resource validation success");

            var success = new ApiSecretValidationResult
            {
                IsError = false,
                Resource = api
            };

            await RaiseSuccessEventAsync(api.Name, parsedSecret.Type);
            return success;
        }

        await RaiseFailureEventAsync(api.Name, "Invalid API secret");
        _logger.LogError("API validation failed.");

        return fail;
    }

    private Task RaiseSuccessEventAsync(string clientId, string authMethod)
    {
        return _events.RaiseAsync(new ApiAuthenticationSuccessEvent(clientId, authMethod));
    }

    private Task RaiseFailureEventAsync(string clientId, string message)
    {
        return _events.RaiseAsync(new ApiAuthenticationFailureEvent(clientId, message));
    }
}
