.. _refResourceOwnerPasswordValidator:
Resource Owner Password Validation
===================================

If you want to use the OAuth 2.0 resource owner password credential grant (aka ``password``), you need to implement and register the ``IResourceOwnerPasswordValidator`` interface::

    public interface IResourceOwnerPasswordValidator
    {
        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        Task ValidateAsync(ResourceOwnerPasswordValidationContext context);
    }

On the context you will find already parsed protocol parameters like ``UserName`` and ``Password``, but also the raw request if you want to look at other input data.

Your job is then to implement the password validation and set the ``Result`` on the context accordingly. See the :ref:`GrantValidationResult <refGrantValidationResult>` documentation.