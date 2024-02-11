/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using IdentityServer8.EntityFramework.Mappers;
using IdentityServer8.Models;
using Xunit;

namespace IdentityServer8.EntityFramework.UnitTests.Mappers
{
    public class IdentityResourcesMappersTests
    {
        [Fact]
        public void IdentityResourceAutomapperConfigurationIsValid()
        {
            IdentityResourceMappers.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void CanMapIdentityResources()
        {
            var model = new IdentityResource();
            var mappedEntity = model.ToEntity();
            var mappedModel = mappedEntity.ToModel();

            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
        }
    }
}