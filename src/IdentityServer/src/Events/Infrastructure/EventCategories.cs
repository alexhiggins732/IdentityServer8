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
/// Categories for events
/// </summary>
public static class EventCategories
{
    /// <summary>
    /// Authentication related events
    /// </summary>
    public const string Authentication = "Authentication";

    /// <summary>
    /// Token related events
    /// </summary>
    public const string Token = "Token";

    /// <summary>
    /// Grants related events
    /// </summary>
    public const string Grants = "Grants";

    /// <summary>
    /// Error related events
    /// </summary>
    public const string Error = "Error";

    /// <summary>
    /// Device flow related events
    /// </summary>
    public const string DeviceFlow = "Device";
}
