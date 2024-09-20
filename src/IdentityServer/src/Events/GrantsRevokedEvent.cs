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
/// Event for revoked grants.
/// </summary>
/// <seealso cref="IdentityServer8.Events.Event" />
public class GrantsRevokedEvent : Event
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GrantsRevokedEvent" /> class.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="clientId">The client identifier.</param>
    public GrantsRevokedEvent(string subjectId, string clientId)
        : base(EventCategories.Grants,
              "Grants revoked",
              EventTypes.Information,
              EventIds.GrantsRevoked)
    {
        SubjectId = subjectId;
        ClientId = clientId;
    }

    /// <summary>
    /// Gets or sets the subject identifier.
    /// </summary>
    /// <value>
    /// The subject identifier.
    /// </value>
    public string SubjectId { get; set; }

    /// <summary>
    /// Gets or sets the client ID.
    /// </summary>
    /// <value>
    /// The client identifier.
    /// </value>
    public string ClientId { get; set; }
}
