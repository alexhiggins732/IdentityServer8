/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServerHost.Quickstart.UI;

/// <summary>
/// Interface for signing and signinn out users given different user and signin schemes.
/// </summary>
public interface ISignInHelper
{
    /// <summary>
    /// Signs in a user given a <see cref="LoginInputModel"/> and an <see cref="AuthorizationRequest"/>.
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="model"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<bool> SigninUser(
        HttpContext httpContext,
        LoginInputModel model,
        AuthorizationRequest context);

    /// <summary>
    /// Signs out a user using the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task SignOutAsync(HttpContext context);
}

/// <summary>
/// A <see cref="ISignInHelper"/> implentionat for signing and signing out users using the HttpContext and an im memory user store.
/// </summary>
public class HttpContextSignInHelper : ISignInHelper
{
    /// <summary>
    /// Constructs a new <see cref="HttpContextSignInHelper"/> with the given <see cref="IEventService"/> and <see cref="TestUserStore"/>.
    /// </summary>
    /// <param name="events"></param>
    /// <param name="users"></param>
    public HttpContextSignInHelper(IEventService events, TestUserStore users)
    {
        _events = events;
        this._users = users;
    }

    private IEventService _events;
    private TestUserStore _users;

    /// inheritedDoc
    public async Task<bool> SigninUser(
        HttpContext httpContext,
        LoginInputModel model,
        AuthorizationRequest context)
    {
        // validate username/password against in-memory store
        if (_users.ValidateCredentials(model.Username, model.Password))
        {
            var user = _users.FindByUsername(model.Username);
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username, clientId: context?.Client.ClientId));

            // only set explicit expiration here if user chooses "remember me". 
            // otherwise we rely upon expiration configured in cookie middleware.
            AuthenticationProperties props = null;
            if (AccountOptions.AllowRememberLogin && model.RememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                };
            };

            // issue authentication cookie with subject ID and username
            var isuser = new IdentityServerUser(user.SubjectId)
            {
                DisplayName = user.Username
            };
            await AuthenticationManagerExtensions.SignInAsync(httpContext, isuser, props);
            return true;
        }
        return false;
    }


    /// inheritedDoc
    public async Task SignOutAsync(HttpContext context)
    {
        await context.SignOutAsync();
    }

}
