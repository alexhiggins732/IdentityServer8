Adding new Protocols
====================

IdentityServer8 allows adding support for other protocols besides the built-in 
support for OpenID Connect and OAuth 2.0.

You can add those additional protocol endpoints either as middleware or using e.g. MVC controllers.
In both cases you have access to the ASP.NET Core DI system which allows re-using our
internal services like access to client definitions or key material.

A sample for adding WS-Federation support can be found `here <https://github.com/IdentityServer/IdentityServer8.WsFederation>`_.

Typical authentication workflow
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
An authentication request typically works like this:

* authentication request arrives at protocol endpoint
* protocol endpoint does input validation
* redirection to login page with a return URL set back to protocol endpoint (if user is anonymous)
    * access to current request details via the ``IIdentityServerInteractionService``
    * authentication of user (either locally or via external authentication middleware)
    * signing in the user
    * redirect back to protocol endpoint
* creation of protocol response (token creation and redirect back to client)

Useful IdentityServer services
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
To achieve the above workflow, some interaction points with IdentityServer are needed.

**Access to configuration and redirecting to the login page**

You can get access to the IdentityServer configuration by injecting the ``IdentityServerOptions``
class into your code. This, e.g. has the configured path to the login page::

    var returnUrl = Url.Action("Index");
    returnUrl = returnUrl.AddQueryString(Request.QueryString.Value);

    var loginUrl = _options.UserInteraction.LoginUrl;
    var url = loginUrl.AddQueryString(_options.UserInteraction.LoginReturnUrlParameter, returnUrl);

    return Redirect(url);

**Interaction between the login page and current protocol request**

The ``IIdentityServerInteractionService`` supports turning a protocol return URL into a 
parsed and validated context object::

    var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

By default the interaction service only understands OpenID Connect protocol messages.
To extend support, you can write your own ``IReturnUrlParser``::

    public interface IReturnUrlParser
    {
        bool IsValidReturnUrl(string returnUrl);
        Task<AuthorizationRequest> ParseAsync(string returnUrl);
    }

..and then register the parser in DI::

    builder.Services.AddTransient<IReturnUrlParser, WsFederationReturnUrlParser>();

This allows the login page to get to information like the client configuration and other 
protocol parameters.

**Access to configuration and key material for creating the protocol response**

By injecting the ``IKeyMaterialService`` into your code, you get access to the configured 
signing credential and validation keys::

    var credential = await _keys.GetSigningCredentialsAsync();
    var key = credential.Key as Microsoft.IdentityModel.Tokens.X509SecurityKey; 
        
    var descriptor = new SecurityTokenDescriptor
    {
        AppliesToAddress = result.Client.ClientId,
        Lifetime = new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddSeconds(result.Client.IdentityTokenLifetime)),
        ReplyToAddress = result.Client.RedirectUris.First(),
        SigningCredentials = new X509SigningCredentials(key.Certificate, result.RelyingParty.SignatureAlgorithm, result.RelyingParty.DigestAlgorithm),
        Subject = outgoingSubject,
        TokenIssuerName = _contextAccessor.HttpContext.GetIdentityServerIssuerUri(),
        TokenType = result.RelyingParty.TokenType
    };