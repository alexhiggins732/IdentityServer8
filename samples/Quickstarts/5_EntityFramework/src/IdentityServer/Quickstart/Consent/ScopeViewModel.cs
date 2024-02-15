/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServerHost.Quickstart.UI;

public class ScopeViewModel
{
    public string Value { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool Emphasize { get; set; }
    public bool Required { get; set; }
    public bool Checked { get; set; }
}
