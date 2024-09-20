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
/// Models a web API resource.
/// </summary>
[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
public class ApiResource : Resource
{
    private string DebuggerDisplay => Name ?? $"{{{typeof(ApiResource)}}}";
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResource"/> class.
    /// </summary>
    public ApiResource()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResource"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    public ApiResource(string name)
        : this(name, name, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResource"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="displayName">The display name.</param>
    public ApiResource(string name, string displayName)
        : this(name, displayName, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResource"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="userClaims">List of associated user claims that should be included when this resource is requested.</param>
    public ApiResource(string name, IEnumerable<string> userClaims)
        : this(name, name, userClaims)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResource"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="displayName">The display name.</param>
    /// <param name="userClaims">List of associated user claims that should be included when this resource is requested.</param>
    /// <exception cref="System.ArgumentNullException">name</exception>
    public ApiResource(string name, string displayName, IEnumerable<string> userClaims)
    {
        if (name.IsMissing()) throw new ArgumentNullException(nameof(name));

        Name = name;
        DisplayName = displayName;

        if (!userClaims.IsNullOrEmpty())
        {
            foreach (var type in userClaims)
            {
                UserClaims.Add(type);
            }
        }
    }

    /// <summary>
    /// The API secret is used for the introspection endpoint. The API can authenticate with introspection using the API name and secret.
    /// </summary>
    public ICollection<Secret> ApiSecrets { get; set; } = new HashSet<Secret>();

    /// <summary>
    /// Models the scopes this API resource allows.
    /// </summary>
    public ICollection<string> Scopes { get; set; } = new HashSet<string>();

    /// <summary>
    /// Signing algorithm for access token. If empty, will use the server default signing algorithm.
    /// </summary>
    public ICollection<string> AllowedAccessTokenSigningAlgorithms { get; set; } = new HashSet<string>();
}
