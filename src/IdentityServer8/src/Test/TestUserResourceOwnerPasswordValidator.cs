/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityServer8.Test;

/// <summary>
/// Resource owner password validator for test users
/// </summary>
/// <seealso cref="IdentityServer8.Validation.IResourceOwnerPasswordValidator" />
public class TestUserResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly TestUserStore _users;
    private readonly ISystemClock _clock;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestUserResourceOwnerPasswordValidator"/> class.
    /// </summary>
    /// <param name="users">The users.</param>
    /// <param name="clock">The clock.</param>
    public TestUserResourceOwnerPasswordValidator(TestUserStore users, ISystemClock clock)
    {
        _users = users;
        _clock = clock;
    }

    /// <summary>
    /// Validates the resource owner password credential
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        if (_users.ValidateCredentials(context.UserName, context.Password))
        {
            var user = _users.FindByUsername(context.UserName);
            context.Result = new GrantValidationResult(
                user.SubjectId ?? throw new ArgumentException("Subject ID not set", nameof(user.SubjectId)), 
                OidcConstants.AuthenticationMethods.Password, _clock.UtcNow.UtcDateTime, 
                user.Claims);
        }

        return Task.CompletedTask;
    }
}
