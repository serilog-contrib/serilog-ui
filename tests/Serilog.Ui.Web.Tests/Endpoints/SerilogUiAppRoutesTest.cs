using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Ui.Web.Endpoints;
using Xunit;

namespace Serilog.Ui.Web.Tests.Endpoints
{
    [Trait("Ui-Api-Routes", "Web")]
    public class SerilogUiAppRoutesTest
    {
        private readonly IAppStreamLoader _streamLoaderMock;
        private readonly SerilogUiAppRoutes _sut;
        private readonly DefaultHttpContext _testContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public SerilogUiAppRoutesTest()
        {
            _testContext = new DefaultHttpContext
            {
                Request =
                {
                    Host = new HostString("test.dev"),
                    Scheme = "https"
                }
            };
            _streamLoaderMock = Substitute.For<IAppStreamLoader>();
            _contextAccessor = Substitute.For<IHttpContextAccessor>();
            _contextAccessor.HttpContext.Returns(_testContext);
            _sut = new SerilogUiAppRoutes(_contextAccessor, _streamLoaderMock);
        }

        [Fact]
        public async Task It_gets_app_home()
        {
            // Arrange
            _sut.SetOptions(new()
            {
                BodyContent = "<div>body-test</div>",
                HeadContent = "<div>head-test</div>",
                Authorization = new() { AuthenticationType = AuthenticationType.Jwt },
                RoutePrefix = "test",
                HomeUrl = "home-url"
            });
            _testContext.Request.Path = "/serilog-ui-url/";
            _testContext.Response.Body = new MemoryStream();

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(
                "<!DOCTYPE html><html lang=\"en\"><head><meta name=\"dummy\" content=\"%(HeadContent)\"></head>" +
                "<body><div id=\"serilog-ui-app\"></div><script>const config = '%(Configs)';</script>" +
                "<meta name=\"dummy\" content=\"%(BodyContent)\"></body></html>"));
            _streamLoaderMock.GetIndex().Returns(stream);

            // Act
            await _sut.GetHomeAsync();

            // Assert
            _testContext.Response.StatusCode.Should().Be(200);
            _testContext.Response.ContentType.Should().Be("text/html;charset=utf-8");

            _testContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var bodyWrite = await new StreamReader(_testContext.Response.Body).ReadToEndAsync();
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
            // Arrange
            _sut.SetOptions(new());
            _testContext.Request.Path = "/serilog-ui-url/index.html";
            _testContext.Response.Body = new MemoryStream();
            _streamLoaderMock.GetIndex().Returns((Stream)null!);

            // Act
            await _sut.GetHomeAsync();

            // Assert
            _testContext.Response.StatusCode.Should().Be(500);

            _testContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var bodyWrite = await new StreamReader(_testContext.Response.Body).ReadToEndAsync();
            bodyWrite.Should().Be("<div>Server error while loading assets. Please contact administration.</div>");
        }

        [Fact]
        public async Task It_redirects_app_home()
        {
            // Arrange
            _testContext.Request.Path = "/serilog-ui-url/index.html";

            // Act
            await _sut.RedirectHomeAsync();

            // Assert
            _testContext.Response.StatusCode.Should().Be(301);
            _testContext.Response.Headers.Location[0].Should().Be("https://test.dev/serilog-ui-url/");
        }

        [Fact]
        public Task It_throws_on_app_home_if_ui_options_were_not_set()
        {
            // Arrange
            _contextAccessor.HttpContext.Returns(new DefaultHttpContext());

            // Act
            var result = () => _sut.GetHomeAsync();

            // Assert
            return result.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
