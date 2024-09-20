/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Events;

/// <summary>
/// Event for failed user authentication
/// </summary>
/// <seealso cref="IdentityServer.Events.Event" />
public class UserLoginFailureEvent : Event
{
    /// <summary>
    /// Initializes a new instance of the <see cref="T:IdentityServer.Events.UserLoginFailureEvent" /> class.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="error">The error.</param>
    /// <param name="interactive">Specifies if login was interactive</param>
    /// <param name="clientId">The client id</param>
    public UserLoginFailureEvent(string username, string error, bool interactive = true, string clientId = null)
        : base(EventCategories.Authentication,
              "User Login Failure",
              EventTypes.Failure, 
              EventIds.UserLoginFailure,
              error)
    {
        Username = username;
        ClientId = clientId;

        if (interactive)
        {
            Endpoint = "UI";
        }
        else
        {
            Endpoint = EndpointNames.Token;
        }
    }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>
    /// The username.
    /// </value>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the endpoint.
    /// </summary>
    /// <value>
    /// The endpoint.
    /// </value>
    public string Endpoint { get; set; }

    /// <summary>
    /// Gets or sets the client id.
    /// </summary>
    /// <value>
    /// The client id.
    /// </value>
    public string ClientId { get; set; }
}
