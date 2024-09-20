/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

#pragma warning disable 1591

namespace IdentityServer8.Models;

public class GrantTypes
{
    public static ICollection<string> Implicit =>
        new[] { GrantType.Implicit };

    public static ICollection<string> ImplicitAndClientCredentials =>
        new[]  { GrantType.Implicit, GrantType.ClientCredentials };

    public static ICollection<string> Code =>
        new[] { GrantType.AuthorizationCode };

    public static ICollection<string> CodeAndClientCredentials =>
        new[] { GrantType.AuthorizationCode, GrantType.ClientCredentials };

    public static ICollection<string> Hybrid =>
        new[] { GrantType.Hybrid };

    public static ICollection<string> HybridAndClientCredentials =>
        new[] { GrantType.Hybrid, GrantType.ClientCredentials };

    public static ICollection<string> ClientCredentials =>
        new[] { GrantType.ClientCredentials };

    public static ICollection<string> ResourceOwnerPassword =>
        new[] { GrantType.ResourceOwnerPassword };

    public static ICollection<string> ResourceOwnerPasswordAndClientCredentials =>
        new[] { GrantType.ResourceOwnerPassword, GrantType.ClientCredentials };

    public static ICollection<string> DeviceFlow =>
        new[] { GrantType.DeviceFlow };
}
