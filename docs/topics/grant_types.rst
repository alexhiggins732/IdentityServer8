.. _refGrantTypes:
Grant Types
^^^^^^^^^^^
The OpenID Connect and OAuth 2.0 specifications define so-called grant types (often also called flows - or protocol flows).
Grant types specify how a client can interact with the token service.

You need to specify which grant types a client can use via the ``AllowedGrantTypes`` property on the ``Client`` configuration.
This allows locking down the protocol interactions that are allowed for a given client.

A client can be configured to use more than a single grant type (e.g. Authorization Code flow for user centric operations and client credentials for server to server communication).
The ``GrantTypes`` class can be used to pick from typical grant type combinations::

    Client.AllowedGrantTypes = GrantTypes.CodeAndClientCredentials;

You can also specify the grant types list manually::

    Client.AllowedGrantTypes = 
    {
        GrantType.Code, 
        GrantType.ClientCredentials,
        "my_custom_grant_type" 
    };

While IdentityServer supports all standard grant types, you really only need to know two of them for common application scenarios.

Machine to Machine Communication
================================
This is the simplest type of communication. Tokens are always requested on behalf of a client, no interactive user is present.

In this scenario, you send a token request to the token endpoint using the ``client credentials`` grant type.
The client typically has to authenticate with the token endpoint using its client ID and secret.

See the :ref:`Client Credentials Quick Start <refClientCredentialsQuickstart>` for a sample how to use it. 

Interactive Clients
===================
This is the most common type of client scenario: web applications, SPAs or native/mobile apps with interactive users.

.. Note:: Feel free to skip to the summary, if you don't care about all the technical details.

For this type of clients, the ``authorization code`` flow was designed. That flow consists of two physical operations:

* a front-channel step via the browser where all "interactive" things happen, e.g. login page, consent etc. This step results in an authorization code that represents the outcome of the front-channel operation.
* a back-channel step where the authorization code from step 1 gets exchanged with the requested tokens. Confidential clients need to authenticate at this point.

This flow has the following security properties:

* no data (besides the authorization code which is basically a random string) gets leaked over the browser channel
* authorization codes can only be used once
* the authorization code can only be turned into tokens when (for confidential clients - more on that later) the client secret is known

This sounds all very good - still there is one problem called `code substitution attack <https://nat.sakimura.org/2016/01/25/cut-and-pasted-code-attack-in-oauth-2-0-rfc6749/>`_.
There are two modern mitigation techniques for this:

**OpenID Connect Hybrid Flow**

This uses a response type of ``code id_token`` to add an additional identity token to the response. This token is signed and protected against substitution.
In addition it contains the hash of the code via the ``c_hash`` claim. This allows checking that you indeed got the right code (experts call this a detached signature).

This solves the problem but has the following down-sides:

* the ``id_token`` gets transmitted over the front-channel and might leak additional (personal identifiable) data
* all the mitigation steps (e.g. crypto) need to be implemented by the client. This results in more complicated client library implementations.

**RFC 7636 - Proof Key for Code Exchange (PKCE)**

This essentially introduces a per-request secret for code flow (please read up on the details `here <https://tools.ietf.org/html/rfc7636>`_).
All the client has to implement for this, is creating a random string and hashing it using SHA256.

This also solves the substitution problem, because the client can prove that it is the same client on front and back-channel, and has the following additional advantages:

* the client implementation is very simple compared to hybrid flow
* it also solves the problem of the absence of a static secret for public clients
* no additional front-channel response artifacts are needed

**Summary**

Interactive clients should use an authorization code-based flow. To protect against code substitution, either hybrid flow or PKCE should be used.
If PKCE is available, this is the simpler solution to the problem.

PKCE is already the official recommendation for `native <https://tools.ietf.org/html/rfc8252#section-6>`_ applications 
and `SPAs <https://tools.ietf.org/html/draft-ietf-oauth-browser-based-apps-03#section-4>`_ - and with the release of ASP.NET Core 3 also by default supported in the OpenID Connect handler as well.

This is how you would configure an interactive client::

    var client = new Client
    {
        ClientId = "...",

        // set client secret for confidential clients
        ClientSecret = { ... },

        // ...or turn off for public clients
        RequireClientSecret = false,

        AllowedGrantTypes = GrantTypes.Code,
        RequirePkce = true
    };


Interactive clients without browsers or with constrained input devices
======================================================================
This grant type is detailed `RFC 8628 <https://tools.ietf.org/html/rfc8628>`_.

This flow outsources user authentication and consent to an external device (e.g. a smart phone).
It is typically used by devices that don't have proper keyboards (e.g. TVs, gaming consoles...) and can request both identity and API resources.

Custom scenarios
================
Extension grants allow extending the token endpoint with new grant types. See :ref:`this <refExtensionGrants>` for more details. 
