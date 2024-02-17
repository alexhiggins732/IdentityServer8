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
/// Configures events
/// </summary>
public class EventsOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether to raise success events.
    /// </summary>
    /// <value>
    ///   <c>true</c> if success event should be raised; otherwise, <c>false</c>.
    /// </value>
    public bool RaiseSuccessEvents { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to raise failure events.
    /// </summary>
    /// <value>
    ///   <c>true</c> if failure events should be raised; otherwise, <c>false</c>.
    /// </value>
    public bool RaiseFailureEvents { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to raise information events.
    /// </summary>
    /// <value>
    /// <c>true</c> if information events should be raised; otherwise, <c>false</c>.
    /// </value>
    public bool RaiseInformationEvents { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to raise error events.
    /// </summary>
    /// <value>
    ///   <c>true</c> if error events should be raised; otherwise, <c>false</c>.
    /// </value>
    public bool RaiseErrorEvents { get; set; } = false;
}
