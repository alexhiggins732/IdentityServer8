/*
 Copyright (c) 2024 HigginsSoft, Alexander Higgins - https://github.com/alexhiggins732/ 

 Copyright (c) 2018, Brock Allen & Dominick Baier. All rights reserved.

 Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information. 
 Source code and license this software can be found 

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.
*/

using IdentityServer8.Models;
using IdentityServer8.Stores;
using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace IdentityServer.UnitTests.Stores
{
    public class InMemoryClientStoreTests
    {
        [Fact]
        public void InMemoryClient_should_throw_if_contain_duplicate_client_ids()
        {
            List<Client> clients = new List<Client>
            {
                new Client { ClientId = "1"},
                new Client { ClientId = "1"},
                new Client { ClientId = "3"}
            };

            Action act = () => new InMemoryClientStore(clients);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void InMemoryClient_should_not_throw_if_does_not_contain_duplicate_client_ids()
        {
            List<Client> clients = new List<Client>
            {
                new Client { ClientId = "1"},
                new Client { ClientId = "2"},
                new Client { ClientId = "3"}
            };

            new InMemoryClientStore(clients);
        }
    }
}
