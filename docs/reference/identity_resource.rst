.. _refIdentityResource:
Identity Resource
=================

This class models an identity resource.

``Enabled``
    Indicates if this resource is enabled and can be requested. Defaults to true.
``Name``
    The unique name of the identity resource. This is the value a client will use for the scope parameter in the authorize request.
``DisplayName``
    This value will be used e.g. on the consent screen.
``Description``
    This value will be used e.g. on the consent screen.
``Required``
    Specifies whether the user can de-select the scope on the consent screen (if the consent screen wants to implement such a feature). Defaults to false.
``Emphasize``
    Specifies whether the consent screen will emphasize this scope (if the consent screen wants to implement such a feature). Use this setting for sensitive or important scopes. Defaults to false.
``ShowInDiscoveryDocument``
    Specifies whether this scope is shown in the discovery document. Defaults to true.
``UserClaims``
    List of associated user claim types that should be included in the identity token.