/*
 Copyright (c) 2024, Martijn van Put - https://github.com/mvput/ 

 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Linq;
using FluentAssertions;
using IdentityServer.EntityFramework.Mappers;
using IdentityServer.Models;
using Xunit;

namespace IdentityServer.EntityFramework.UnitTests.Mappers;

public class ScopesMappersTests
{
    [Fact]
    public void ScopeAutomapperConfigurationIsValid()
    {
        ScopeMappers.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void CanMapScope()
    {
        var model = new ApiScope();
        var mappedEntity = model.ToEntity();
        var mappedModel = mappedEntity.ToModel();

        Assert.NotNull(mappedModel);
        Assert.NotNull(mappedEntity);
    }

    [Fact]
    public void Properties_Map()
    {
        var model = new ApiScope()
        {
            Description = "description",
            DisplayName = "displayname",
            Name = "foo",
            UserClaims = { "c1", "c2" },
            Properties = {
                { "x", "xx" },
                { "y", "yy" },
           },
            Enabled = false
        };


        var mappedEntity = model.ToEntity();
        mappedEntity.Description.Should().Be("description");
        mappedEntity.DisplayName.Should().Be("displayname");
        mappedEntity.Name.Should().Be("foo");

        mappedEntity.UserClaims.Count.Should().Be(2);
        mappedEntity.UserClaims.Select(x => x.Type).Should().BeEquivalentTo(new[] { "c1", "c2" });
        mappedEntity.Properties.Count.Should().Be(2);
        mappedEntity.Properties.Should().Contain(x => x.Key == "x" && x.Value == "xx");
        mappedEntity.Properties.Should().Contain(x => x.Key == "y" && x.Value == "yy");


        var mappedModel = mappedEntity.ToModel();

        mappedModel.Description.Should().Be("description");
        mappedModel.DisplayName.Should().Be("displayname");
        mappedModel.Enabled.Should().BeFalse();
        mappedModel.Name.Should().Be("foo");
        mappedModel.UserClaims.Count.Should().Be(2);
        mappedModel.UserClaims.Should().BeEquivalentTo(new[] { "c1", "c2" });
        mappedModel.Properties.Count.Should().Be(2);
        mappedModel.Properties["x"].Should().Be("xx");
        mappedModel.Properties["y"].Should().Be("yy");
    }
}
