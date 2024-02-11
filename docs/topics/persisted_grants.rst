.. _refPersistedGrants:
Persisted Grants
================
Many grant types require persistence in IdentityServer.
These include authorization codes, refresh tokens, reference tokens, and remembered user consents.
Internally in IdentityServer, the default storage for these grants is in a common store called the persisted grants store.

Persisted Grant
^^^^^^^^^^^^^^^
The persisted grant is the data type that maintains the values for a grant. 
It has these properties:

``Key``
    The unique identifier for the persisted grant in the store.
``Type``
    The type of the grant.
``SubjectId``
    The subject id to which the grant belongs.
``ClientId``
    The client identifier for which the grant was created.
``Description``
    The description the user assigned to the grant or device being authorized.
``CreationTime``
    The date/time the grant was created.
``Expiration``
    The expiration of the grant.
``ConsumedTime``
    The date/time the grant was "consumed" (see below).
``Data``
    The grant specific serialized data.

.. note:: The ``Data`` property contains a copy of all of the values (and more) and is considered authoritative by IdentityServer, thus the above values, by default, are considered informational and read-only.

The presence of the record in the store without a ``ConsumedTime`` and while still within the ``Expiration`` represents the validity of the grant.
Setting either of these two values, or removing the record from the store effectively revokes the grant.

Grant Consumption
^^^^^^^^^^^^^^^^^
Some grant types are one-time use only (either by definition or configuration).
Once they are "used", rather than deleting the record, the ``ConsumedTime`` value is set in the database marking them as having been used.
This "soft delete" allows for custom implementations to either have flexibility in allowing a grant to be re-used (typically within a short window of time),
or to be used in risk assessment and threat mitigation scenarios (where suspicious activity is detected) to revoke access.
For refresh tokens, this sort of custom logic would be performed in the ``IRefreshTokenService``.

Persisted Grant Service
^^^^^^^^^^^^^^^^^^^^^^^
Working with the grants store directly might be too low level. 
As such, a higher level service called ``IPersistedGrantService`` is provided.
It abstracts and aggregates the different grant types into one concept, and allows querying and revoking the persisted grants for a user.

It contains these APIs:

``GetAllGrantsAsync``
    Gets all the grants for a user based upon subject id. 
``RemoveAllGrantsAsync``
    Removes grants from the store based on the subject id and optionally a client id and/or a session id.
