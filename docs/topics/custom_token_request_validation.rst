.. _refCustomTokenRequestValidation:
Custom Token Request Validation and Issuance
============================================

You can run custom code as part of the token issuance pipeline at the token endpoint.
This allows e.g. for

* adding additional validation logic
* changing certain parameters (e.g. token lifetime) dynamically

For this purpose, implement (and register) the ``ICustomTokenRequestValidator`` interface::

    /// <summary>
    /// Allows inserting custom validation logic into token requests
    /// </summary>
    public interface ICustomTokenRequestValidator
    {
        /// <summary>
        /// Custom validation logic for a token request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The validation result
        /// </returns>
        Task ValidateAsync(CustomTokenRequestValidationContext context);
    }

The context object gives you access to:

* adding custom response parameters
* return an error and error description
* modifying the request parameters, e.g. access token lifetime and type, client claims, and the confirmation method

You can register your implementation of the validator using the ``AddCustomTokenRequestValidator`` extension method on the configuration builder.
