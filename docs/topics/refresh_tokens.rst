Refresh Tokens
==============
Since access tokens have finite lifetimes, refresh tokens allow requesting new access tokens without user interaction.

Refresh tokens are supported for the following flows: authorization code, hybrid and resource owner password credential flow.
The clients needs to be explicitly authorized to request refresh tokens by setting ``AllowOfflineAccess`` to ``true``.

Additional client settings
^^^^^^^^^^^^^^^^^^^^^^^^^^
``AbsoluteRefreshTokenLifetime``
    Maximum lifetime of a refresh token in seconds. Defaults to 2592000 seconds / 30 days. Zero allows refresh tokens that, when used with ``RefreshTokenExpiration = Sliding`` only expire after the SlidingRefreshTokenLifetime is passed.
``SlidingRefreshTokenLifetime``
    Sliding lifetime of a refresh token in seconds. Defaults to 1296000 seconds / 15 days
``RefreshTokenUsage``
    ``ReUse`` the refresh token handle will stay the same when refreshing tokens
    
    ``OneTimeOnly`` the refresh token handle will be updated when refreshing tokens
``RefreshTokenExpiration``
    ``Absolute`` the refresh token will expire on a fixed point in time (specified by the AbsoluteRefreshTokenLifetime). This is the default.
    
    ``Sliding`` when refreshing the token, the lifetime of the refresh token will be renewed (by the amount specified in SlidingRefreshTokenLifetime). The lifetime will not exceed `AbsoluteRefreshTokenLifetime`.
``UpdateAccessTokenClaimsOnRefresh``
    Gets or sets a value indicating whether the access token (and its claims) should be updated on a refresh token request.

.. note:: Public clients (clients without a client secret) should rotate their refresh tokens. Set the ``RefreshTokenUsage`` to ``OneTimeOnly``.

Requesting a refresh token
^^^^^^^^^^^^^^^^^^^^^^^^^^
You can request a refresh token by adding a scope called ``offline_access`` to the scope parameter.

Requesting an access token using a refresh token
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
To get a new access token, you send the refresh token to the token endpoint.
This will result in a new token response containing a new access token and its expiration and potentially also a new refresh token depending on the client configuration (see above).

::

    POST /connect/token

        client_id=client&
        client_secret=secret&
        grant_type=refresh_token&
        refresh_token=hdh922
        
(Form-encoding removed and line breaks added for readability)

.. Note:: You can use the `IdentityModel <https://github.com/IdentityModel/IdentityModel>`_ client library to programmatically access the token endpoint from .NET code. For more information check the IdentityModel `docs <https://identitymodel.readthedocs.io/en/latest/client/token.html>`_.

.. Note:: The refresh token, must be valid or an invalid_grant error is returned.  By default, a refresh_token can only be used once.  Using an already used refresh_token will result in an invalid_grant error.

Customizing refresh token behavior
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
All refresh token handling is implemented in the ``DefaultRefreshTokenService`` (which is the default implementation of the ``IRefreshTokenService`` interface)::

    public interface IRefreshTokenService
    {
        /// <summary>
        /// Validates a refresh token.
        /// </summary>
        Task<TokenValidationResult> ValidateRefreshTokenAsync(string token, Client client);
        
        /// <summary>
        /// Creates the refresh token.
        /// </summary>
        Task<string> CreateRefreshTokenAsync(ClaimsPrincipal subject, Token accessToken, Client client);

        /// <summary>
        /// Updates the refresh token.
        /// </summary>
        Task<string> UpdateRefreshTokenAsync(string handle, RefreshToken refreshToken, Client client);
    }

The logic around refresh token handling is pretty involved, and we don't recommend implementing the interface from scratch,
unless you exactly know what you are doing.
If you want to customize certain behavior, it is more recommended to derive from the default implementation and call the base checks first.

The most common customization that you probably want to do is how to deal with refresh token replays.
This is for situations where the token usage has been set to one-time only, but the same token gets sent more than once.
This could either point to a replay attack of the refresh token, or to faulty client code like logic bugs or race conditions.

It is important to note, that a refresh token is never deleted in the database. 
Once it has been used, the ``ConsumedTime`` property will be set.
If a token is received that has already been consumed, the default service will call a virtual method called ``AcceptConsumedTokenAsync``.

The default implementation will reject the request, but here you can implement custom logic like grace periods, 
or revoking additional refresh or access tokens.
