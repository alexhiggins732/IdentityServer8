/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Models;

internal static class ScopeExtensions
{
    [DebuggerStepThrough]
    public static string ToSpaceSeparatedString(this IEnumerable<ApiScope> apiScopes)
    {
        var scopeNames = from s in apiScopes
                         select s.Name;

        return string.Join(" ", scopeNames.ToArray());
    }

    [DebuggerStepThrough]
    public static IEnumerable<string> ToStringList(this IEnumerable<ApiScope> apiScopes)
    {
        var scopeNames = from s in apiScopes
                         select s.Name;

        return scopeNames;
    }
}
