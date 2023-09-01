using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Ui.Web;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ui.Web.Tests.Extensions
{
    [Trait("Ui-HttpRequest", "Web")]
    public class HttpRequestExtensionsTest
    {
        [Fact]
        public void It_is_local_when_remote_ip_address_equals_local_ip_address()
        {
            var mock = Substitute.For<HttpRequest>();
            var context = Substitute.For<HttpContext>();
            var dic = new HeaderDictionary { };

            context.Connection.Returns(new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.Parse("20.100.30.10"))
                .WithLocalIp(IPAddress.Parse("20.100.30.10")));
            mock.Headers.Returns(dic);
            mock.HttpContext.Returns(context);

            mock.IsLocal().Should().BeTrue();
        }

        [Fact]
        public void It_is_local_when_remote_ip_address_is_loopback()
        {
            var mock = Substitute.For<HttpRequest>();
            var context = Substitute.For<HttpContext>();
            var dic = new HeaderDictionary { };
            mock.Headers.Returns(dic);
            mock.HttpContext.Returns(context);

            context.Connection.Returns(new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.Loopback));
            mock.IsLocal().Should().BeTrue();

            context.Connection.Returns(new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.IPv6Loopback));
            mock.IsLocal().Should().BeTrue();
        }

        [Fact]
        public void It_is_local_when_no_xforwarded_and_remote_ip_address_is_null()
        {
            var mock = Substitute.For<HttpRequest>();
            var context = Substitute.For<HttpContext>();
            var dic = new HeaderDictionary { };

            context.Connection.Returns(new ConnectionInfoMock()
                .WithRemoteIp(null)
                .WithLocalIp(null));
            mock.Headers.Returns(dic);
            mock.HttpContext.Returns(context);

            mock.IsLocal().Should().BeTrue();
        }

        [Fact]
        public void It_is_not_local_when_xforwarded()
        {
            var mock = Substitute.For<HttpRequest>();
            var dic = new HeaderDictionary { ["X-forwarded-for"] = "test" };
            mock.Headers.Returns(dic);

            mock.IsLocal().Should().BeFalse();
        }

        [Fact]
        public void It_is_not_local_when_remote_ip_address_is_not_local_nor_loopback()
        {
            var mock = Substitute.For<HttpRequest>();
            var context = Substitute.For<HttpContext>();
            var dic = new HeaderDictionary { };

            mock.Headers.Returns(dic);
            mock.HttpContext.Returns(context);

            context.Connection.Returns(
                new ConnectionInfoMock().WithRemoteIp(IPAddress.Parse("20.100.30.10")));
            mock.IsLocal().Should().BeFalse();

            context.Connection.Returns(
                new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.Parse("20.100.30.10"))
                .WithLocalIp(IPAddress.Parse("231.228.97.51"))
                );
            mock.IsLocal().Should().BeFalse();
        }

        private class ConnectionInfoMock : ConnectionInfo
        {
            public override string Id { get; set; } = string.Empty;
            public override IPAddress? RemoteIpAddress { get; set; }
            public override int RemotePort { get; set; }
            public override IPAddress? LocalIpAddress { get; set; }
            public override int LocalPort { get; set; }
            public override X509Certificate2? ClientCertificate { get; set; }

            public override Task<X509Certificate2?> GetClientCertificateAsync(CancellationToken cancellationToken = default)
            {
                throw new System.NotImplementedException();
            }

            public ConnectionInfoMock WithRemoteIp(IPAddress? remoteIp) { RemoteIpAddress = remoteIp; return this; }
            public ConnectionInfoMock WithLocalIp(IPAddress? localIp) { LocalIpAddress = localIp; return this; }
        }
    }
}
