/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Services;

/// <summary>
/// Models making HTTP requests for back-channel logout notification.
/// </summary>
public interface IBackChannelLogoutHttpClient
{
    /// <summary>
    /// Performs HTTP POST.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    Task PostAsync(string url, Dictionary<string, string> payload);
}
