using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Serilog.Ui.Web.Tests.Utilities;
using Xunit;

namespace Serilog.Ui.Web.Tests
{
    [Trait("Ui-Middleware", "Web")]
    public class SerilogUiMiddlewareTest :
        IClassFixture<WebAppFactory.WithMocks>,
        IClassFixture<WebAppFactory.WithMocks.AndCustomOptions>
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClientWithCustomOpts;

        public SerilogUiMiddlewareTest(WebAppFactory.WithMocks program, WebAppFactory.WithMocks.AndCustomOptions programOpts)
        {
            _httpClient = program.CreateClient();
            _httpClientWithCustomOpts = programOpts.CreateClient();
        }

        [Theory]
        [InlineData("/serilog-ui/api/keys/", 417)]
        [InlineData("/serilog-ui/api/logs/", 409)]
        [InlineData("/serilog-ui/", 400)]
        [InlineData("/serilog-ui/index.html", 418)]
        public async Task It_hits_ui_endpoint_when_request_matches_method_and_options_prefix(string pathReq, int statusCode)
        {
            // Act
            var send = await _httpClient.GetAsync(pathReq);

            // Assert
            send.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Theory]
        [InlineData("/test/api/keys/", 417)]
        [InlineData("/test/api/logs/", 409)]
        [InlineData("/test/", 400)]
        [InlineData("/test/index.html", 418)]
        public async Task It_hits_ui_endpoint_when_request_matches_method_and_custom_options_prefix(string pathReq, int statusCode)
        {
            // Act
            var send = await _httpClientWithCustomOpts.GetAsync(pathReq);

            // Assert
            send.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Theory]
        [InlineData("fake-prefix/api/keys/", 417)]
        [InlineData("fake-prefix/api/logs/", 409)]
        [InlineData("/fake-prefix/", 400)]
        [InlineData("/fake-prefix/index.html", 418)]
        public async Task It_proceeds_onwards_when_request_does_not_match_options_prefix(string pathReq, int statusCode)
        {
            // Act
            var send = await _httpClient.GetAsync(pathReq);

            // Assert
            send.StatusCode.Should().NotBe((HttpStatusCode)statusCode);
        }

        [Theory]
        [InlineData("/serilog-ui/api/keys/", 417)]
        [InlineData("/serilog-ui/api/logs/", 409)]
        [InlineData("/serilog-ui/", 400)]
        [InlineData("/serilog-ui/index.html", 418)]
        public async Task It_proceeds_onwards_when_request_is_not_a_get(string pathReq, int statusCode)
        {
            // Arrange
            var methods = new HttpMethod[] {
                HttpMethod.Connect, HttpMethod.Delete, HttpMethod.Head, HttpMethod.Options,
                HttpMethod.Patch, HttpMethod.Post, HttpMethod.Put, HttpMethod.Trace,
            };

            foreach (var method in methods)
            {
                // Act
                var requestMsg = new HttpRequestMessage(method, pathReq);
                var send = await _httpClient.SendAsync(requestMsg);

                // Assert
                send.StatusCode.Should().NotBe((HttpStatusCode)statusCode);
            }
        }
    }
}