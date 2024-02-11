Introspection Endpoint
======================

The introspection endpoint is an implementation of `RFC 7662 <https://tools.ietf.org/html/rfc7662>`_.

It can be used to validate reference tokens (or JWTs if the consumer does not have support for appropriate JWT or cryptographic libraries).
The introspection endpoint requires authentication - since the client of an introspection endpoint is an API, you configure the secret on the ``ApiResource``.

Example
^^^^^^^

::


    POST /connect/introspect
    Authorization: Basic xxxyyy

    token=<token>


A successful response will return a status code of 200 and either an active or inactive token::


    {
        "active": true,
        "sub": "123"
    }


Unknown or expired tokens will be marked as inactive::


    {
        "active": false,
    }


An invalid request will return a 400, an unauthorized request 401.

.. Note:: You can use the `IdentityModel <https://github.com/IdentityModel/IdentityModel2>`_ client library to programmatically access the introspection endpoint from .NET code. For more information check the IdentityModel `docs <https://identitymodel.readthedocs.io/en/latest/client/introspection.html>`_.