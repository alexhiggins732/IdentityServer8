.. _refEndSession:
End Session Endpoint
====================

The end session endpoint can be used to trigger single sign-out (see `spec <https://openid.net/specs/openid-connect-rpinitiated-1_0.html>`_).

To use the end session endpoint a client application will redirect the user's browser to the end session URL.
All applications that the user has logged into via the browser during the user's session can participate in the sign-out.

.. Note:: The URL for the end session endpoint is available via the :ref:`discovery endpoint <refDiscovery>`.

Parameters
^^^^^^^^^^

**id_token_hint**

When the user is redirected to the endpoint, they will be prompted if they really want to sign-out. 
This prompt can be bypassed by a client sending the original *id_token* received from authentication.
This is passed as a query string parameter called ``id_token_hint``.

**post_logout_redirect_uri**

If a valid ``id_token_hint`` is passed, then the client may also send a ``post_logout_redirect_uri`` parameter.
This can be used to allow the user to redirect back to the client after sign-out.
The value must match one of the client's pre-configured `PostLogoutRedirectUris` (:ref:`client docs <refClient>`).

**state**

If a valid ``post_logout_redirect_uri`` is passed, then the client may also send a ``state`` parameter.
This will be returned back to the client as a query string parameter after the user redirects back to the client.
This is typically used by clients to round-trip state across the redirect.

Example
^^^^^^^

::

    GET /connect/endsession?id_token_hint=eyJhbGciOiJSUzI1NiIsImtpZCI6IjdlOGFkZmMzMjU1OTEyNzI0ZDY4NWZmYmIwOThjNDEyIiwidHlwIjoiSldUIn0.eyJuYmYiOjE0OTE3NjUzMjEsImV4cCI6MTQ5MTc2NTYyMSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoianNfb2lkYyIsIm5vbmNlIjoiYTQwNGFjN2NjYWEwNGFmNzkzNmJjYTkyNTJkYTRhODUiLCJpYXQiOjE0OTE3NjUzMjEsInNpZCI6IjI2YTYzNWVmOTQ2ZjRiZGU3ZWUzMzQ2ZjFmMWY1NTZjIiwic3ViIjoiODg0MjExMTMiLCJhdXRoX3RpbWUiOjE0OTE3NjUzMTksImlkcCI6ImxvY2FsIiwiYW1yIjpbInB3ZCJdfQ.STzOWoeVYMtZdRAeRT95cMYEmClixWkmGwVH2Yyiks9BETotbSZiSfgE5kRh72kghN78N3-RgCTUmM2edB3bZx4H5ut3wWsBnZtQ2JLfhTwJAjaLE9Ykt68ovNJySbm8hjZhHzPWKh55jzshivQvTX0GdtlbcDoEA1oNONxHkpDIcr3pRoGi6YveEAFsGOeSQwzT76aId-rAALhFPkyKnVc-uB8IHtGNSyRWLFhwVqAdS3fRNO7iIs5hYRxeFSU7a5ZuUqZ6RRi-bcDhI-djKO5uAwiyhfpbpYcaY_TxXWoCmq8N8uAw9zqFsQUwcXymfOAi2UF3eFZt02hBu-shKA&post_logout_redirect_uri=http%3A%2F%2Flocalhost%3A7017%2Findex.html

.. Note:: You can use the `IdentityModel <https://github.com/IdentityModel/IdentityModel2>`_ client library to programmatically create end_session requests .NET code. For more information check the IdentityModel `docs <https://identitymodel.readthedocs.io/en/latest/client/end_session.html>`_.
