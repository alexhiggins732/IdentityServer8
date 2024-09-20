/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Events;

/// <summary>
/// Event for unhandled exceptions
/// </summary>
/// <seealso cref="IdentityServer8.Events.Event" />
public class InvalidClientConfigurationEvent : Event
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnhandledExceptionEvent" /> class.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="errorMessage">The error message.</param>
    public InvalidClientConfigurationEvent(Client client, string errorMessage)
        : base(EventCategories.Error,
              "Invalid Client Configuration",
              EventTypes.Error, 
              EventIds.InvalidClientConfiguration,
              errorMessage)
    {
        ClientId = client.ClientId;
        ClientName = client.ClientName ?? "unknown name";
    }

    /// <summary>
    /// Gets or sets the client ID.
    /// </summary>
    /// <value>
    /// The details.
    /// </value>
    public string ClientId { get; set; }

    /// <summary>
    /// Gets or sets the name of the client.
    /// </summary>
    /// <value>
    /// The name of the client.
    /// </value>
    public string ClientName { get; set; }
}
