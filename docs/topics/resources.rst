.. _refResources:
Defining Resources
==================
The ultimate job of an OpenID Connect/OAuth token service is to control access to resources.

The two fundamental resource types in IdentityServer are:

* **identity resources:** represent claims about a user like user ID, display name, email address etc…
* **API resources:** represent functionality a client wants to access. Typically, they are HTTP-based endpoints (aka APIs), but could be also message queuing endpoints or similar.

.. note:: You can define resources using a C# object model - or load them from a data store. An implementation of ``IResourceStore`` deals with these low-level details. For this document we are using the in-memory implementation.

Identity Resources
------------------
An identity resource is a named group of claims that can be requested using the *scope* parameter.

The OpenID Connect specification `suggests <https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims>`_ a couple of standard 
scope name to claim type mappings that might be useful to you for inspiration, but you can freely design them yourself.

One of them is actually mandatory, the *openid* scope, which tells the provider to return the *sub* (subject id) claim in the identity token.

This is how you could define the openid scope in code::

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResource(
                name: "openid",
                userClaims: new[] { "sub" },
                displayName: "Your user identifier")
        };
    }

But since this is one of the standard scopes from the spec you can shorten that to::

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId()
        };
    }

.. note:: see the reference section for more information on ``IdentityResource``.

The following example shows a custom identity resource called *profile* that represents the display name, email address and website claim::

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResource(
                name: "profile",
                userClaims: new[] { "name", "email", "website" },
                displayName: "Your profile data")
        };
    }

Once the resource is defined, you can give access to it to a client via the ``AllowedScopes`` option (other properties omitted)::

    var client = new Client
    {
        ClientId = "client",
        
        AllowedScopes = { "openid", "profile" }
    };


The client can then request the resource using the scope parameter (other parameters omitted)::

    https://demo.identityserver.io/connect/authorize?client_id=client&scope=openid profile

IdentityServer will then use the scope names to create a list of requested claim types, 
and present that to your implementation of the :ref:`profile service <refProfileService>`.

.. _refApiResources:
APIs
----
Designing your API surface can be a complicated task. IdentityServer provides a couple of primitives to help you with that.

The original OAuth 2.0 specification has the concept of scopes, which is just defined as *the scope of access* that the client requests.
Technically speaking, the *scope* parameter is a list of space delimited values - you need to provide the structure and semantics of it.

In more complex systems, often the notion of a *resource* is introduced. This might be e.g. a physical or logical API. 
In turn each API can potentially have scopes as well. Some scopes might be exclusive to that resource, and some scopes might be shared.

Let's start with simple scopes first, and then we'll have a look how resources can help structure scopes.

Scopes
^^^^^^
Let's model something very simple - a system that has three logical operations *read*, *write*, and *delete*.

You can define them using the ``ApiScope`` class::

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new ApiScope(name: "read",   displayName: "Read your data."),
            new ApiScope(name: "write",  displayName: "Write your data."),
            new ApiScope(name: "delete", displayName: "Delete your data.")
        };
    }

You can then assign the scopes to various clients, e.g.::

    var webViewer = new Client
    {
        ClientId = "web_viewer",
        
        AllowedScopes = { "openid", "profile", "read" }
    };

    var mobileApp = new Client
    {
        ClientId = "mobile_app",
        
        AllowedScopes = { "openid", "profile", "read", "write", "delete" }
    }

Authorization based on Scopes
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
When a client asks for a scope (and that scope is allowed via configuration and not denied via consent), 
the value of that scope will be included in the resulting access token as a claim of type *scope* (for both JWTs and introspection), e.g.::

    {
        "typ": "at+jwt"
    }.
    {
        "client_id": "mobile_app",
        "sub": "123",

        "scope": "read write delete"
    }

The consumer of the access token can use that data to make sure that the client is actually allowed to invoke the corresponding functionality.

.. note:: Be aware, that scopes are purely for authorizing clients - not users. IOW - the *write* scope allows the client to invoke the functionality associated with that. Still that client can most probably only write the data the belongs to the current user. This additional user centric authorization is application logic and not covered by OAuth.

You can add more identity information about the user by deriving additional claims from the scope request. The following scope definition tells the configuration system,
that when a *write* scope gets granted, the *user_level* claim should be added to the access token::

    var writeScope = new ApiScope(
        name: "write",
        displayName: "Write your data.",
        userClaims: new[] { "user_level" });

This will pass the *user_level* claim as a requested claim type to the profile service, 
so that the consumer of the access token can use this data as input for authorization decisions or business logic.

.. note:: When using the scope-only model, no aud (audience) claim will be added to the token, since this concept does not apply. If you need an aud claim, you can enable the ``EmitStaticAudience`` setting on the options. This will emit an aud claim in the ``issuer_name/resources`` format. If you need more control of the aud claim, use API resources.

Parameterized Scopes
^^^^^^^^^^^^^^^^^^^^
Sometimes scopes have a certain structure, e.g. a scope name with an additional parameter: *transaction:id* or *read_patient:patientid*.

In this case you would create a scope without the parameter part and assign that name to a client, but in addition provide some logic to parse the structure
of the scope at runtime using the ``IScopeParser`` interface or by deriving from our default implementation, e.g.::

    public class ParameterizedScopeParser : DefaultScopeParser
    {
        public ParameterizedScopeParser(ILogger<DefaultScopeParser> logger) : base(logger)
        {
        }

        public override void ParseScopeValue(ParseScopeContext scopeContext)
        {
            const string transactionScopeName = "transaction";
            const string separator = ":";
            const string transactionScopePrefix = transactionScopeName + separator;

            var scopeValue = scopeContext.RawValue;

            if (scopeValue.StartsWith(transactionScopePrefix))
            {
                // we get in here with a scope like "transaction:something"
                var parts = scopeValue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    scopeContext.SetParsedValues(transactionScopeName, parts[1]);
                }
                else
                {
                    scopeContext.SetError("transaction scope missing transaction parameter value");
                }
            }
            else if (scopeValue != transactionScopeName)
            {
                // we get in here with a scope not like "transaction"
                base.ParseScopeValue(scopeContext);
            }
            else
            {
                // we get in here with a scope exactly "transaction", which is to say we're ignoring it 
                // and not including it in the results
                scopeContext.SetIgnore();
            }
        }
    }

You then have access to the parsed value throughout the pipeline, e.g. in the profile service::

    public class HostProfileService : IProfileService
    {
        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var transaction = context.RequestedResources.ParsedScopes.FirstOrDefault(x => x.ParsedName == "transaction");
            if (transaction?.ParsedParameter != null)
            {
                context.IssuedClaims.Add(new Claim("transaction_id", transaction.ParsedParameter));
            }
        }
    }

API Resources
^^^^^^^^^^^^^
When the API surface gets larger, a flat list of scopes like the one used above might not be feasible.

You typically need to introduce some sort of namespacing to organize the scope names, and maybe you also want to group them together and 
get some higher-level constructs like an *audience* claim in access tokens.
You might also have scenarios, where multiple resources should support the same scope names, whereas sometime you explicitly want to isolate a scope to a certain resource.

In IdentityServer, the ``ApiResource`` class allows some additional organization. Let's use the following scope definition::

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            // invoice API specific scopes
            new ApiScope(name: "invoice.read",   displayName: "Reads your invoices."),
            new ApiScope(name: "invoice.pay",    displayName: "Pays your invoices."),

            // customer API specific scopes
            new ApiScope(name: "customer.read",    displayName: "Reads you customers information."),
            new ApiScope(name: "customer.contact", displayName: "Allows contacting one of your customers."),

            // shared scope
            new ApiScope(name: "manage", displayName: "Provides administrative access to invoice and customer data.")
        };
    }

With ``ApiResource`` you can now create two logical APIs and their corresponding scopes::

    public static readonly IEnumerable<ApiResource> GetApiResources()
    { 
        return new List<ApiResource>
        {
            new ApiResource("invoice", "Invoice API")
            {
                Scopes = { "invoice.read", "invoice.pay", "manage" }
            },
            
            new ApiResource("customer", "Customer API")
            {
                Scopes = { "customer.read", "customer.contact", "manage" }
            }
        };
    }

Using the API resource grouping gives you the following additional features

* support for the JWT *aud* claim. The value(s) of the audience claim will be the name of the API resource(s)
* support for adding common user claims across all contained scopes
* support for introspection by assigning an API secret to the resource
* support for configuring the access token signing algorithm for the resource

Let's have a look at some example access tokens for the above resource configuration.

**Client requests** invoice.read and invoice.pay::

    {
        "typ": "at+jwt"
    }.
    {
        "client_id": "client",
        "sub": "123",

        "aud": "invoice",
        "scope": "invoice.read invoice.pay"
    }

**Client requests** invoice.read and customer.read::

    {
        "typ": "at+jwt"
    }.
    {
        "client_id": "client",
        "sub": "123",

        "aud": [ "invoice", "customer" ]
        "scope": "invoice.read customer.read"
    }

**Client requests** manage::

    {
        "typ": "at+jwt"
    }.
    {
        "client_id": "client",
        "sub": "123",

        "aud": [ "invoice", "customer" ]
        "scope": "manage"
    }

Migration steps to v4
^^^^^^^^^^^^^^^^^^^^^
As described above, starting with v4, scopes have their own definition and can optionally be referenced by resources. 
Before v4, scopes were always contained within a resource.

To migrate to v4 you need to split up scope and resource registration, typically by first registering all your scopes
(e.g. using the ``AddInMemoryApiScopes`` method), and then register the API resources (if any) afterwards.
The API resources will then reference the prior registered scopes by name.
