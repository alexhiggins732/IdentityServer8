/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

#pragma warning disable 1591

namespace IdentityServer8.ResponseHandling;

public class DeviceAuthorizationResponse
{
    public string DeviceCode { get; set; }
    public string UserCode { get; set; }
    public string VerificationUri { get; set; }

    public string VerificationUriComplete { get; set; }
    public int DeviceCodeLifetime { get; set; }
    public int Interval { get; set; }
}
