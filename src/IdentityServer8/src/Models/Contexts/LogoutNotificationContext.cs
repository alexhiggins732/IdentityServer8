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
/// Provides the context necessary to construct a logout notificaiton.
/// </summary>
public class LogoutNotificationContext
{
    /// <summary>
    ///  The SubjectId of the user.
    /// </summary>
    public string SubjectId { get; set; }

    /// <summary>
    /// The session Id of the user's authentication session.
    /// </summary>
    public string SessionId { get; set; }

    /// <summary>
    /// The list of client Ids that the user has authenticated to.
    /// </summary>
    public IEnumerable<string> ClientIds { get; set; }
}
