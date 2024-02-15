/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IdentityServer8.Extensions;

internal static partial class JsonExtensions
{
    [DebuggerStepThrough]
    public static T ToObject<T>(this JsonElement element, JsonSerializerOptions options = null)
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using (var writer = new Utf8JsonWriter(bufferWriter))
            element.WriteTo(writer);
        return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
    }

    [DebuggerStepThrough]
    public static T ToObject<T>(this JsonDocument document, JsonSerializerOptions options = null)
    {
        if (document == null)
            throw new ArgumentNullException(nameof(document));
        return document.RootElement.ToObject<T>(options);
    }
}
