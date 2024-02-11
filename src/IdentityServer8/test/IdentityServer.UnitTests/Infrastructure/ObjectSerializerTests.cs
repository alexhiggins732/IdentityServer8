/*
 Copyright (c) 2024 HigginsSoft
 Written by Alexander Higgins https://github.com/alexhiggins732/ 
 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code for this software can be found at https://github.com/alexhiggins732/IdentityServer8

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

*/

using System;
using FluentAssertions;
using IdentityServer8.Models;
using Xunit;

namespace IdentityServer.UnitTests.Infrastructure
{
    public class ObjectSerializerTests
    {
        public ObjectSerializerTests()
        {
        }

        [Fact]
        public void Can_be_deserialize_message()
        {
            Action a = () => IdentityServer8.ObjectSerializer.FromString<Message<ErrorMessage>>("{\"created\":0, \"data\": {\"error\": \"error\"}}");
            a.Should().NotThrow();
        }
    }
}
