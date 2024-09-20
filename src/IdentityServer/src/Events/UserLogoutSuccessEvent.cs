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
/// Event for successful user logout
/// </summary>
/// <seealso cref="IdentityServer8.Events.Event" />
public class UserLogoutSuccessEvent : Event
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserLogoutSuccessEvent"/> class.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="name">The name.</param>
    public UserLogoutSuccessEvent(string subjectId, string name)
        : base(EventCategories.Authentication, 
              "User Logout Success",
              EventTypes.Success, 
              EventIds.UserLogoutSuccess)
    {
        SubjectId = subjectId;
        DisplayName = name;
    }

    /// <summary>
    /// Gets or sets the subject identifier.
    /// </summary>
    /// <value>
    /// The subject identifier.
    /// </value>
    public string SubjectId { get; set; }

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    /// <value>
    /// The display name.
    /// </value>
    public string DisplayName { get; set; }
}
