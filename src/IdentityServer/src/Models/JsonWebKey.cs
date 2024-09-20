/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

#pragma warning disable 1591

namespace IdentityServer.Models;

public class JsonWebKey
{
    public string kty { get; set; }
    public string use { get; set; }
    public string kid { get; set; }
    public string x5t { get; set; }
    public string e { get; set; }
    public string n { get; set; }
    public string[] x5c { get; set; }
    public string alg { get; set; }

    public string x { get; set; }
    public string y { get; set; }
    public string crv { get; set; }
}
