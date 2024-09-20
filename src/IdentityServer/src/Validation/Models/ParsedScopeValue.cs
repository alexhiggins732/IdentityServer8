/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer.Validation;

/// <summary>
/// Models a parsed scope value.
/// </summary>
public class ParsedScopeValue
{
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="rawValue"></param>
    public ParsedScopeValue(string rawValue)
        : this(rawValue, rawValue, null)
    {
    }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="rawValue"></param>
    /// <param name="parsedName"></param>
    /// <param name="parsedParameter"></param>
    public ParsedScopeValue(string rawValue, string parsedName, string parsedParameter)
    {
        if (String.IsNullOrWhiteSpace(rawValue))
        {
            throw new ArgumentNullException(nameof(rawValue));
        }

        if (String.IsNullOrWhiteSpace(parsedName))
        {
            throw new ArgumentNullException(nameof(parsedName));
        }

        RawValue = rawValue;
        ParsedName = parsedName;
        ParsedParameter = parsedParameter;
    }

    /// <summary>
    /// The original (raw) value of the scope.
    /// </summary>
    public string RawValue { get; set; }

    /// <summary>
    /// The parsed name of the scope. If the scope has no structure, the parsed name will be the same as the raw value.
    /// </summary>
    public string ParsedName { get; set; }

    // future: maybe this should be something w/ more structure? dictionary?
    /// <summary>
    /// The parameter value of the parsed scope. If the scope has no structure, then the value will be null.
    /// </summary>
    public string ParsedParameter { get; set; }
}
