using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Newtonsoft.Json;
using Serilog.Ui.Core;
using Serilog.Ui.Web.Endpoints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Ui.Web.Tests.Endpoints
{
    [Trait("Ui-Api-Endpoints", "Web")]
    public class SerilogUiEndpointsTest
    {
        private readonly ILogger<SerilogUiEndpoints> _loggerMock;
        private readonly SerilogUiEndpoints _sut;
        private readonly DefaultHttpContext _testContext;

        public SerilogUiEndpointsTest()
        {
            _loggerMock = Substitute.For<ILogger<SerilogUiEndpoints>>();
            _testContext = new DefaultHttpContext();
            _testContext.Request.Host = new HostString("test.dev");
            _testContext.Request.Scheme = "https";
            _sut = new SerilogUiEndpoints(_loggerMock);
        }

        [Fact]
        public async Task It_gets_logs_keys()
        {
            var result = await HappyPath<IEnumerable<string>>(_sut.GetApiKeys);
            result.Should().ContainInOrder("FakeFirstProvider", "FakeSecondProvider");
        }

        [Fact]
        public async Task It_gets_logs()
        {
            var result = await HappyPath<AnonymousObject>(_sut.GetLogs);
            result.Count.Should().Be(10);
            result.CurrentPage.Should().Be(1);
            result.Total.Should().Be(100);
            result.Logs.Should().HaveCount(10);
        }

        [Fact]
        public async Task It_gets_logs_with_search_parameters()
        {
            _testContext.Request.QueryString = new QueryString("?page=2&count=30&level=Verbose" +
                "&search=test&startDate=2020-01-02%2018:00:00&endDate=2020-02-02%2018:00:00&key=FakeSecondProvider");
            var result = await HappyPath<AnonymousObject>(_sut.GetLogs);
            result.Count.Should().Be(30);
            result.CurrentPage.Should().Be(2);
            result.Total.Should().Be(50);
            result.Logs.Should().HaveCount(5);
        }

        [Fact]
        public async Task It_serializes_an_error_on_exception()
        {
            _testContext.Response.Body = new MemoryStream();
            await _sut.GetLogs(_testContext);

            _testContext.Response.StatusCode.Should().Be(500);
            _testContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var result = await new StreamReader(_testContext.Response.Body).ReadToEndAsync();
            result.Should().Be("{\"errorMessage\":\"{\\\"errorMessage\\\":\\\"Value cannot be null. (Parameter 'provider')\\\"}\"}");
        }

        private async Task<T> HappyPath<T>(Func<HttpContext, Task> call)
        {
            var mockProvider = Substitute.For<IServiceProvider>();
            mockProvider
                .GetService(typeof(AggregateDataProvider))
                .Returns(new AggregateDataProvider(new IDataProvider[] { new FakeProvider(), new FakeSecondProvider() }));
            _testContext.RequestServices = mockProvider;
            _testContext.Response.Body = new MemoryStream();

            await call(_testContext);

            _testContext.Response.ContentType.Should().Be("application/json;charset=utf-8");
            _testContext.Response.StatusCode.Should().Be(200);
            mockProvider.Received().GetService(typeof(AggregateDataProvider));
            _testContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var result = await new StreamReader(_testContext.Response.Body).ReadToEndAsync();
            return JsonConvert.DeserializeObject<T>(result)!;
        }

        private class FakeProvider : IDataProvider
        {
            public string Name => "FakeFirstProvider";

            public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page, int count, string? level = null, string? searchCriteria = null, DateTime? startDate = null, DateTime? endDate = null)
            {
                if (page != 1 ||
                    count != 10 ||
                    !string.IsNullOrWhiteSpace(level) ||
                    !string.IsNullOrWhiteSpace(searchCriteria) ||
                    startDate != null ||
                    endDate != null)
                    return Task.FromResult<(IEnumerable<LogModel>, int)>((Array.Empty<LogModel>(), 0));

                var modelArray = new LogModel[10];
                Array.Fill(modelArray, new());
                return Task.FromResult<(IEnumerable<LogModel>, int)>((modelArray, 100));
            }
        }

        private class FakeSecondProvider : IDataProvider
        {
            public string Name => "FakeSecondProvider";

            public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page, int count, string? level = null, string? searchCriteria = null, DateTime? startDate = null, DateTime? endDate = null)
            {
                if (page != 2 ||
                    count != 30 ||
                    !(level?.Equals("Verbose") ?? false) ||
                    !(searchCriteria?.Equals("test") ?? false) ||
                    !startDate.HasValue || startDate.Value.Equals(DateTime.MinValue) ||
                    !endDate.HasValue || endDate.Value.Equals(DateTime.MinValue))
                    return Task.FromResult<(IEnumerable<LogModel>, int)>((Array.Empty<LogModel>(), 0));

                var modelArray = new LogModel[5];
                Array.Fill(modelArray, new());
                return Task.FromResult<(IEnumerable<LogModel>, int)>((modelArray, 50));
            }
        }

        private class AnonymousObject
        {
            [JsonProperty("logs")]
            public IEnumerable<LogModel>? Logs { get; set; }
            [JsonProperty("total")]
            public int Total { get; set; }
            [JsonProperty("count")]
            public int Count { get; set; }
            [JsonProperty("currentPage")]
            public int CurrentPage { get; set; }
        }
    }
}