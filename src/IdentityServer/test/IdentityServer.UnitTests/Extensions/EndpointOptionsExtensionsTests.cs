/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer.Configuration;
using IdentityServer.Extensions;
using IdentityServer.Hosting;
using Xunit;
using static IdentityServer.Constants;

namespace IdentityServer.UnitTests.Extensions
{
    public class EndpointOptionsExtensionsTests
    {
        private readonly EndpointsOptions _options = new EndpointsOptions();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsEndpointEnabledShouldReturnExpectedForAuthorizeEndpoint(bool expectedIsEndpointEnabled)
        {
            _options.EnableAuthorizeEndpoint = expectedIsEndpointEnabled;

            Assert.Equal(
                expectedIsEndpointEnabled,
                _options.IsEndpointEnabled(
                    CreateTestEndpoint(EndpointNames.Authorize)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsEndpointEnabledShouldReturnExpectedForCheckSessionEndpoint(bool expectedIsEndpointEnabled)
        {
            _options.EnableCheckSessionEndpoint = expectedIsEndpointEnabled;

            Assert.Equal(
                expectedIsEndpointEnabled,
                _options.IsEndpointEnabled(
                    CreateTestEndpoint(EndpointNames.CheckSession)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsEndpointEnabledShouldReturnExpectedForDeviceAuthorizationEndpoint(bool expectedIsEndpointEnabled)
        {
            _options.EnableDeviceAuthorizationEndpoint = expectedIsEndpointEnabled;

            Assert.Equal(
                expectedIsEndpointEnabled,
                _options.IsEndpointEnabled(
                    CreateTestEndpoint(EndpointNames.DeviceAuthorization)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsEndpointEnabledShouldReturnExpectedForDiscoveryEndpoint(bool expectedIsEndpointEnabled)
        {
            _options.EnableDiscoveryEndpoint = expectedIsEndpointEnabled;

            Assert.Equal(
                expectedIsEndpointEnabled,
                _options.IsEndpointEnabled(
                    CreateTestEndpoint(EndpointNames.Discovery)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsEndpointEnabledShouldReturnExpectedForEndSessionEndpoint(bool expectedIsEndpointEnabled)
        {
            _options.EnableEndSessionEndpoint = expectedIsEndpointEnabled;

            Assert.Equal(
                expectedIsEndpointEnabled,
                _options.IsEndpointEnabled(
                    CreateTestEndpoint(EndpointNames.EndSession)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsEndpointEnabledShouldReturnExpectedForIntrospectionEndpoint(bool expectedIsEndpointEnabled)
        {
            _options.EnableIntrospectionEndpoint = expectedIsEndpointEnabled;

            Assert.Equal(
                expectedIsEndpointEnabled,
                _options.IsEndpointEnabled(
                    CreateTestEndpoint(EndpointNames.Introspection)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsEndpointEnabledShouldReturnExpectedForTokenEndpoint(bool expectedIsEndpointEnabled)
        {
            _options.EnableTokenEndpoint = expectedIsEndpointEnabled;

            Assert.Equal(
                expectedIsEndpointEnabled,
                _options.IsEndpointEnabled(
                    CreateTestEndpoint(EndpointNames.Token)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsEndpointEnabledShouldReturnExpectedForRevocationEndpoint(bool expectedIsEndpointEnabled)
        {
            _options.EnableTokenRevocationEndpoint = expectedIsEndpointEnabled;

            Assert.Equal(
                expectedIsEndpointEnabled,
                _options.IsEndpointEnabled(
                    CreateTestEndpoint(EndpointNames.Revocation)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsEndpointEnabledShouldReturnExpectedForUserInfoEndpoint(bool expectedIsEndpointEnabled)
        {
            _options.EnableUserInfoEndpoint = expectedIsEndpointEnabled;

            Assert.Equal(
                expectedIsEndpointEnabled,
                _options.IsEndpointEnabled(
                    CreateTestEndpoint(EndpointNames.UserInfo)));
        }

        private Endpoint CreateTestEndpoint(string name)
        {
            return new Endpoint(name, "", null);
        }
    }
}
