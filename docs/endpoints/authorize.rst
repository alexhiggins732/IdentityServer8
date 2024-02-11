Authorize Endpoint
==================

The authorize endpoint can be used to request tokens or authorization codes via the browser.
This process typically involves authentication of the end-user and optionally consent.

.. Note:: IdentityServer supports a subset of the OpenID Connect and OAuth 2.0 authorize request parameters. For a full list, see `here <https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest>`_.

``client_id``
    identifier of the client (required).
``request``
    instead of providing all parameters as individual query string parameters, you can provide a subset or all of them as a JWT
``request_uri``
    URL of a pre-packaged JWT containing request parameters
``scope``
    one or more registered scopes (required)
``redirect_uri`` 
    must exactly match one of the allowed redirect URIs for that client (required)
``response_type`` 
    ``id_token`` requests an identity token (only identity scopes are allowed)

    ``token`` requests an access token (only resource scopes are allowed)

    ``id_token token`` requests an identity token and an access token

    ``code`` requests an authorization code

    ``code id_token`` requests an authorization code and identity token

    ``code id_token token`` requests an authorization code, identity token and access token
    
``response_mode``
    ``form_post`` sends the token response as a form post instead of a fragment encoded redirect (optional)
``state`` 
    identityserver will echo back the state value on the token response, 
    this is for round tripping state between client and provider, correlating request and response and CSRF/replay protection. (recommended)
``nonce`` 
    identityserver will echo back the nonce value in the identity token, this is for replay protection)

    *Required for identity tokens via implicit grant.*
``prompt``
    ``none`` no UI will be shown during the request. If this is not possible (e.g. because the user has to sign in or consent) an error is returned
    
    ``login`` the login UI will be shown, even if the user is already signed-in and has a valid session
``code_challenge``
    sends the code challenge for PKCE
``code_challenge_method``
    ``plain`` indicates that the challenge is using plain text (not recommended)
    ``S256`` indicates the challenge is hashed with SHA256
``login_hint``
    can be used to pre-fill the username field on the login page
``ui_locales``
    gives a hint about the desired display language of the login UI
``max_age``
    if the user's logon session exceeds the max age (in seconds), the login UI will be shown
``acr_values``
    allows passing in additional authentication related information - identityserver special cases the following proprietary acr_values:
        
        ``idp:name_of_idp`` bypasses the login/home realm screen and forwards the user directly to the selected identity provider (if allowed per client configuration)
        
        ``tenant:name_of_tenant`` can be used to pass a tenant name to the login UI

**Example**

::

    GET /connect/authorize?
        client_id=client1&
        scope=openid email api1&
        response_type=id_token token&
        redirect_uri=https://myapp/callback&
        state=abc&
        nonce=xyz 

(URL encoding removed, and line breaks added for readability)

.. Note:: You can use the `IdentityModel <https://github.com/IdentityModel/IdentityModel2>`_ client library to programmatically create authorize requests .NET code. For more information check the IdentityModel `docs <https://identitymodel.readthedocs.io/en/latest/client/authorize.html>`_.
