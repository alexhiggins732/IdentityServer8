/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

namespace IdentityModel.AspNetCore
{
    public class TokenEndpointService
    {
        private readonly AutomaticTokenManagementOptions _managementOptions;
        private readonly IOptionsSnapshot<OpenIdConnectOptions> _oidcOptions;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TokenEndpointService> _logger;

        public TokenEndpointService(
            IOptions<AutomaticTokenManagementOptions> managementOptions,
            IOptionsSnapshot<OpenIdConnectOptions> oidcOptions,
            IAuthenticationSchemeProvider schemeProvider,
            IHttpClientFactory httpClientFactory,
            ILogger<TokenEndpointService> logger)
        {
            _managementOptions = managementOptions.Value;
            _oidcOptions = oidcOptions;
            _schemeProvider = schemeProvider;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            var oidcOptions = await GetOidcOptionsAsync();
            var configuration = await oidcOptions.ConfigurationManager.GetConfigurationAsync(default(CancellationToken));

            var tokenClient = _httpClientFactory.CreateClient("tokenClient");

            return await tokenClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = configuration.TokenEndpoint,

                ClientId = oidcOptions.ClientId,
                ClientSecret = oidcOptions.ClientSecret,
                RefreshToken = refreshToken
            });
        }

        public async Task<TokenRevocationResponse> RevokeTokenAsync(string refreshToken)
        {
            var oidcOptions = await GetOidcOptionsAsync();
            var configuration = await oidcOptions.ConfigurationManager.GetConfigurationAsync(default(CancellationToken));

            var tokenClient = _httpClientFactory.CreateClient("tokenClient");

            return await tokenClient.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = configuration.AdditionalData[OidcConstants.Discovery.RevocationEndpoint].ToString(),
                ClientId = oidcOptions.ClientId,
                ClientSecret = oidcOptions.ClientSecret,
                Token = refreshToken,
                TokenTypeHint = OidcConstants.TokenTypes.RefreshToken
            });
        }

        private async Task<OpenIdConnectOptions> GetOidcOptionsAsync()
        {
            if (string.IsNullOrEmpty(_managementOptions.Scheme))
            {
                var scheme = await _schemeProvider.GetDefaultChallengeSchemeAsync();
                return _oidcOptions.Get(scheme.Name);
            }
            else
            {
                return _oidcOptions.Get(_managementOptions.Scheme);
            }
        }
    }
}
