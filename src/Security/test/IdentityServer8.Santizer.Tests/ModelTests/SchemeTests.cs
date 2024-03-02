using IdentityServer8.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer8.Sanitizer.Tests.ModelTests
{
    public class SchemeTests
    {
        [Fact]
        public void Scheme_SerializesName()
        {
            var any = Scheme.Any;
            var json = JsonSerializer.Serialize(any);
            var fromJson = JsonSerializer.Deserialize<Scheme>(json);
            Assert.Equal(any.Name, fromJson.Name);

        }

        [Fact]
        public void Scheme_ToString_MatchesName()
        {
            var any = Scheme.Any;
            var toString = any.ToString();
            Assert.Equal(any.Name, toString);

        }

        [Fact]
        public void Scheme_ParsesAnyName()
        {
            var scheme = Scheme.Any;
            var parsed = Scheme.Parse(scheme.Name);
            Assert.Equal(scheme.Name, parsed.Name);
        }

        [Fact]
        public void Scheme_ParsesHttpName()
        {
            var scheme = Scheme.Http;
            var parsed = Scheme.Parse(scheme.Name);
            Assert.Equal(scheme.Name, parsed.Name);
        }

        [Fact]
        public void Scheme_ParsesHttpsName()
        {
            var scheme = Scheme.Https;
            var parsed = Scheme.Parse(scheme.Name);
            Assert.Equal(scheme.Name, parsed.Name);
        }


        [Fact]
        public void Scheme_ParsingInvalidName_ThrowsException()
        {
            var scheme = "foo";
            Assert.Throws<ArgumentException>(()=> Scheme.Parse(scheme));
        }
    }
}
