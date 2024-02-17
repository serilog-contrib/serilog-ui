using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Serilog.Ui.Core;
using Serilog.Ui.Web.Endpoints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog.Ui.Core.Models;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            var aggregateDataProvider = new AggregateDataProvider(new IDataProvider[] { new FakeProvider(), new FakeSecondProvider() });
            _sut = new SerilogUiEndpoints(_loggerMock, aggregateDataProvider);
        }

        [Fact]
        public async Task It_gets_logs_keys()
        {
            // Act
            var result = await HappyPath<IEnumerable<string>>(_sut.GetApiKeysAsync);

            // Assert
            result.Should().ContainInOrder("FakeFirstProvider", "FakeSecondProvider");
        }

        [Fact]
        public async Task It_gets_logs()
        {
            // Act
            var result = await HappyPath<AnonymousObject>(_sut.GetLogsAsync);

            // Assert
            result.Count.Should().Be(10);
            result.CurrentPage.Should().Be(1);
            result.Total.Should().Be(100);
            result.Logs.Should().HaveCount(10);
        }

        [Fact]
        public async Task It_gets_logs_with_search_parameters()
        {
            // Arrange
            _testContext.Request.QueryString = new QueryString("?page=2&count=30&level=Verbose" +
                "&search=test&startDate=2020-01-02%2018:00:00&endDate=2020-02-02%2018:00:00&key=FakeSecondProvider");

            // Act
            var result = await HappyPath<AnonymousObject>(_sut.GetLogsAsync);

            // Assert
            result.Count.Should().Be(30);
            result.CurrentPage.Should().Be(2);
            result.Total.Should().Be(50);
            result.Logs.Should().HaveCount(5);
        }

        [Fact]
        public async Task It_serializes_an_error_on_exception()
        {
            // Arrange
            _testContext.Response.Body = new MemoryStream();
            var sut = new SerilogUiEndpoints(_loggerMock, new AggregateDataProvider(new[] { new BrokenProvider() }));
            
            // Act
            await sut.GetLogsAsync(_testContext);

            // Assert
            _testContext.Response.StatusCode.Should().Be(500);
            _testContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var result = await new StreamReader(_testContext.Response.Body).ReadToEndAsync();

            _testContext.Response.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
            _testContext.Response.ContentType.Should().Be("application/problem+json");
            
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(result)!;

            problemDetails.Title.Should().StartWith("An error occured");
            problemDetails.Detail.Should().NotBeNullOrWhiteSpace();
            problemDetails.Status.Should().Be((int)HttpStatusCode.InternalServerError);
            problemDetails.Extensions.Should().ContainKey("traceId");
            ((JsonElement) problemDetails.Extensions["traceId"]!).GetString().Should().NotBeNullOrWhiteSpace();
        }

        private async Task<T> HappyPath<T>(Func<HttpContext, Task> call)
        {
            // Arrange
            _testContext.Response.Body = new MemoryStream();

            // Act
            await call(_testContext);

            // Assert
            _testContext.Response.ContentType.Should().Be("application/json;charset=utf-8");
            _testContext.Response.StatusCode.Should().Be(200);
            _testContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var result = await new StreamReader(_testContext.Response.Body).ReadToEndAsync();
            return JsonSerializer.Deserialize<T>(result)!;
        }

        private class FakeProvider : IDataProvider
        {
            public string Name => "FakeFirstProvider";

            public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page,
                int count,
                string? level = null,
                string? searchCriteria = null,
                DateTime? startDate = null,
                DateTime? endDate = null,
                SearchOptions.SortProperty sortOn = SearchOptions.SortProperty.Timestamp,
                SearchOptions.SortDirection sortBy = SearchOptions.SortDirection.Desc)
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

            public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page,
                int count,
                string? level = null,
                string? searchCriteria = null,
                DateTime? startDate = null,
                DateTime? endDate = null,
                SearchOptions.SortProperty sortOn = SearchOptions.SortProperty.Timestamp,
                SearchOptions.SortDirection sortBy = SearchOptions.SortDirection.Desc)
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

        private class BrokenProvider : IDataProvider
        {
            public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(int page,
                int count,
                string level = null,
                string searchCriteria = null,
                DateTime? startDate = null,
                DateTime? endDate = null,
                SearchOptions.SortProperty sortOn = SearchOptions.SortProperty.Timestamp,
                SearchOptions.SortDirection sortBy = SearchOptions.SortDirection.Desc)
            {
                throw new NotImplementedException();
            }

            public string Name { get; } = "BrokenProvider";
        };
        
        private class AnonymousObject
        {
            [JsonPropertyName("logs")]
            public IEnumerable<LogModel>? Logs { get; set; }
            [JsonPropertyName("total")]
            public int Total { get; set; }
            [JsonPropertyName("count")]
            public int Count { get; set; }
            [JsonPropertyName("currentPage")]
            public int CurrentPage { get; set; }
        }
    }
}