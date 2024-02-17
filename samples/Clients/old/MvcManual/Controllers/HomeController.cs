/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Secure()
    {
        if (User.Identity.IsAuthenticated) return View();

        return await StartAuthentication();
    }

    public async Task <IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync(Constants.Authority);

        return Redirect(disco.EndSessionEndpoint);
    }

    public async Task<IActionResult> FrontChannelLogout(string sid)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentSid = User.FindFirst("sid")?.Value ?? "";
            if (string.Equals(currentSid, sid, StringComparison.Ordinal))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        return NoContent();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> BackChannelLogout(string logout_token)
    {
        Response.Headers.Append("Cache-Control", "no-cache, no-store");
        Response.Headers.Append("Pragma", "no-cache");

        try
        {
            var user = await ValidateLogoutToken(logout_token);

            // these are the sub & sid to signout
            var sub = user.FindFirst("sub")?.Value;
            var sid = user.FindFirst("sid")?.Value;

            return Ok();
        }
        catch { }

        return BadRequest();
    }

    public IActionResult Error()
    {
        return View();
    }

    private async Task<IActionResult> StartAuthentication()
    {
        // read discovery document to find authorize endpoint
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync(Constants.Authority);

        var authorizeUrl = new RequestUrl(disco.AuthorizeEndpoint).CreateAuthorizeUrl(
            clientId: "mvc.manual",
            responseType: "id_token",
            scope: "openid profile",
            redirectUri: "http://localhost:44078/home/callback",
            state: "random_state",
            nonce: "random_nonce",
            responseMode: "form_post");

        return Redirect(authorizeUrl);
    }

    public async Task<IActionResult> Callback()
    {
        var state = Request.Form["state"].FirstOrDefault();
        var idToken = Request.Form["id_token"].FirstOrDefault();
        var error = Request.Form["error"].FirstOrDefault();

        if (!string.IsNullOrEmpty(error)) throw new Exception(error);
        if (!string.Equals(state, "random_state")) throw new Exception("invalid state");

        var user = await ValidateIdentityToken(idToken);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
        return Redirect("/home/secure");
    }

    private async Task<ClaimsPrincipal> ValidateIdentityToken(string idToken)
    {
        var user = await ValidateJwt(idToken);

        var nonce = user.FindFirst("nonce")?.Value ?? "";
        if (!string.Equals(nonce, "random_nonce")) throw new Exception("invalid nonce");

        return user;
    }

    private async Task<ClaimsPrincipal> ValidateLogoutToken(string logoutToken)
    {
        var claims = await ValidateJwt(logoutToken);

        if (claims.FindFirst("sub") == null && claims.FindFirst("sid") == null) throw new Exception("Invalid logout token");

        var nonce = claims.FindFirstValue("nonce");
        if (!String.IsNullOrWhiteSpace(nonce)) throw new Exception("Invalid logout token");

        var eventsJson = claims.FindFirst("events")?.Value;
        if (String.IsNullOrWhiteSpace(eventsJson)) throw new Exception("Invalid logout token");

        var events = JsonSerializer.Deserialize<JsonElement>(eventsJson);
        var logoutEvent = events.TryGetValue("http://schemas.openid.net/event/backchannel-logout");
        if (logoutEvent.ValueKind== JsonValueKind.Null) throw new Exception("Invalid logout token");

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
            var e = Base64Url.Decode(webKey.E);
            var n = Base64Url.Decode(webKey.N);

            var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
            {
                KeyId = webKey.Kid
            };

            keys.Add(key);
        }

        var parameters = new TokenValidationParameters
        {
            ValidIssuer = disco.Issuer,
            ValidAudience = "mvc.manual",
            IssuerSigningKeys = keys,

            NameClaimType = JwtClaimTypes.Name,
            RoleClaimType = JwtClaimTypes.Role,

            RequireSignedTokens = true
        };

        var handler = new JwtSecurityTokenHandler();
        handler.InboundClaimTypeMap.Clear();

        var user = handler.ValidateToken(jwt, parameters, out var _);
        return user;
    }
}
