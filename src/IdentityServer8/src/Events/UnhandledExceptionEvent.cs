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
public class UnhandledExceptionEvent : Event
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnhandledExceptionEvent"/> class.
    /// </summary>
    /// <param name="ex">The ex.</param>
    public UnhandledExceptionEvent(Exception ex)
        : base(EventCategories.Error,
              "Unhandled Exception",
              EventTypes.Error, 
              EventIds.UnhandledException,
              ex.Message)
    {
        Details = ex.ToString();
    }

    /// <summary>
    /// Gets or sets the details.
    /// </summary>
    /// <value>
    /// The details.
    /// </value>
    public string Details { get; set; }
}
