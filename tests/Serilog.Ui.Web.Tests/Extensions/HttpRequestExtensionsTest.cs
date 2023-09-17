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
            var requestMock = Substitute.For<HttpRequest>();
            var httpContextMock = Substitute.For<HttpContext>();
            var dic = new HeaderDictionary { };

            httpContextMock.Connection.Returns(new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.Parse("20.100.30.10"))
                .WithLocalIp(IPAddress.Parse("20.100.30.10")));
            requestMock.Headers.Returns(dic);
            requestMock.HttpContext.Returns(httpContextMock);

            requestMock.IsLocal().Should().BeTrue();
        }

        [Fact]
        public void It_is_local_when_remote_ip_address_is_loopback()
        {
            var requestMock = Substitute.For<HttpRequest>();
            var httpContextMock = Substitute.For<HttpContext>();
            var dic = new HeaderDictionary { };
            requestMock.Headers.Returns(dic);
            requestMock.HttpContext.Returns(httpContextMock);

            httpContextMock.Connection.Returns(new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.Loopback));
            requestMock.IsLocal().Should().BeTrue();

            httpContextMock.Connection.Returns(new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.IPv6Loopback));
            requestMock.IsLocal().Should().BeTrue();
        }

        [Fact]
        public void It_is_local_when_no_xforwarded_and_remote_ip_address_is_null()
        {
            var requestMock = Substitute.For<HttpRequest>();
            var httpContextMock = Substitute.For<HttpContext>();
            var dic = new HeaderDictionary { };

            httpContextMock.Connection.Returns(new ConnectionInfoMock()
                .WithRemoteIp(null)
                .WithLocalIp(null));
            requestMock.Headers.Returns(dic);
            requestMock.HttpContext.Returns(httpContextMock);

            requestMock.IsLocal().Should().BeTrue();
        }

        [Fact]
        public void It_is_not_local_when_xforwarded()
        {
            var requestMock = Substitute.For<HttpRequest>();
            var dic = new HeaderDictionary { ["X-forwarded-for"] = "test" };
            requestMock.Headers.Returns(dic);

            requestMock.IsLocal().Should().BeFalse();
        }

        [Fact]
        public void It_is_not_local_when_remote_ip_address_is_not_local_nor_loopback()
        {
            var requestMock = Substitute.For<HttpRequest>();
            var httpContextMock = Substitute.For<HttpContext>();
            var dic = new HeaderDictionary { };

            requestMock.Headers.Returns(dic);
            requestMock.HttpContext.Returns(httpContextMock);

            httpContextMock.Connection.Returns(
                new ConnectionInfoMock().WithRemoteIp(IPAddress.Parse("20.100.30.10")));
            requestMock.IsLocal().Should().BeFalse();

            httpContextMock.Connection.Returns(
                new ConnectionInfoMock()
                .WithRemoteIp(IPAddress.Parse("20.100.30.10"))
                .WithLocalIp(IPAddress.Parse("231.228.97.51"))
                );
            requestMock.IsLocal().Should().BeFalse();
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
