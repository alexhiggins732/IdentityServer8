/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IdentityServer8.Logging;

/// <summary>
/// Helper to JSON serialize object data for logging.
/// </summary>
internal static class LogSerializer
{
    static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        IgnoreNullValues = true,
        WriteIndented = true
    };

    static LogSerializer()
    {
        Options.Converters.Add(new JsonStringEnumConverter());
    }

    /// <summary>
    /// Serializes the specified object.
    /// </summary>
    /// <param name="logObject">The object.</param>
    /// <returns></returns>
    public static string Serialize(object logObject)
    {
        return JsonSerializer.Serialize(logObject, Options);
    }
}
