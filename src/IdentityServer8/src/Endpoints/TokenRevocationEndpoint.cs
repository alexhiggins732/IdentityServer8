/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityModel;
using IdentityServer8.Endpoints.Results;
using IdentityServer8.Hosting;
using IdentityServer8.Validation;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using IdentityServer8.Services;
using Microsoft.AspNetCore.Http;
using IdentityServer8.Events;
using IdentityServer8.ResponseHandling;
using IdentityServer8.Extensions;

namespace IdentityServer8.Endpoints
{
    /// <summary>
    /// The revocation endpoint
    /// </summary>
    /// <seealso cref="IdentityServer8.Hosting.IEndpointHandler" />
    internal class TokenRevocationEndpoint : IEndpointHandler
    {
        private readonly ILogger _logger;
        private readonly IClientSecretValidator _clientValidator;
        private readonly ITokenRevocationRequestValidator _requestValidator;
        private readonly ITokenRevocationResponseGenerator _responseGenerator;
        private readonly IEventService _events;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenRevocationEndpoint" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="clientValidator">The client validator.</param>
        /// <param name="requestValidator">The request validator.</param>
        /// <param name="responseGenerator">The response generator.</param>
        /// <param name="events">The events.</param>
        public TokenRevocationEndpoint(ILogger<TokenRevocationEndpoint> logger,
            IClientSecretValidator clientValidator,
            ITokenRevocationRequestValidator requestValidator,
            ITokenRevocationResponseGenerator responseGenerator,
            IEventService events)
        {
            _logger = logger;
            _clientValidator = clientValidator;
            _requestValidator = requestValidator;
            _responseGenerator = responseGenerator;

            _events = events;
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            _logger.LogTrace("Processing revocation request.");

            if (!HttpMethods.IsPost(context.Request.Method))
            {
                _logger.LogWarning("Invalid HTTP method");
                return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            if (!context.Request.HasApplicationFormContentType())
            {
                _logger.LogWarning("Invalid media type");
                return new StatusCodeResult(HttpStatusCode.UnsupportedMediaType);
            }

            var response = await ProcessRevocationRequestAsync(context);

            return response;
        }

        private async Task<IEndpointResult> ProcessRevocationRequestAsync(HttpContext context)
        {
            _logger.LogDebug("Start revocation request.");

            // validate client
            var clientValidationResult = await _clientValidator.ValidateAsync(context);

            if (clientValidationResult.IsError)
            {
                return new TokenRevocationErrorResult(OidcConstants.TokenErrors.InvalidClient);
            }

            _logger.LogTrace("Client validation successful");

            // validate the token request
            var form = (await context.Request.ReadFormAsync()).AsNameValueCollection();

            _logger.LogTrace("Calling into token revocation request validator: {type}", _requestValidator.GetType().FullName);
            var requestValidationResult = await _requestValidator.ValidateRequestAsync(form, clientValidationResult.Client);

            if (requestValidationResult.IsError)
            {
                return new TokenRevocationErrorResult(requestValidationResult.Error);
            }

            _logger.LogTrace("Calling into token revocation response generator: {type}", _responseGenerator.GetType().FullName);
            var response = await _responseGenerator.ProcessAsync(requestValidationResult);

            if (response.Success)
            {
                _logger.LogInformation("Token revocation complete");
                await _events.RaiseAsync(new TokenRevokedSuccessEvent(requestValidationResult, requestValidationResult.Client));
            }
            else
            {
                _logger.LogInformation("No matching token found");
            }

            if (response.Error.IsPresent()) return new TokenRevocationErrorResult(response.Error);

            return new StatusCodeResult(HttpStatusCode.OK);
        }
    }
}