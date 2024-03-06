using FluentAssertions;
using FluentAssertions.Equivalency;
using IdentityServer.UnitTests.Common;
using IdentityServer8.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using static IdentityServer8.Validation.DefaultScopeParser;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IdentityServer.UnitTests.Serialization
{
    public class ParseScopeContextTests
    {

        [Fact]
        public void ParseScopeContext_SerializesRoundTrips()
        {
            var context = new ParseScopeContext("test");
            var json = JsonSerializer.Serialize(context);
            Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<ParseScopeContext>(json));
        }

        [Fact]
        public void ParseScopeValues_ThrowsWithNull_Enumerable()
        {
            IEnumerable<string>? scopeValues = null;
            var logger = TestLogger.Create<DefaultScopeParser>();
            var parser = new DefaultScopeParser(logger);
            Assert.Throws<ArgumentNullException>(() => parser.ParseScopeValues(scopeValues));
        }

        [Fact]
        public void ParseScopeContext_SetsIgnore()
        {
            var ctx = new ParseScopeContext("test");
            ctx.SetIgnore();

            ctx.Ignore.Should().BeTrue();
            ctx.Error.Should().BeNull();
            ctx.ParsedName.Should().BeNull();
            ctx.ParsedParameter.Should().BeNull();
            ctx.Succeeded.Should().BeFalse();
        }

        [Fact]
        public void ParseScopeContext_SetsError()
        {
            var ctx = new ParseScopeContext("test");
            ctx.SetError("error");

            ctx.Ignore.Should().BeFalse();
            ctx.Error.Should().Be("error");
            ctx.ParsedName.Should().BeNull();
            ctx.ParsedParameter.Should().BeNull();
            ctx.Succeeded.Should().BeFalse();   
        }

        [Fact]
        public void ParseScopeContext_ThrowsWithNullOrEmptyNameOrParameter()
        {
            var ctx = new ParseScopeContext("test");
            Assert.Throws<ArgumentNullException>(() => ctx.SetParsedValues(null, "test"));
            Assert.Throws<ArgumentNullException>(() => ctx.SetParsedValues("", "test"));
            Assert.Throws<ArgumentNullException>(() => ctx.SetParsedValues(" ", "test"));
            Assert.Throws<ArgumentNullException>(() => ctx.SetParsedValues("test", null));
            Assert.Throws<ArgumentNullException>(() => ctx.SetParsedValues("test", ""));
            Assert.Throws<ArgumentNullException>(() => ctx.SetParsedValues("test", " "));
        }


        [Fact]
        public void ParseScopeContext_SucceedsOnNonNullValues()
        {
            var ctx = new ParseScopeContext("test");

            ctx.SetParsedValues(nameof(ParseScopeContext.ParsedName), nameof(ParseScopeContext.ParsedParameter));
            ctx.Error.Should().BeNull();
            ctx.Ignore.Should().BeFalse();
            ctx.ParsedName.Should().Be(nameof(ParseScopeContext.ParsedName));
            ctx.ParsedParameter.Should().Be(nameof(ParseScopeContext.ParsedParameter));
            ctx.Succeeded.Should().BeTrue();
        }
    }
}
