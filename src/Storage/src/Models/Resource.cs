/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Models;

/// <summary>
/// Models the common data of API and identity resources.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class Resource
{
    private string DebuggerDisplay => Name ?? $"{{{typeof(Resource)}}}";

    /// <summary>
    /// Indicates if this resource is enabled. Defaults to true.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The unique name of the resource.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Display name of the resource.
    /// </summary>
    public string DisplayName { get; set; }
    
    /// <summary>
    /// Description of the resource.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Specifies whether this scope is shown in the discovery document. Defaults to true.
    /// </summary>
    public bool ShowInDiscoveryDocument { get; set; } = true;

    /// <summary>
    /// List of associated user claims that should be included when this resource is requested.
    /// </summary>
    public ICollection<string> UserClaims { get; set; } = new HashSet<string>();

    /// <summary>
    /// Gets or sets the custom properties for the resource.
    /// </summary>
    /// <value>
    /// The properties.
    /// </value>
    public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
}
