Device Flow Interaction Service
==================================

The ``IDeviceFlowInteractionService`` interface is intended to provide services to be used by the user interface to communicate with IdentityServer during device flow authorization.
It is available from the dependency injection system and would normally be injected as a constructor parameter into your MVC controllers for the user interface of IdentityServer.

IDeviceFlowInteractionService APIs
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

``GetAuthorizationContextAsync``
    Returns the ``DeviceFlowAuthorizationRequest`` based on the ``userCode`` passed to the login or consent pages.

``DeviceFlowInteractionResult``
    Completes device authorization for the given ``userCode``.

DeviceFlowAuthorizationRequest
^^^^^^^^^^^^^^^^^^^^
``ClientId``
    The client identifier that initiated the request.
``ScopesRequested``
    The scopes requested from the authorization request.

DeviceFlowInteractionResult
^^^^^^^^^^^^^^^^^^^^^^^^^^^
``IsError``
    Specifies if the authorization request errored.
``ErrorDescription``
    Error description upon failure.
