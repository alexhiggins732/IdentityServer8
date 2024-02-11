Client Authentication
=====================
In certain situations, clients need to authenticate with IdentityServer, e.g.

* confidential applications (aka clients) requesting tokens at the token endpoint
* APIs validating reference tokens at the introspection endpoint

For that purpose you can assign a list of secrets to a client or an API resource.

Secret parsing and validation is an extensibility point in identityserver, out of the box it supports shared secrets
as well as transmitting the shared secret via a basic authentication header or the POST body.

Creating a shared secret
^^^^^^^^^^^^^^^^^^^^^^^^
The following code sets up a hashed shared secret::

    var secret = new Secret("secret".Sha256());

This secret can now be assigned to either a ``Client`` or an ``ApiResource``. 
Notice that both do not only support a single secret, but multiple. This is useful for secret rollover and rotation::

    var client = new Client
    {
        ClientId = "client",
        ClientSecrets = new List<Secret> { secret },

        AllowedGrantTypes = GrantTypes.ClientCredentials,
        AllowedScopes = 
        {
            "api1", "api2"
        }
    };

In fact you can also assign a description and an expiration date to a secret. The description will be used for logging, and 
the expiration date for enforcing a secret lifetime::

    var secret = new Secret(
        "secret".Sha256(), 
        "2016 secret", 
        new DateTime(2016, 12, 31));  

Authentication using a shared secret
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
You can either send the client id/secret combination as part of the POST body::

    POST /connect/token
    
    client_id=client1&
    client_secret=secret&
    ...

..or as a basic authentication header::

    POST /connect/token
    
    Authorization: Basic xxxxx

    ...

You can manually create a basic authentication header using the following C# code::

    var credentials = string.Format("{0}:{1}", clientId, clientSecret);
    var headerValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

    var client = new HttpClient();
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", headerValue);

The `IdentityModel <https://github.com/IdentityModel/IdentityModel>`_ library has helper classes called ``TokenClient`` and ``IntrospectionClient`` that encapsulate
both authentication and protocol messages.

Authentication using an asymmetric Key
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
There are other techniques to authenticate clients, e.g. based on public/private key cryptography.
IdentityServer includes support for private key JWT client secrets (see `RFC 7523 <https://tools.ietf.org/html/rfc7523>`_
and `here <https://openid.net/specs/openid-connect-core-1_0.html#ClientAuthentication>`_).

Secret extensibility typically consists of three things:

* a secret definition
* a secret parser that knows how to extract the secret from the incoming request
* a secret validator that knows how to validate the parsed secret based on the definition

Secret parsers and validators are implementations of the ``ISecretParser`` and ``ISecretValidator`` interfaces. 
To make them available to IdentityServer, you need to register them with the DI container, e.g.::

    builder.AddSecretParser<JwtBearerClientAssertionSecretParser>()
    builder.AddSecretValidator<PrivateKeyJwtSecretValidator>()

Our default private key JWT secret validator expects the full (leaf) certificate as base64 on the secret definition 
or an ESA/EC JSON web key::

    var client = new Client
    {
        ClientId = "client.jwt",
        ClientSecrets =
        {
            new Secret
            {
                Type = IdentityServerConstants.SecretTypes.X509CertificateBase64,
                Value = "MIIDATCCAe2gAwIBAgIQoHUYAquk9rBJcq8W+F0FAzAJBgUrDgMCHQUAMBIxEDAOBgNVBAMTB0RldlJvb3QwHhcNMTAwMTIwMjMwMDAwWhcNMjAwMTIwMjMwMDAwWjARMQ8wDQYDVQQDEwZDbGllbnQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDSaY4x1eXqjHF1iXQcF3pbFrIbmNw19w/IdOQxbavmuPbhY7jX0IORu/GQiHjmhqWt8F4G7KGLhXLC1j7rXdDmxXRyVJBZBTEaSYukuX7zGeUXscdpgODLQVay/0hUGz54aDZPAhtBHaYbog+yH10sCXgV1Mxtzx3dGelA6pPwiAmXwFxjJ1HGsS/hdbt+vgXhdlzud3ZSfyI/TJAnFeKxsmbJUyqMfoBl1zFKG4MOvgHhBjekp+r8gYNGknMYu9JDFr1ue0wylaw9UwG8ZXAkYmYbn2wN/CpJl3gJgX42/9g87uLvtVAmz5L+rZQTlS1ibv54ScR2lcRpGQiQav/LAgMBAAGjXDBaMBMGA1UdJQQMMAoGCCsGAQUFBwMCMEMGA1UdAQQ8MDqAENIWANpX5DZ3bX3WvoDfy0GhFDASMRAwDgYDVQQDEwdEZXZSb290ghAsWTt7E82DjU1E1p427Qj2MAkGBSsOAwIdBQADggEBADLje0qbqGVPaZHINLn+WSM2czZk0b5NG80btp7arjgDYoWBIe2TSOkkApTRhLPfmZTsaiI3Ro/64q+Dk3z3Kt7w+grHqu5nYhsn7xQFAQUf3y2KcJnRdIEk0jrLM4vgIzYdXsoC6YO+9QnlkNqcN36Y8IpSVSTda6gRKvGXiAhu42e2Qey/WNMFOL+YzMXGt/nDHL/qRKsuXBOarIb++43DV3YnxGTx22llhOnPpuZ9/gnNY7KLjODaiEciKhaKqt/b57mTEz4jTF4kIg6BP03MUfDXeVlM1Qf1jB43G2QQ19n5lUiqTpmQkcfLfyci2uBZ8BkOhXr3Vk9HIk/xBXQ="
            }
            new Secret
            {
                Type = IdentityServerConstants.SecretTypes.JsonWebKey,
                Value = "{'e':'AQAB','kid':'ZzAjSnraU3bkWGnnAqLapYGpTyNfLbjbzgAPbbW2GEA','kty':'RSA','n':'wWwQFtSzeRjjerpEM5Rmqz_DsNaZ9S1Bw6UbZkDLowuuTCjBWUax0vBMMxdy6XjEEK4Oq9lKMvx9JzjmeJf1knoqSNrox3Ka0rnxXpNAz6sATvme8p9mTXyp0cX4lF4U2J54xa2_S9NF5QWvpXvBeC4GAJx7QaSw4zrUkrc6XyaAiFnLhQEwKJCwUw4NOqIuYvYp_IXhw-5Ti_icDlZS-282PcccnBeOcX7vc21pozibIdmZJKqXNsL1Ibx5Nkx1F1jLnekJAmdaACDjYRLL_6n3W4wUp19UvzB1lGtXcJKLLkqB6YDiZNu16OSiSprfmrRXvYmvD8m6Fnl5aetgKw'}"
            }
        },

        AllowedGrantTypes = GrantTypes.ClientCredentials,
        AllowedScopes = { "api1", "api2" }
    };
