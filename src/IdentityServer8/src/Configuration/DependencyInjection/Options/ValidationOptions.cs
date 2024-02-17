/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Configuration;

/// <summary>
/// The ValidationOptions contains settings that affect some of the default validation behavior.
/// </summary>
public class ValidationOptions
{
    /// <summary>
    ///  Collection of URI scheme prefixes that should never be used as custom URI schemes in the redirect_uri passed to tha authorize endpoint.
    /// </summary>
    public ICollection<string> InvalidRedirectUriPrefixes { get; } = new HashSet<string>
    {
        "javascript:",
        "file:",
        "data:",
        "mailto:",
        "ftp:",
        "blob:",
        "about:",
        "ssh:",
        "tel:",
        "view-source:",
        "ws:",
        "wss:"
    };
}
