﻿using FluentAssertions;
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
        private readonly HttpClient _client;

        public ApplicationBuilderExtensionsTest(WebAppFactory.WithMocks program)
        {
            _client = program.CreateClient();
        }

        [Fact]
        public async Task It_register_ui_middleware()
        {
            // Act
            var middlewareResponse = await _client.GetAsync("/serilog-ui/index.html");

            // Assert
            middlewareResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)418, "because that means that the middleware isn't intercepting the request");
        }

        [Fact]
        public void It_throws_on_null_deps()
        {
            // Arrange
            IApplicationBuilder builder = null!;

            // Act
            var fail = () => builder.UseSerilogUi(null);

            // Assert
            fail.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void It_not_throws_on_null_parameters()
        {
            // Arrange
            var webApp = WebApplication.CreateBuilder().Build();

            // Act
            var fail = () => webApp.UseSerilogUi(null);

            // Assert
            fail.Should().NotThrow();
        }
    }
}
