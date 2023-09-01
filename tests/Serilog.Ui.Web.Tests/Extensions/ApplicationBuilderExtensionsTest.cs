using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Serilog.Ui.Web;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ui.Web.Tests.Utilities;
using Xunit;

namespace Ui.Web.Tests.Extensions
{
    [Trait("Ui-ApplicationBuilder", "Web")]
    public class ApplicationBuilderExtensionsTest : IClassFixture<WebAppFactory.WithMocks>
    {
        private readonly HttpClient client;

        public ApplicationBuilderExtensionsTest(WebAppFactory.WithMocks program)
        {
            client = program.CreateClient();
        }

        [Fact]
        public async Task It_register_ui_middleware()
        {
            var middlewareResponse = await client.GetAsync("/serilog-ui/index.html");

            middlewareResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)418, "because that means that the middleware isn't intercepting the request");
        }

        [Fact]
        public void It_throws_on_null_deps()
        {
            IApplicationBuilder builder = null!;

            var fail = () => builder.UseSerilogUi(null);

            fail.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void It_not_throws_on_null_parameters()
        {
            var webApp = WebApplication.CreateBuilder().Build();

            var fail = () => webApp.UseSerilogUi(null);

            fail.Should().NotThrow();
        }
    }
}
