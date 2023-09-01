using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
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
            var mock = new Mock<HttpRequest>();
            var context = new Mock<HttpContext>();
            var dic = new HeaderDictionary { };

            context.SetupGet(p => p.Connection).Returns(new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.Parse("20.100.30.10"))
                .WithLocalIp(IPAddress.Parse("20.100.30.10")));
            mock.SetupGet(p => p.Headers).Returns(dic);
            mock.SetupGet(p => p.HttpContext).Returns(context.Object);

            mock.Object.IsLocal().Should().BeTrue();
        }

        [Fact]
        public void It_is_local_when_remote_ip_address_is_loopback()
        {
            var mock = new Mock<HttpRequest>();
            var context = new Mock<HttpContext>();
            var dic = new HeaderDictionary { };
            mock.SetupGet(p => p.Headers).Returns(dic);
            mock.SetupGet(p => p.HttpContext).Returns(context.Object);

            context.SetupGet(p => p.Connection).Returns(new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.Loopback));
            mock.Object.IsLocal().Should().BeTrue();

            context.SetupGet(p => p.Connection).Returns(new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.IPv6Loopback));
            mock.Object.IsLocal().Should().BeTrue();
        }

        [Fact]
        public void It_is_local_when_no_xforwarded_and_remote_ip_address_is_null()
        {
            var mock = new Mock<HttpRequest>();
            var context = new Mock<HttpContext>();
            var dic = new HeaderDictionary { };

            context.SetupGet(p => p.Connection).Returns(new ConnectionInfoMock()
                .WithRemoteIp(null)
                .WithLocalIp(null));
            mock.SetupGet(p => p.Headers).Returns(dic);
            mock.SetupGet(p => p.HttpContext).Returns(context.Object);

            mock.Object.IsLocal().Should().BeTrue();
        }

        [Fact]
        public void It_is_not_local_when_xforwarded()
        {
            var mock = new Mock<HttpRequest>();
            var dic = new HeaderDictionary { ["X-forwarded-for"] = "test" };
            mock.SetupGet(p => p.Headers).Returns(dic);

            mock.Object.IsLocal().Should().BeFalse();
        }

        [Fact]
        public void It_is_not_local_when_remote_ip_address_is_not_local_nor_loopback()
        {
            var mock = new Mock<HttpRequest>();
            var context = new Mock<HttpContext>();
            var dic = new HeaderDictionary { };

            mock.SetupGet(p => p.Headers).Returns(dic);
            mock.SetupGet(p => p.HttpContext).Returns(context.Object);

            context.SetupGet(p => p.Connection).Returns(
                new ConnectionInfoMock().WithRemoteIp(IPAddress.Parse("20.100.30.10")));
            mock.Object.IsLocal().Should().BeFalse();

            context.SetupGet(p => p.Connection).Returns(
                new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.Parse("20.100.30.10"))
                .WithLocalIp(IPAddress.Parse("231.228.97.51"))
                );
            mock.Object.IsLocal().Should().BeFalse();
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
