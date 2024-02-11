.. _refClient:
Client
======
The ``Client`` class models an OpenID Connect or OAuth 2.0 client - 
e.g. a native application, a web application or a JS-based application.

Basics
^^^^^^
``Enabled``
    Specifies if client is enabled. Defaults to `true`.
``ClientId``
    Unique ID of the client
``ClientSecrets``
    List of client secrets - credentials to access the token endpoint.
``RequireClientSecret``
    Specifies whether this client needs a secret to request tokens from the token endpoint (defaults to ``true``)
``RequireRequestObject``
    Specifies whether this client needs to wrap the authorize request parameters in a JWT (defaults to ``false``)
``AllowedGrantTypes``
    Specifies the grant types the client is allowed to use. Use the ``GrantTypes`` class for common combinations.
``RequirePkce``
    Specifies whether clients using an authorization code based grant type must send a proof key (defaults to ``true``).
``AllowPlainTextPkce``
    Specifies whether clients using PKCE can use a plain text code challenge (not recommended - and default to ``false``)
``RedirectUris``
    Specifies the allowed URIs to return tokens or authorization codes to
``AllowedScopes``
    By default a client has no access to any resources - specify the allowed resources by adding the corresponding scopes names
``AllowOfflineAccess``
    Specifies whether this client can request refresh tokens (be requesting the ``offline_access`` scope)
``AllowAccessTokensViaBrowser``
    Specifies whether this client is allowed to receive access tokens via the browser. 
    This is useful to harden flows that allow multiple response types 
    (e.g. by disallowing a hybrid flow client that is supposed to use `code id_token` to add the `token` response type 
    and thus leaking the token to the browser.
``Properties``
    Dictionary to hold any custom client-specific values as needed.

Authentication/Logout
^^^^^^^^^^^^^^^^^^^^^
``PostLogoutRedirectUris``
    Specifies allowed URIs to redirect to after logout. See the `OIDC Connect Session Management spec <https://openid.net/specs/openid-connect-session-1_0.html>`_ for more details.
``FrontChannelLogoutUri``
    Specifies logout URI at client for HTTP based front-channel logout. See the `OIDC Front-Channel spec <https://openid.net/specs/openid-connect-frontchannel-1_0.html>`_ for more details.
``FrontChannelLogoutSessionRequired``
    Specifies if the user's session id should be sent to the FrontChannelLogoutUri. Defaults to true.
``BackChannelLogoutUri``
    Specifies logout URI at client for HTTP based back-channel logout. See the `OIDC Back-Channel spec <https://openid.net/specs/openid-connect-backchannel-1_0.html>`_ for more details.
``BackChannelLogoutSessionRequired``
    Specifies if the user's session id should be sent in the request to the BackChannelLogoutUri. Defaults to true.
``EnableLocalLogin``
    Specifies if this client can use local accounts, or external IdPs only. Defaults to `true`.
``IdentityProviderRestrictions``
    Specifies which external IdPs can be used with this client (if list is empty all IdPs are allowed). Defaults to empty.
``UserSsoLifetime`` `added in 2.3`
    The maximum duration (in seconds) since the last time the user authenticated. Defaults to ``null``.
    You can adjust the lifetime of a session token to control when and how often a user is required to reenter credentials instead of being silently authenticated, when using a web application.

Token
^^^^^
``IdentityTokenLifetime``
    Lifetime to identity token in seconds (defaults to 300 seconds / 5 minutes)
``AllowedIdentityTokenSigningAlgorithms``
    List of allowed signing algorithms for identity token. If empty, will use the server default signing algorithm.
``AccessTokenLifetime``
    Lifetime of access token in seconds (defaults to 3600 seconds / 1 hour)
``AuthorizationCodeLifetime``
    Lifetime of authorization code in seconds (defaults to 300 seconds / 5 minutes)
``AbsoluteRefreshTokenLifetime``
    Maximum lifetime of a refresh token in seconds. Defaults to 2592000 seconds / 30 days
``SlidingRefreshTokenLifetime``
    Sliding lifetime of a refresh token in seconds. Defaults to 1296000 seconds / 15 days
``RefreshTokenUsage``
    ``ReUse`` the refresh token handle will stay the same when refreshing tokens
    
    ``OneTime`` the refresh token handle will be updated when refreshing tokens. This is the default.
``RefreshTokenExpiration``
    ``Absolute`` the refresh token will expire on a fixed point in time (specified by the AbsoluteRefreshTokenLifetime). This is the default.
    
    ``Sliding`` when refreshing the token, the lifetime of the refresh token will be renewed (by the amount specified in SlidingRefreshTokenLifetime). The lifetime will not exceed `AbsoluteRefreshTokenLifetime`.
``UpdateAccessTokenClaimsOnRefresh``
    Gets or sets a value indicating whether the access token (and its claims) should be updated on a refresh token request.
``AccessTokenType``
    Specifies whether the access token is a reference token or a self contained JWT token (defaults to `Jwt`).
``IncludeJwtId``
    Specifies whether JWT access tokens should have an embedded unique ID (via the `jti` claim). Defaults to ``true``.
``AllowedCorsOrigins``
    If specified, will be used by the default CORS policy service implementations (In-Memory and EF) to build a CORS policy for JavaScript clients.
``Claims``
    Allows settings claims for the client (will be included in the access token).
``AlwaysSendClientClaims``
    If set, the client claims will be sent for every flow. If not, only for client credentials flow (default is `false`)
``AlwaysIncludeUserClaimsInIdToken``
    When requesting both an id token and access token, should the user claims always be added to the id token instead of requiring the client to use the userinfo endpoint. Default is `false`.
``ClientClaimsPrefix``
    If set, the prefix client claim types will be prefixed with. Defaults to `client_`. The intent is to make sure they don't accidentally collide with user claims.
``PairWiseSubjectSalt``
    Salt value used in pair-wise subjectId generation for users of this client.

Consent Screen
^^^^^^^^^^^^^^
``RequireConsent``
    Specifies whether a consent screen is required. Defaults to ``false``.
``AllowRememberConsent``
    Specifies whether user can choose to store consent decisions. Defaults to ``true``.
``ConsentLifetime``
    Lifetime of a user consent in seconds. Defaults to null (no expiration).
``ClientName``
    Client display name (used for logging and consent screen)
``ClientUri``
    URI to further information about client (used on consent screen)
``LogoUri``
    URI to client logo (used on consent screen)

Device flow
^^^^^^^^^^^
``UserCodeType``
    Specifies the type of user code to use for the client. Otherwise falls back to default.
``DeviceCodeLifetime``
    Lifetime to device code in seconds (defaults to 300 seconds / 5 minutes)
