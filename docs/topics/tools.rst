Tools
=====

The ``IdentityServerTools`` class is a collection of useful internal tools that you might need when writing extensibility code
for IdentityServer. To use it, inject it into your code, e.g. a controller::

    public MyController(IdentityServerTools tools)
    {
        _tools = tools;
    }

The ``IssueJwtAsync`` method allows creating JWT tokens using the IdentityServer token creation engine. The ``IssueClientJwtAsync`` is an easier
version of that for creating tokens for server-to-server communication (e.g. when you have to call an IdentityServer protected API from your code)::

    public async Task<IActionResult> MyAction()
    {
        var token = await _tools.IssueClientJwtAsync(
            clientId: "client_id",
            lifetime: 3600,
            audiences: new[] { "backend.api" });

        // more code
    }