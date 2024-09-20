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
/// Indicates if the event is a success or fail event.
/// </summary>
public enum EventTypes
{
    /// <summary>
    /// Success event
    /// </summary>
    Success = 1,

    /// <summary>
    /// Failure event
    /// </summary>
    Failure = 2,

    /// <summary>
    /// Information event
    /// </summary>
    Information = 3,
    
    /// <summary>
    /// Error event
    /// </summary>
    Error = 4
}
