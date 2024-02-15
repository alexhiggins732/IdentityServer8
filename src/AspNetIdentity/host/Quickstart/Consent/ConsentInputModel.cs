/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServerHost.Quickstart.UI;

public class ConsentInputModel
{
    public string Button { get; set; }
    public IEnumerable<string> ScopesConsented { get; set; }
    public bool RememberConsent { get; set; }
    public string ReturnUrl { get; set; }
    public string Description { get; set; }
}
