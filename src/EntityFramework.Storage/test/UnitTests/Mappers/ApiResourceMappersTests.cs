/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using System.Linq;
using FluentAssertions;
using IdentityServer8.EntityFramework.Mappers;
using Xunit;
using ApiResource = IdentityServer8.Models.ApiResource;

namespace IdentityServer8.EntityFramework.UnitTests.Mappers;

public class ApiResourceMappersTests
{
    [Fact]
    public void AutomapperConfigurationIsValid()
    {
        ApiResourceMappers.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Can_Map()
    {
        var model = new ApiResource();
        var mappedEntity = model.ToEntity();
        var mappedModel = mappedEntity.ToModel();

        Assert.NotNull(mappedModel);
        Assert.NotNull(mappedEntity);
    }

    [Fact]
    public void Properties_Map()
    {
        var model = new ApiResource()
        {
           Description = "description",
           DisplayName = "displayname",
           Name = "foo",
           Scopes = { "foo1", "foo2" },
           Enabled = false
        };


        var mappedEntity = model.ToEntity();

        mappedEntity.Scopes.Count.Should().Be(2);
        var foo1 = mappedEntity.Scopes.FirstOrDefault(x => x.Scope == "foo1");
        foo1.Should().NotBeNull();
        var foo2 = mappedEntity.Scopes.FirstOrDefault(x => x.Scope == "foo2");
        foo2.Should().NotBeNull();
        

        var mappedModel = mappedEntity.ToModel();
        
        mappedModel.Description.Should().Be("description");
        mappedModel.DisplayName.Should().Be("displayname");
        mappedModel.Enabled.Should().BeFalse();
        mappedModel.Name.Should().Be("foo");
    }

    [Fact]
    public void missing_values_should_use_defaults()
    {
        var entity = new IdentityServer8.EntityFramework.Entities.ApiResource
        {
            Secrets = new System.Collections.Generic.List<Entities.ApiResourceSecret>
            {
                new Entities.ApiResourceSecret
                {
                }
            }
        };

        var def = new ApiResource
        {
            ApiSecrets = { new Models.Secret("foo") }
        };

        var model = entity.ToModel();
        model.ApiSecrets.First().Type.Should().Be(def.ApiSecrets.First().Type);
    }
}
