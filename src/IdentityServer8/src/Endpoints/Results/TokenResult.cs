/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using JsonExtensionDataAttribute = System.Text.Json.Serialization.JsonExtensionDataAttribute;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IdentityServer8.Endpoints.Results;

internal class TokenResult : IEndpointResult
{
    public TokenResponse Response { get; set; }

    public TokenResult(TokenResponse response)
    {
        Response = response ?? throw new ArgumentNullException(nameof(response));
    }

    public async Task ExecuteAsync(HttpContext context)
    {
        context.Response.SetNoCache();

        var dto = new ResultDto
        {
            id_token = Response.IdentityToken,
            access_token = Response.AccessToken,
            refresh_token = Response.RefreshToken,
            expires_in = Response.AccessTokenLifetime,
            token_type = OidcConstants.TokenResponse.BearerTokenType,
            scope = Response.Scope,

            Custom = Response.Custom
        };

        await context.Response.WriteJsonAsync(dto);
    }

    internal class ResultDto
    {
        public string id_token { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object>? Custom { get; set; }
    }
}
