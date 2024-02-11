.. _refGrantValidationResult:
GrantValidationResult
=====================

The ``GrantValidationResult`` class models the outcome of grant validation for extensions grants and resource owner password grants.

The most common usage is to either new it up using an identity (success case)::

    context.Result = new GrantValidationResult(
        subject: "818727", 
        authenticationMethod: "custom", 
        claims: optionalClaims);

...or using an error and description (failure case)::

    context.Result = new GrantValidationResult(
        TokenRequestErrors.InvalidGrant, 
        "invalid custom credential");

In both case you can pass additional custom values that will be included in the token response.