/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Models;

/// <summary>
/// A client claim
/// </summary>
public class ClientClaim
{
    /// <summary>
    /// The claim type
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The claim value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// The claim value type
    /// </summary>
    public string ValueType { get; set; } = ClaimValueTypes.String;

    /// <summary>
    /// ctor
    /// </summary>
    public ClientClaim()
    {
    }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public ClientClaim(string type, string value)
    {
        Type = type;
        Value = value;
    }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <param name="valueType"></param>
    public ClientClaim(string type, string value, string valueType)
    {
        Type = type;
        Value = value;
        ValueType = valueType;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked 
        {
            int hash = 17;

            hash = hash * 23 + Value.GetHashCode();
            hash = hash * 23 + Type.GetHashCode();
            hash = hash * 23 + ValueType.GetHashCode();
            return hash;
        }
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is ClientClaim c)
        {
            return (string.Equals(Type, c.Type, StringComparison.Ordinal) &&
                    string.Equals(Value, c.Value, StringComparison.Ordinal) &&
                    string.Equals(ValueType, c.ValueType, StringComparison.Ordinal));
        }

        return false;
    }
}
