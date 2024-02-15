/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Models;

/// <summary>
/// Represents data needed for device flow.
/// </summary>
public class DeviceCode
{
    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    /// <value>
    /// The creation time.
    /// </value>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Gets or sets the lifetime.
    /// </summary>
    /// <value>
    /// The lifetime.
    /// </value>
    public int Lifetime { get; set; }

    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    /// <value>
    /// The client identifier.
    /// </value>
    public string ClientId { get; set; }

    /// <summary>
    /// Gets the description the user assigned to the device being authorized.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is open identifier.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is open identifier; otherwise, <c>false</c>.
    /// </value>
    public bool IsOpenId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is authorized.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is authorized; otherwise, <c>false</c>.
    /// </value>
    public bool IsAuthorized { get; set; }

    /// <summary>
    /// Gets or sets the requested scopes.
    /// </summary>
    /// <value>
    /// The authorized scopes.
    /// </value>
    public IEnumerable<string> RequestedScopes { get; set; }

    /// <summary>
    /// Gets or sets the authorized scopes.
    /// </summary>
    /// <value>
    /// The authorized scopes.
    /// </value>
    public IEnumerable<string> AuthorizedScopes { get; set; }

    /// <summary>
    /// Gets or sets the subject.
    /// </summary>
    /// <value>
    /// The subject.
    /// </value>
    public ClaimsPrincipal Subject { get; set; }

    /// <summary>
    /// Gets or sets the session identifier.
    /// </summary>
    /// <value>
    /// The session identifier.
    /// </value>
    public string SessionId { get; set; }
}
