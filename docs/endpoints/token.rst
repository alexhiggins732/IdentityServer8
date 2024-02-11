Token Endpoint
==============

The token endpoint can be used to programmatically request tokens.
It supports the ``password``, ``authorization_code``, ``client_credentials``, ``refresh_token`` and ``urn:ietf:params:oauth:grant-type:device_code`` grant types.
Furthermore the token endpoint can be extended to support extension grant types.

.. Note:: IdentityServer supports a subset of the OpenID Connect and OAuth 2.0 token request parameters. For a full list, see `here <http://openid.net/specs/openid-connect-core-1_0.html#TokenRequest>`_.

``client_id``
    client identifier (required – Either in the body or as part of the authorization header.)
``client_secret``
    client secret either in the post body, or as a basic authentication header. Optional.
``grant_type``
    ``authorization_code``, ``client_credentials``, ``password``, ``refresh_token``, ``urn:ietf:params:oauth:grant-type:device_code`` or custom
``scope``
    one or more registered scopes. If not specified, a token for all explicitly allowed scopes will be issued.
``redirect_uri`` 
    required for the ``authorization_code`` grant type
``code``
    the authorization code (required for ``authorization_code`` grant type)
``code_verifier``
    PKCE proof key
``username`` 
    resource owner username (required for ``password`` grant type)
``password``
    resource owner password (required for ``password`` grant type)
``acr_values``
   allows passing in additional authentication related information for the ``password`` grant type - identityserver special cases the following proprietary acr_values:
        
        ``idp:name_of_idp`` bypasses the login/home realm screen and forwards the user directly to the selected identity provider (if allowed per client configuration)
        
        ``tenant:name_of_tenant`` can be used to pass a tenant name to the token endpoint
``refresh_token``
    the refresh token (required for ``refresh_token`` grant type)
``device_code``
    the device code (required for ``urn:ietf:params:oauth:grant-type:device_code`` grant type)

Example
^^^^^^^

::

    POST /connect/token
    CONTENT-TYPE application/x-www-form-urlencoded

        client_id=client1&
        client_secret=secret&
        grant_type=authorization_code&
        code=hdh922&
        redirect_uri=https://myapp.com/callback

(Form-encoding removed and line breaks added for readability)

.. Note:: You can use the `IdentityModel <https://github.com/IdentityModel/IdentityModel>`_ client library to programmatically access the token endpoint from .NET code. For more information check the IdentityModel `docs <https://identitymodel.readthedocs.io/en/latest/client/token.html>`_.
