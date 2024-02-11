.. _refSignOutFederated:
Federated Sign-out
==================

Federated sign-out is the situation where a user has used an external identity provider to log into IdentityServer, and then the user logs out of that external identity provider via a workflow unknown to IdentityServer.
When the user signs out, it will be useful for IdentityServer to be notified so that it can sign the user out of IdentityServer and all of the applications that use IdentityServer.

Not all external identity providers support federated sign-out, but those that do will provide a mechanism to notify clients that the user has signed out.
This notification usually comes in the form of a request in an ``<iframe>`` from the external identity provider's "logged out" page.
IdentityServer must then notify all of its clients (as discussed :ref:`here <refSignOut>`), also typically in the form of a request in an ``<iframe>`` from within the external identity provider's ``<iframe>``.

What makes federated sign-out a special case (when compared to a normal :ref:`sign-out <refSignOut>`) is that the federated sign-out request is not to the normal sign-out endpoint in IdentityServer.
In fact, each external IdentityProvider will have a different endpoint into your IdentityServer host. 
This is due to that fact that each external identity provider might use a different protocol, and each middleware listens on different endpoints.

The net effect of all of these factors is that there is no "logged out" page being rendered as we would on the normal sign-out workflow, 
which means we are missing the sign-out notifications to IdentityServer's clients.
We must add code for each of these federated sign-out endpoints to render the necessary notifications to achieve federated sign-out.

Fortunately IdentityServer already contains this code. 
When requests come into IdentityServer and invoke the handlers for external authentication providers, IdentityServer detects if these are federated signout requests and if they are it will automatically render the same ``<iframe>`` as :ref:`described here for signout <refSignOut>`.
In short, federated signout is automatically supported.
