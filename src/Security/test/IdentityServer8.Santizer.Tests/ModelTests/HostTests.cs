using FluentAssertions;
using IdentityServer8.Security;
using System.Net;
using Xunit;
using Path = IdentityServer8.Security.Path;

namespace IdentityServer8.Sanitizer.Tests.ModelTests
{

    public class PathTests
    {
        [Fact]
        public void Path_AllowsNull()
        {
            var path = new Path(null);
            path.Value.Should().NotBeNull();
            path.Value.Should().Be(string.Empty);
        }


        [Fact]
        public void Path_TrimsValue()
        {
            var path = new Path(" test ");
            path.Value.Should().NotBeNull();
            path.Value.Should().Be("test");
        }

        [Fact]
        public void Path_IsAnyTest()
        {
            var path = new Path(" test ");
            path.Value.Should().NotBeNull();
            path.IsAny.Should().BeFalse();

            path = new Path("");
            path.Value.Should().NotBeNull();
            path.IsAny.Should().BeTrue();

            path = Path.Any;
            path.IsAny.Should().BeTrue();

        }

    }
    public class QueryTests
    {
        [Fact]
        public void Query_AllowsNull()
        { 
            var result = new Query(null);
            result.Value.Should().NotBeNull();
            result.Value.Should().Be(string.Empty);
            result.IsAny.Should().BeTrue();
        }


        [Fact]
        public void Query_TrimsValue()
        {
            var result = new Query(" test ");
            result.Value.Should().NotBeNull();
            result.Value.Should().Be("test");
        }

        [Fact]
        public void Query_IsAnyTest()
        {
            var result = new Query(" test ");
            result.Value.Should().NotBeNull();
            result.IsAny.Should().BeFalse();

            result = new Query("");
            result.Value.Should().NotBeNull();
            result.IsAny.Should().BeTrue();

            result = Query.Any;
            result.IsAny.Should().BeTrue();

        }

    }

    public class FragmentTests
    {
        [Fact]
        public void Fragment_AllowsNull()
        {
            var result = new Fragment(null);
            result.Value.Should().NotBeNull();
            result.Value.Should().Be(string.Empty);
            result.IsAny.Should().BeTrue();
        }


        [Fact]
        public void Fragment_TrimsValue()
        {
            var result = new Fragment(" test ");
            result.Value.Should().NotBeNull();
            result.Value.Should().Be("test");
        }

        [Fact]
        public void Fragment_IsAnyTest()
        {
            var result = new Fragment(" test ");
            result.Value.Should().NotBeNull();
            result.IsAny.Should().BeFalse();

            result = new Fragment("");
            result.Value.Should().NotBeNull();
            result.IsAny.Should().BeTrue();

            result = Fragment.Any;
            result.IsAny.Should().BeTrue();

        }

    }



    public class PortTest
    {
        [Fact]
        public void Port_ReturnsValue()
        {
            var port = new Port(80);
            port.Value.Should().Be(80);
        }

        [Fact]
        public void Port_IsAnyTests()
        {
            var port = new Port(80);
            port.IsAny.Should().BeFalse();

            var anyPort = new Port(0);
            anyPort.IsAny.Should().BeTrue();

            anyPort = Port.Any;
            anyPort.IsAny.Should().BeTrue();
        }

    }
    public class HostTests
    {
        [Fact]
        public void Host_AllowsNullName()
        {
            var host = new Host(null);
            host.Value.Should().NotBeNull();
            host.Value.Should().Be(string.Empty);
        }

        [Fact]
        public void Host_LowersName()
        {
            var host = new Host("TEST");
            host.Value.Should().NotBeNull();
            host.Value.Should().Be("test");
        }

        [Fact]
        public void Host_TrimsName()
        {
            var host = new Host(" TEST ");
            host.Value.Should().NotBeNull();
            host.Value.Should().Be("test");
        }

        [Fact]
        public void Host_ParsesAnyWildCard()
        {
            var host = new Host(" TEST ");
            host.Value.Should().NotBeNull();
            host.IsAny.Should().BeFalse();

            host = new Host("*");
            host.Value.Should().NotBeNull();
            host.IsAny.Should().BeTrue();

            host = new Host(" * ");
            host.Value.Should().NotBeNull();
            host.IsAny.Should().BeTrue();
        }

        [Fact]
        public void Host_ParsesSubdomainAnyWildCard()
        {
            var host = new Host(" TEST ");
            host.Value.Should().NotBeNull();
            host.HasWildcard.Should().BeFalse();

            host = new Host("*");
            host.Value.Should().NotBeNull();
            host.HasWildcard.Should().BeFalse();

            host = new Host("*.domain.com");
            host.Value.Should().NotBeNull();
            host.HasWildcard.Should().BeTrue();

            host = new Host(" *.domain.com ");
            host.Value.Should().NotBeNull();
            host.HasWildcard.Should().BeTrue();
        }


        [Fact]
        public void Host_IsLocalHost()
        {
            var host = new Host(" TEST ");
            host.Value.Should().NotBeNull();
            host.IsLocalhost.Should().BeFalse();

            host = new Host("127.0.0.1");
            host.Value.Should().NotBeNull();
            host.IsLocalhost.Should().BeTrue();

            host = new Host("localhost");
            host.Value.Should().NotBeNull();
            host.IsLocalhost.Should().BeTrue();

            host = new Host(" LOCALHOST ");
            host.Value.Should().NotBeNull();
            host.IsLocalhost.Should().BeTrue();
        }

        [Fact]
        public void Host_IsLocalOrNetwork()
        {
            var host = new Host(" TEST ");
            host.Value.Should().NotBeNull();
            host.IsLocalOrIpNetwork.Should().BeFalse();

            host = new Host("localhost");
            host.Value.Should().NotBeNull();
            host.IsLocalOrIpNetwork.Should().BeTrue();

            host = new Host(IPAddress.Loopback.ToString());
            host.Value.Should().NotBeNull();
            host.IsLocalOrIpNetwork.Should().BeTrue();

            host = new Host(IPAddress.IPv6None.ToString());
            host.Value.Should().NotBeNull();
            host.IsLocalOrIpNetwork.Should().BeTrue();

            host = new Host(IPAddress.IPv6Any.ToString());
            host.Value.Should().NotBeNull();
            host.IsIpAddress.Should().BeTrue();

            host = new Host(IPAddress.IPv6Loopback.ToString());
            host.Value.Should().NotBeNull();
            host.IsIpAddress.Should().BeTrue();

    
        }

        [Fact]
        public void Host_IsIpAddress()
        {
            var host = new Host(" TEST ");
            host.Value.Should().NotBeNull();
            host.IsIpAddress.Should().BeFalse();

            host = new Host(IPAddress.Loopback.ToString());
            host.Value.Should().NotBeNull();
            host.IsIpAddress.Should().BeTrue();

            host = new Host(IPAddress.IPv6None.ToString());
            host.Value.Should().NotBeNull();
            host.IsIpAddress.Should().BeTrue();

            host = new Host(IPAddress.IPv6Any.ToString());
            host.Value.Should().NotBeNull();
            host.IsIpAddress.Should().BeTrue();

            host = new Host(IPAddress.IPv6Loopback.ToString());
            host.Value.Should().NotBeNull();
            host.IsIpAddress.Should().BeTrue();

            host = new Host(IPAddress.IPv6None.ToString());
            host.Value.Should().NotBeNull();
            host.IsIpAddress.Should().BeTrue();
        }


        [Fact]
        public void Host_IsFQDN()
        {
            var host = new Host(" TEST ");
            host.Value.Should().NotBeNull();
            host.IsFullQualifiedDomainName.Should().BeFalse();

            host = new Host(IPAddress.Loopback.ToString());
            host.Value.Should().NotBeNull();
            host.IsFullQualifiedDomainName.Should().BeFalse();

            host = new Host(IPAddress.IPv6None.ToString());
            host.Value.Should().NotBeNull();
            host.IsFullQualifiedDomainName.Should().BeFalse();

            host = new Host(IPAddress.IPv6Any.ToString());
            host.Value.Should().NotBeNull();
            host.IsFullQualifiedDomainName.Should().BeFalse();

            host = new Host(IPAddress.IPv6Loopback.ToString());
            host.Value.Should().NotBeNull();
            host.IsFullQualifiedDomainName.Should().BeFalse();

            host = new Host(IPAddress.IPv6None.ToString());
            host.Value.Should().NotBeNull();
            host.IsFullQualifiedDomainName.Should().BeFalse();

            host= new Host("some.domain.com");
            host.Value.Should().NotBeNull();
            host.IsFullQualifiedDomainName.Should().BeTrue();

            host= new Host("domain.com");
            host.Value.Should().NotBeNull();
            host.IsFullQualifiedDomainName.Should().BeTrue();

            host = new Host("domain.local");
            host.Value.Should().NotBeNull();
            host.IsFullQualifiedDomainName.Should().BeTrue();
        }

    }
}
