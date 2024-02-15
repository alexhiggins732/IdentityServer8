/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Endpoints;

internal class AuthorizeCallbackEndpoint : AuthorizeEndpointBase
{
    private readonly IConsentMessageStore _consentResponseStore;
    private readonly IAuthorizationParametersMessageStore _authorizationParametersMessageStore;

    public AuthorizeCallbackEndpoint(
        IEventService events,
        ILogger<AuthorizeCallbackEndpoint> logger,
        IdentityServerOptions options,
        IAuthorizeRequestValidator validator,
        IAuthorizeInteractionResponseGenerator interactionGenerator,
        IAuthorizeResponseGenerator authorizeResponseGenerator,
        IUserSession userSession,
        IConsentMessageStore consentResponseStore,
        IAuthorizationParametersMessageStore authorizationParametersMessageStore = null)
        : base(events, logger, options, validator, interactionGenerator, authorizeResponseGenerator, userSession)
    {
        _consentResponseStore = consentResponseStore;
        _authorizationParametersMessageStore = authorizationParametersMessageStore;
    }

    public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
    {
        if (!HttpMethods.IsGet(context.Request.Method))
        {
            Logger.LogWarning("Invalid HTTP method for authorize endpoint.");
            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }

        Logger.LogDebug("Start authorize callback request");

        var parameters = context.Request.Query.AsNameValueCollection();
        if (_authorizationParametersMessageStore != null)
        {
            var messageStoreId = parameters[Constants.AuthorizationParamsStore.MessageStoreIdParameterName];
            var entry = await _authorizationParametersMessageStore.ReadAsync(messageStoreId);
            parameters = entry?.Data.FromFullDictionary() ?? new NameValueCollection();

            await _authorizationParametersMessageStore.DeleteAsync(messageStoreId);
        }

        var user = await UserSession.GetUserAsync();
        var consentRequest = new ConsentRequest(parameters, user?.GetSubjectId());
        var consent = await _consentResponseStore.ReadAsync(consentRequest.Id);

        if (consent != null && consent.Data == null)
        {
            return await CreateErrorResultAsync("consent message is missing data");
        }

        try
        {
            var result = await ProcessAuthorizeRequestAsync(parameters, user, consent?.Data);

            Logger.LogTrace("End Authorize Request. Result type: {0}", result?.GetType().ToString() ?? "-none-");

            return result;
        }
        finally
        {
            if (consent != null)
            {
                await _consentResponseStore.DeleteAsync(consentRequest.Id);
            }
        }
    }
}
