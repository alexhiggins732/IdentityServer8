/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

public class LogoutController : Controller
{
    public LogoutSessionManager LogoutSessions { get; }

    public LogoutController(LogoutSessionManager logoutSessions)
    {
        LogoutSessions = logoutSessions;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string logout_token)
    {
        Response.Headers.Append("Cache-Control", "no-cache, no-store");
        Response.Headers.Append("Pragma", "no-cache");

        try
        {
            var user = await ValidateLogoutToken(logout_token);

            // these are the sub & sid to signout
            var sub = user.FindFirst("sub")?.Value;
            var sid = user.FindFirst("sid")?.Value;

            LogoutSessions.Add(sub, sid);

            return Ok();
        }
        catch { }

        return BadRequest();
    }

    private async Task<ClaimsPrincipal> ValidateLogoutToken(string logoutToken)
    {
        var claims = await ValidateJwt(logoutToken);

        if (claims.FindFirst("sub") == null && claims.FindFirst("sid") == null) throw new Exception("Invalid logout token");

        var nonce = claims.FindFirstValue("nonce");
        if (!String.IsNullOrWhiteSpace(nonce)) throw new Exception("Invalid logout token");

        var eventsJson = claims.FindFirst("events")?.Value;
        if (String.IsNullOrWhiteSpace(eventsJson)) throw new Exception("Invalid logout token");

        var events =JsonSerializer.Deserialize<JsonElement>(eventsJson);
        var logoutEvent = events.TryGetValue("http://schemas.openid.net/event/backchannel-logout");
        if (logoutEvent.ValueKind == JsonValueKind.Null) throw new Exception("Invalid logout token");

        return claims;
    }

    private static async Task<ClaimsPrincipal> ValidateJwt(string jwt)
    {
        // read discovery document to find issuer and key material
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync(Constants.Authority);

        var keys = new List<SecurityKey>();
        foreach (var webKey in disco.KeySet.Keys)
        {
            var key = new JsonWebKey()
            {
                Kty = webKey.Kty,
                Alg = webKey.Alg,
                Kid = webKey.Kid,
                X = webKey.X,
                Y = webKey.Y,
                Crv = webKey.Crv,
                E = webKey.E,
                N = webKey.N,
            };
            keys.Add(key);
        }

        var parameters = new TokenValidationParameters
        {
            ValidIssuer = disco.Issuer,
            ValidAudience = "mvc.hybrid.backchannel",
            IssuerSigningKeys = keys,

            NameClaimType = JwtClaimTypes.Name,
            RoleClaimType = JwtClaimTypes.Role
        };

        var handler = new JwtSecurityTokenHandler();
        handler.InboundClaimTypeMap.Clear();

        var user = handler.ValidateToken(jwt, parameters, out var _);
        return user;
    }
}
