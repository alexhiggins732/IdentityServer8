/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Hosting.LocalApiAuthentication;

/// <summary>
/// Authentication handler for validating access token from the local IdentityServer
/// </summary>
public class LocalApiAuthenticationHandler : AuthenticationHandler<LocalApiAuthenticationOptions>
{
    private readonly ITokenValidator _tokenValidator;
    private readonly ILogger _logger;

    /// <inheritdoc />
    public LocalApiAuthenticationHandler(IOptionsMonitor<LocalApiAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ITokenValidator tokenValidator)
        : base(options, logger, encoder, clock)
    {
        _tokenValidator = tokenValidator;
        _logger = logger.CreateLogger<LocalApiAuthenticationHandler>();
    }

    /// <summary>
    /// The handler calls methods on the events which give the application control at certain points where processing is occurring. 
    /// If it is not provided a default instance is supplied which does nothing when the methods are called.
    /// </summary>
    protected new LocalApiAuthenticationEvents Events
    {
        get => (LocalApiAuthenticationEvents)base.Events;
        set => base.Events = value;
    }

    /// <inheritdoc/>
    protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new LocalApiAuthenticationEvents());

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        _logger.LogTrace("HandleAuthenticateAsync called");

        string token = null;

        string authorization = Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(authorization))
        {
            return AuthenticateResult.NoResult();
        }

        if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authorization.Substring("Bearer ".Length).Trim();
        }

        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.Fail("No Access Token is sent.");
        }

        _logger.LogTrace("Token found: {token}", Ioc.Sanitizer.Log.Sanitize(token));

        TokenValidationResult result = await _tokenValidator.ValidateAccessTokenAsync(token, Options.ExpectedScope);

        if (result.IsError)
        {
            _logger.LogTrace("Failed to validate the token");

            return AuthenticateResult.Fail(result.Error);
        }

        _logger.LogTrace("Successfully validated the token.");

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(result.Claims, Scheme.Name, JwtClaimTypes.Name, JwtClaimTypes.Role);
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        AuthenticationProperties authenticationProperties = new AuthenticationProperties();

        if (Options.SaveToken)
        {
            authenticationProperties.StoreTokens(new[]
            {
                new AuthenticationToken { Name = "access_token", Value = token }
            });
        }

        var claimsTransformationContext = new ClaimsTransformationContext
        {
            Principal = claimsPrincipal,
            HttpContext = Context
        };

        await Events.ClaimsTransformation(claimsTransformationContext);

        AuthenticationTicket authenticationTicket = new AuthenticationTicket(claimsTransformationContext.Principal, authenticationProperties, Scheme.Name);
        return AuthenticateResult.Success(authenticationTicket);
    }
}
