Device Authorization Endpoint
=============================

The device authorization endpoint can be used to request device and user codes.
This endpoint is used to start the device flow authorization process.

.. Note:: The URL for the end session endpoint is available via the :ref:`discovery endpoint <refDiscovery>`.

``client_id``
    client identifier (required)
``client_secret``
    client secret either in the post body, or as a basic authentication header. Optional.
``scope``
    one or more registered scopes. If not specified, a token for all explicitly allowed scopes will be issued.

Example
^^^^^^^

::

    POST /connect/deviceauthorization

        client_id=client1&
        client_secret=secret&
        scope=openid api1

(Form-encoding removed and line breaks added for readability)

.. Note:: You can use the `IdentityModel <https://github.com/IdentityModel/IdentityModel2>`_ client library to programmatically access the device authorization endpoint from .NET code. For more information check the IdentityModel `docs <https://identitymodel.readthedocs.io/en/latest/client/device_authorize.html>`_.