using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Serilog.Ui.Web.Tests.Utilities;
using Xunit;

namespace Serilog.Ui.Web.Tests
{
    [Trait("Ui-Middleware", "Web")]
    public class SerilogUiMiddlewareTest(WebAppFactory.WithMocks program, WebAppFactory.WithMocks.AndCustomOptions programOpts) :
        IClassFixture<WebAppFactory.WithMocks>,
        IClassFixture<WebAppFactory.WithMocks.AndCustomOptions>
    {
        private readonly HttpClient _httpClient = program.CreateClient();

        private readonly HttpClient _httpClientWithCustomOpts = programOpts.CreateClient();

        [Theory]
        [InlineData("/serilog-ui/api/keys/", 417)]
        [InlineData("/serilog-ui/api/logs/", 409)]
        [InlineData("/serilog-ui/", 418)]
        [InlineData("/serilog-ui/index.html", 400)]
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
        [InlineData("/test/", 418)]
        [InlineData("/test/index.html", 400)]
        public async Task It_hits_ui_endpoint_when_request_matches_method_and_custom_options_prefix(string pathReq, int statusCode)
        {
            // Act
            var send = await _httpClientWithCustomOpts.GetAsync(pathReq);

            // Assert
            send.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }
        
        [Theory]
        [InlineData("/serilog-ui/assets/index.js")]
        [InlineData("/serilog-ui/assets/index.js?query=query", "?query=query")]
        [InlineData("/serilog-ui/test/assets/index.js")]
        [InlineData("/serilog-ui/test/nested/assets/index.js")]
        [InlineData("/serilog-ui/test/nested/assets/index.js?query=query", "?query=query")]
        public async Task It_maps_request_to_assets_when_request_final_path_part_starts_with_assets_folder(string pathReq,
            string? additionalPart = null)
        {
            // Act
            var send = await _httpClient.GetAsync(pathReq);

            // Assert
            send.RequestMessage!.RequestUri!.AbsoluteUri.Should().EndWith($"/serilog-ui/assets/index.js{additionalPart}");
            send.StatusCode.Should().Be((HttpStatusCode)404);
        }

        [Theory]
        [InlineData("/serilog-ui/index.js")]
        [InlineData("/serilog-ui/assets")]
        [InlineData("/serilog-ui/assets.js")]
        [InlineData("/serilog-ui/asset/index.js?query=query")]
        [InlineData("/serilog-ui/test/asset/index.js")]
        [InlineData("/serilog-ui/test/nested/my-assets/index.js")]
        public async Task It_not_map_request_to_assets_when_request_final_path_part_not_match_assets_folder(string pathReq)
        {
            // Act
            var send = await _httpClient.GetAsync(pathReq);

            // Assert
            send.RequestMessage!.RequestUri!.AbsoluteUri.Should().NotEndWith("/serilog-ui/assets/index.js");
        }

        [Theory]
        [InlineData("fake-prefix/api/keys/", 417)]
        [InlineData("fake-prefix/api/logs/", 409)]
        [InlineData("/fake-prefix", 400)]
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
            var methods = new[]
            {
                HttpMethod.Delete, HttpMethod.Head, HttpMethod.Options,
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