/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IdentityServer8;

internal static class ObjectSerializer
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        IgnoreNullValues = true
    };
    
    public static string ToString(object o)
    {
        return JsonSerializer.Serialize(o, Options);
    }

    public static T FromString<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value, Options);
    }
}
