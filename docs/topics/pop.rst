Proof-of-Possession Access Tokens
=================================
By default, OAuth access tokens are so called *bearer* tokens. This means they are not bound to a client and anybody who possess the token can use it (compare to cash).

*Proof-of-Possession* (short PoP) tokens are bound to the client that requested the token. 
If that token leaks, it cannot be used by anyone else (compare to a credit card - well at least in an ideal world).

See `this <https://leastprivilege.com/2020/01/15/oauth-2-0-the-long-road-to-proof-of-possession-access-tokens/>`_ blog post for more history and motivation.

IdentityServer supports PoP tokens by using the :ref:`Mutual TLS mechanism <refMutualTLS>`.