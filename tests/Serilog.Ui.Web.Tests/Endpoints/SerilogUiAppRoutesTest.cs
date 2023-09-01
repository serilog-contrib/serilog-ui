using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Serilog.Ui.Web;
using Serilog.Ui.Web.Endpoints;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ui.Web.Tests.Endpoints
{
    [Trait("Ui-Api-Routes", "Web")]
    public class SerilogUiAppRoutesTest
    {
        private readonly Mock<IAppStreamLoader> streamLoaderMock;
        private readonly SerilogUiAppRoutes sut;
        private readonly DefaultHttpContext testContext;

        public SerilogUiAppRoutesTest()
        {
            testContext = new DefaultHttpContext();
            testContext.Request.Host = new HostString("test.dev");
            testContext.Request.Scheme = "https";
            streamLoaderMock = new Mock<IAppStreamLoader>();
            sut = new SerilogUiAppRoutes(streamLoaderMock.Object);
        }

        [Fact]
        public async Task It_gets_app_home()
        {
            sut.SetOptions(new()
            {
                BodyContent = "<div>body-test</div>",
                HeadContent = "<div>head-test</div>",
                Authorization = new() { AuthenticationType = AuthenticationType.Jwt },
                RoutePrefix = "test",
                HomeUrl = "home-url"
            });
            testContext.Request.Path = "/serilog-ui-url/index.html";
            testContext.Response.Body = new MemoryStream();

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(
                "<!DOCTYPE html><html lang=\"en\"><head><meta name=\"dummy\" content=\"%(HeadContent)\"></head>" +
                "<body><div id=\"serilog-ui-app\"></div><script>const config = '%(Configs)';</script>" +
                "<meta name=\"dummy\" content=\"%(BodyContent)\"></body></html>"));
            streamLoaderMock.Setup(sm => sm.GetIndex()).Returns(stream);

            await sut.GetHome(testContext);

            testContext.Response.StatusCode.Should().Be(200);
            testContext.Response.ContentType.Should().Be("text/html;charset=utf-8");

            testContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var bodyWrite = await new StreamReader(testContext.Response.Body).ReadToEndAsync();
            bodyWrite.Should().Be(
                "<!DOCTYPE html><html lang=\"en\"><head><div>head-test</div></head>" +
                "<body><div id=\"serilog-ui-app\"></div>" +
                "<script>const config = '%7B%22routePrefix%22%3A%22test%22%2C%22authType%22%3A%22Jwt%22%2C%22homeUrl%22%3A%22home-url%22%7D';</script>" +
                "<div>body-test</div></body></html>"
                );
        }

        [Fact]
        public async Task It_returns_page_error_when_stream_cannot_load_app_home()
        {
            sut.SetOptions(new());
            testContext.Request.Path = "/serilog-ui-url/index.html";
            testContext.Response.Body = new MemoryStream();
            streamLoaderMock.Setup(sm => sm.GetIndex()).Returns<Stream>(null);

            await sut.GetHome(testContext);

            testContext.Response.StatusCode.Should().Be(500);

            testContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var bodyWrite = await new StreamReader(testContext.Response.Body).ReadToEndAsync();
            bodyWrite.Should().Be("<div>Server error while loading assets. Please contact administration.</div>");
        }

        [Fact]
        public async Task It_redirects_app_home()
        {
            testContext.Request.Path = "/serilog-ui-url/";
            await sut.RedirectHome(testContext);

            testContext.Response.StatusCode.Should().Be(301);
            testContext.Response.Headers.Location.First().Should().Be("https://test.dev/serilog-ui-url/index.html");
        }

        [Fact]
        public Task It_throws_on_app_home_if_ui_options_were_not_set()
        {
            var result = () => sut.GetHome(new DefaultHttpContext());

            return result.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
