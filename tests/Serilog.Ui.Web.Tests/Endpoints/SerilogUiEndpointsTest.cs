using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Web.Endpoints;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Serilog.Ui.Web.Tests.Endpoints;

[Trait("Ui-Api-Endpoints", "Web")]
public class SerilogUiEndpointsTest
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<SerilogUiEndpoints> _loggerMock;
    private readonly SerilogUiEndpoints _sut;
    private readonly DefaultHttpContext _testContext;

    public SerilogUiEndpointsTest()
    {
        _loggerMock = Substitute.For<ILogger<SerilogUiEndpoints>>();
        _testContext = new DefaultHttpContext
        {
            Request =
            {
                Host = new HostString("test.dev"),
                Scheme = "https"
            }
        };
        _contextAccessor = Substitute.For<IHttpContextAccessor>();
        _contextAccessor.HttpContext.Returns(_testContext);
        AggregateDataProvider aggregateDataProvider =
            new(new IDataProvider[] { new FakeProvider(), new FakeSecondProvider() });
        _sut = new SerilogUiEndpoints(_contextAccessor, _loggerMock, aggregateDataProvider);
    }

    [Fact]
    public async Task It_gets_logs_keys()
    {
        // Act
        IEnumerable<string>? result = await HappyPath<IEnumerable<string>>(_sut.GetApiKeysAsync);

        // Assert
        result.Should().ContainInOrder("FakeFirstProvider", "FakeSecondProvider");
    }

    [Fact]
    public async Task It_gets_logs()
    {
        // Act
        AnonymousObject? result = await HappyPath<AnonymousObject>(_sut.GetLogsAsync);

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
        _testContext.Request.QueryString =
            new QueryString(
                "?page=2&count=30&level=Verbose&search=test&startDate=2020-01-02%2018:00:00&endDate=2020-02-02%2018:00:00&key=FakeSecondProvider");

        // Act
        AnonymousObject result = await HappyPath<AnonymousObject>(_sut.GetLogsAsync);

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
        SerilogUiEndpoints sut = new(_contextAccessor, _loggerMock,
            new AggregateDataProvider(new[] { new BrokenProvider() }));

        // Act
        await sut.GetLogsAsync();

        // Assert
        _testContext.Response.StatusCode.Should().Be(500);
        _testContext.Response.Body.Seek(0, SeekOrigin.Begin);
        string result = await new StreamReader(_testContext.Response.Body).ReadToEndAsync();

        _testContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        _testContext.Response.ContentType.Should().Be("application/problem+json");

        ProblemDetails problemDetails = JsonSerializer.Deserialize<ProblemDetails>(result)!;

        problemDetails.Title.Should().StartWith("An error occured");
        problemDetails.Detail.Should().NotBeNullOrWhiteSpace();
        problemDetails.Status.Should().Be((int)HttpStatusCode.InternalServerError);
        problemDetails.Extensions.Should().ContainKey("traceId");
        ((JsonElement)problemDetails.Extensions["traceId"]!).GetString().Should().NotBeNullOrWhiteSpace();
    }

    private async Task<T> HappyPath<T>(Func<Task> call)
    {
        // Arrange
        _testContext.Response.Body = new MemoryStream();

        // Act
        await call();

        // Assert
        _testContext.Response.ContentType.Should().Be("application/json;charset=utf-8");
        _testContext.Response.StatusCode.Should().Be(200);
        _testContext.Response.Body.Seek(0, SeekOrigin.Begin);
        string result = await new StreamReader(_testContext.Response.Body).ReadToEndAsync();
        return JsonSerializer.Deserialize<T>(result)!;
    }

    private class FakeProvider : IDataProvider
    {
        public string Name => "FakeFirstProvider";

        public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams,
            CancellationToken cancellationToken = default)
        {
            if (queryParams.Page != 0 ||
                queryParams.Count != 10 ||
                !string.IsNullOrWhiteSpace(queryParams.Level) ||
                !string.IsNullOrWhiteSpace(queryParams.SearchCriteria) ||
                queryParams.StartDate != null ||
                queryParams.EndDate != null)
                return Task.FromResult<(IEnumerable<LogModel>, int)>((Array.Empty<LogModel>(), 0));

            LogModel[] modelArray = new LogModel[10];
            Array.Fill(modelArray, new LogModel());
            return Task.FromResult<(IEnumerable<LogModel>, int)>((modelArray, 100));
        }

        public Task<LogStatisticModel> FetchDashboardAsync(CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();
    }

    private class FakeSecondProvider : IDataProvider
    {
        public string Name => "FakeSecondProvider";

        public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams,
            CancellationToken cancellationToken = default)
        {
            if (queryParams.Page != 1 ||
                queryParams.Count != 30 ||
                !(queryParams.Level?.Equals("Verbose") ?? false) ||
                !(queryParams.SearchCriteria?.Equals("test") ?? false) ||
                !queryParams.StartDate.HasValue || queryParams.StartDate.Value.Equals(DateTime.MinValue) ||
                !queryParams.EndDate.HasValue || queryParams.EndDate.Value.Equals(DateTime.MinValue))
                return Task.FromResult<(IEnumerable<LogModel>, int)>((Array.Empty<LogModel>(), 0));

            LogModel[] modelArray = new LogModel[5];
            Array.Fill(modelArray, new LogModel());
            return Task.FromResult<(IEnumerable<LogModel>, int)>((modelArray, 50));
        }

        public Task<LogStatisticModel> FetchDashboardAsync(CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();
    }

    private class BrokenProvider : IDataProvider
    {
        public Task<(IEnumerable<LogModel>, int)> FetchDataAsync(FetchLogsQuery queryParams,
            CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public Task<LogStatisticModel> FetchDashboardAsync(CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public string Name { get; } = "BrokenProvider";
    }

    private record AnonymousObject
    {
        [JsonPropertyName("logs")] public IEnumerable<LogModel>? Logs { get; set; }

        [JsonPropertyName("total")] public int Total { get; set; }

        [JsonPropertyName("count")] public int Count { get; set; }

        [JsonPropertyName("currentPage")] public int CurrentPage { get; set; }
    }
}