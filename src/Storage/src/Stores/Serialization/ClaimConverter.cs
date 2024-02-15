/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

#pragma warning disable 1591

namespace IdentityServer8.Stores.Serialization;

public class ClaimConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(Claim) == objectType;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var source = serializer.Deserialize<ClaimLite>(reader);
        var target = new Claim(source.Type, source.Value, source.ValueType);
        return target;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var source = (Claim)value;

        var target = new ClaimLite
        {
            Type = source.Type,
            Value = source.Value,
            ValueType = source.ValueType
        };

        serializer.Serialize(writer, target);
    }
}
