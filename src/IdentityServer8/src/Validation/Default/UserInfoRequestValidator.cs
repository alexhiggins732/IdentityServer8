/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityModel;
using IdentityServer8.Extensions;
using IdentityServer8.Models;
using IdentityServer8.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer8.Validation
{
    /// <summary>
    /// Default userinfo request validator
    /// </summary>
    /// <seealso cref="IdentityServer8.Validation.IUserInfoRequestValidator" />
    internal class UserInfoRequestValidator : IUserInfoRequestValidator
    {
        private readonly ITokenValidator _tokenValidator;
        private readonly IProfileService _profile;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoRequestValidator" /> class.
        /// </summary>
        /// <param name="tokenValidator">The token validator.</param>
        /// <param name="profile">The profile service</param>
        /// <param name="logger">The logger.</param>
        public UserInfoRequestValidator(
            ITokenValidator tokenValidator, 
            IProfileService profile,
            ILogger<UserInfoRequestValidator> logger)
        {
            _tokenValidator = tokenValidator;
            _profile = profile;
            _logger = logger;
        }

        /// <summary>
        /// Validates a userinfo request.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<UserInfoRequestValidationResult> ValidateRequestAsync(string accessToken)
        {
            // the access token needs to be valid and have at least the openid scope
            var tokenResult = await _tokenValidator.ValidateAccessTokenAsync(
                accessToken,
                IdentityServerConstants.StandardScopes.OpenId);

            if (tokenResult.IsError)
            {
                return new UserInfoRequestValidationResult
                {
                    IsError = true,
                    Error = tokenResult.Error
                };
            }

            // the token must have a one sub claim
            var subClaim = tokenResult.Claims.SingleOrDefault(c => c.Type == JwtClaimTypes.Subject);
            if (subClaim == null)
            {
                _logger.LogError("Token contains no sub claim");

                return new UserInfoRequestValidationResult
                {
                    IsError = true,
                    Error = OidcConstants.ProtectedResourceErrors.InvalidToken
                };
            }

            // create subject from incoming access token
            var claims = tokenResult.Claims.Where(x => !Constants.Filters.ProtocolClaimsFilter.Contains(x.Type));
            var subject = Principal.Create("UserInfo", claims.ToArray());

            // make sure user is still active
            var isActiveContext = new IsActiveContext(subject, tokenResult.Client, IdentityServerConstants.ProfileIsActiveCallers.UserInfoRequestValidation);
            await _profile.IsActiveAsync(isActiveContext);

            if (isActiveContext.IsActive == false)
            {
                _logger.LogError("User is not active: {sub}", subject.GetSubjectId());

                return new UserInfoRequestValidationResult
                {
                    IsError = true,
                    Error = OidcConstants.ProtectedResourceErrors.InvalidToken
                };
            }

            return new UserInfoRequestValidationResult
            {
                IsError = false,
                TokenValidationResult = tokenResult,
                Subject = subject
            };
        }
    }
}