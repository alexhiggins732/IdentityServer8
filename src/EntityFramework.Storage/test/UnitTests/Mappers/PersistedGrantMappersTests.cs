/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using FluentAssertions;
using IdentityServer8.EntityFramework.Mappers;
using IdentityServer8.Models;
using Xunit;

namespace IdentityServer8.EntityFramework.UnitTests.Mappers;

public class PersistedGrantMappersTests
{
    [Fact]
    public void PersistedGrantAutomapperConfigurationIsValid()
    {
        PersistedGrantMappers.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void CanMap()
    {
        var model = new PersistedGrant()
        {
            ConsumedTime = new System.DateTime(2020, 02, 03, 4, 5, 6)
        };
        
        var mappedEntity = model.ToEntity();
        mappedEntity.ConsumedTime.Value.Should().Be(new System.DateTime(2020, 02, 03, 4, 5, 6));
        
        var mappedModel = mappedEntity.ToModel();
        mappedModel.ConsumedTime.Value.Should().Be(new System.DateTime(2020, 02, 03, 4, 5, 6));

        Assert.NotNull(mappedModel);
        Assert.NotNull(mappedEntity);
    }
}
