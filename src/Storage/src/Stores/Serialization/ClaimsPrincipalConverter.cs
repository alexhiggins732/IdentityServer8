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

public class ClaimsPrincipalConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(ClaimsPrincipal) == objectType;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var source = serializer.Deserialize<ClaimsPrincipalLite>(reader);
        if (source == null) return null;

        var claims = source.Claims.Select(x => new Claim(x.Type, x.Value, x.ValueType));
        var id = new ClaimsIdentity(claims, source.AuthenticationType, JwtClaimTypes.Name, JwtClaimTypes.Role);
        var target = new ClaimsPrincipal(id);
        return target;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var source = (ClaimsPrincipal)value;

        var target = new ClaimsPrincipalLite
        {
            AuthenticationType = source.Identity.AuthenticationType,
            Claims = source.Claims.Select(x => new ClaimLite { Type = x.Type, Value = x.Value, ValueType = x.ValueType }).ToArray()
        };
        serializer.Serialize(writer, target);
    }
}
