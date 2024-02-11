.. _refEvents:
Events
======
While logging is more low level "printf" style - events represent higher level information about certain operations in IdentityServer.
Events are structured data and include event IDs, success/failure information, categories and details.
This makes it easy to query and analyze them and extract useful information that can be used for further processing.

Events work great with event stores like `ELK <https://www.elastic.co/webinars/introduction-elk-stack>`_, `Seq <https://getseq.net/>`_ or `Splunk <https://www.splunk.com/>`_.

Emitting events
^^^^^^^^^^^^^^^
Events are not turned on by default - but can be globally configured in the ``ConfigureServices`` method, e.g.::

    services.AddIdentityServer(options =>
    {
        options.Events.RaiseSuccessEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseErrorEvents = true;
    });

To emit an event use the ``IEventService`` from the DI container and call the ``RaiseAsync`` method, e.g.::

    public async Task<IActionResult> Login(LoginInputModel model)
    {
        if (_users.ValidateCredentials(model.Username, model.Password))
        {
            // issue authentication cookie with subject ID and username
            var user = _users.FindByUsername(model.Username);
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username));
        }
        else
        {
            await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
        }
    }

Custom sinks
^^^^^^^^^^^^
Our default event sink will simply serialize the event class to JSON and forward it to the ASP.NET Core logging system.
If you want to connect to a custom event store, implement the ``IEventSink`` interface and register it with DI.

The following example uses `Seq <https://getseq.net/>`_ to emit events::

     public class SeqEventSink : IEventSink
    {
        private readonly Logger _log;

        public SeqEventSink()
        {
            _log = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
        }

        public Task PersistAsync(Event evt)
        {
            if (evt.EventType == EventTypes.Success ||
                evt.EventType == EventTypes.Information)
            {
                _log.Information("{Name} ({Id}), Details: {@details}",
                    evt.Name,
                    evt.Id,
                    evt);
            }
            else
            {
                _log.Error("{Name} ({Id}), Details: {@details}",
                    evt.Name,
                    evt.Id,
                    evt);
            }

            return Task.CompletedTask;
        }
    }

Add the ``Serilog.Sinks.Seq`` package to your host to make the above code work.

Built-in events
^^^^^^^^^^^^^^^
The following events are defined in IdentityServer:

``ApiAuthenticationFailureEvent`` & ``ApiAuthenticationSuccessEvent``
    Gets raised for successful/failed API authentication at the introspection endpoint.
``ClientAuthenticationSuccessEvent`` & ``ClientAuthenticationFailureEvent``
    Gets raised for successful/failed client authentication at the token endpoint.
``TokenIssuedSuccessEvent`` & ``TokenIssuedFailureEvent``
    Gets raised for successful/failed attempts to request identity tokens, access tokens, refresh tokens and authorization codes.
``TokenIntrospectionSuccessEvent`` & ``TokenIntrospectionFailureEvent``
    Gets raised for successful token introspection requests.
``TokenRevokedSuccessEvent``
    Gets raised for successful token revocation requests.
``UserLoginSuccessEvent`` & ``UserLoginFailureEvent``
    Gets raised by the quickstart UI for successful/failed user logins.
``UserLogoutSuccessEvent``
    Gets raised for successful logout requests.
``ConsentGrantedEvent`` & ``ConsentDeniedEvent``
    Gets raised in the consent UI.
``UnhandledExceptionEvent``
    Gets raised for unhandled exceptions.
``DeviceAuthorizationFailureEvent`` & ``DeviceAuthorizationSuccessEvent``
    Gets raised for successful/failed device authorization requests.

Custom events
^^^^^^^^^^^^^
You can create your own events and emit them via our infrastructure.

You need to derive from our base ``Event`` class which injects contextual information like activity ID, timestamp, etc.
Your derived class can then add arbitrary data fields specific to the event context::

    public class UserLoginFailureEvent : Event
    {
        public UserLoginFailureEvent(string username, string error)
            : base(EventCategories.Authentication,
                    "User Login Failure",
                    EventTypes.Failure, 
                    EventIds.UserLoginFailure,
                    error)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
