using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Web.Extensions;
using Serilog.Ui.Web.Tests.Utilities;
using Xunit;

namespace Serilog.Ui.Web.Tests.Extensions
{
    [Trait("Ui-ApplicationBuilder", "Web")]
    public class ApplicationBuilderExtensionsTest(WebAppFactory.WithMocks program) : IClassFixture<WebAppFactory.WithMocks>
    {
        private readonly HttpClient _client = program.CreateClient();

        [Fact]
        public async Task It_register_ui_middleware()
        {
            // Act
            var middlewareResponse = await _client.GetAsync("/serilog-ui/");

            // Assert
            middlewareResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)418,
                "because that means that the middleware isn't intercepting the request");
        }

        [Fact]
        public void It_throws_on_null_deps()
        {
            // Arrange
            IApplicationBuilder builder = null!;

            // Act
            var fail = () => builder.UseSerilogUi();

            // Assert
            fail.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void It_not_throws_on_null_parameters()
        {
            // Arrange
            var webApp = WebApplication.CreateBuilder().Build();

            // Act
            var fail = () => webApp.UseSerilogUi();

            // Assert
            fail.Should().NotThrow();
        }

        [Fact]
        public void It_register_disabled_keys_if_found()
        {
            // Arrange
            var appBuilder = WebApplication.CreateBuilder();
            appBuilder.Services
                .AddScoped<IDataProvider, ElasticSearchFakeProvider>()
                .AddScoped<IDataProvider, NotDisabledFakeProvider>();
            var app = appBuilder.Build();
            
            // Act
            var expectedData = Array.Empty<string>();
            app.UseSerilogUi(opt => { expectedData = opt.DisabledSortOnKeys.ToArray(); });

            // Assert
            expectedData.Should().BeEquivalentTo(["Test"]);
        }

        private class ElasticSearchFakeProvider : IDataProvider
        {
            public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
                => throw new NotImplementedException();

            public string Name => "Test";
        }
        private class NotDisabledFakeProvider : IDataProvider
        {
            public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams, CancellationToken cancellationToken = default)
                => throw new NotImplementedException();

            public string Name => "TestNotDisabled";
        }
    }
}