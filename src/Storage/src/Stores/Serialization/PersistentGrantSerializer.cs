/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Stores.Serialization;

/// <summary>
/// JSON-based persisted grant serializer
/// </summary>
/// <seealso cref="IdentityServer8.Stores.Serialization.IPersistentGrantSerializer" />
public class PersistentGrantSerializer : IPersistentGrantSerializer
{
    private static readonly JsonSerializerSettings _settings;

    static PersistentGrantSerializer()
    {
        _settings = new JsonSerializerSettings
        {
            ContractResolver = new CustomContractResolver()
        };
        _settings.Converters.Add(new ClaimConverter());
        _settings.Converters.Add(new ClaimsPrincipalConverter());
    }

    /// <summary>
    /// Serializes the specified value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value, _settings);
    }

    /// <summary>
    /// Deserializes the specified string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json">The json.</param>
    /// <returns></returns>
    public T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, _settings);
    }
}
