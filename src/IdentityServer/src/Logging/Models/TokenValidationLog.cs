/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Logging.Models;

internal class TokenValidationLog
{
    // identity token
    public string ClientId { get; set; }
    public string ClientName { get; set; }
    public bool ValidateLifetime { get; set; }

    // access token
    public string AccessTokenType { get; set; }
    public string ExpectedScope { get; set; }
    public string TokenHandle { get; set; }
    public string JwtId { get; set; }

    // both
    public Dictionary<string, object> Claims { get; set; }

    public override string ToString()
    {
        return LogSerializer.Serialize(this);
    }
}
