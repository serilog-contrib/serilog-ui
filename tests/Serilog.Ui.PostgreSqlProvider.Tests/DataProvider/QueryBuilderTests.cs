using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Postgres.Tests.Util;
using Serilog.Ui.Core.Models;
using Serilog.Ui.PostgreSqlProvider;
using Serilog.Ui.PostgreSqlProvider.Models;
using Xunit;

namespace Postgres.Tests.DataProvider;

[Trait("Unit-QueryBuilder", "Postgres")]
public class QueryBuilderTests
{
    private readonly PostgreSqlAlternativeSinkColumnNames _sut = new();

    [Theory]
    [ClassData(typeof(QueryBuilderTestData))]
    public void BuildFetchLogsQuery_ForAlternativeSink_ReturnsCorrectQuery(
        string schema,
        string tableName,
        string level,
        string searchCriteria,
        DateTime? startDate,
        DateTime? endDate,
        string expectedQuery)
    {
        // Arrange
        var queryLogs = new Dictionary<string, StringValues>
        {
            ["level"] = level,
            ["search"] = searchCriteria,
            ["startDate"] = startDate?.ToString("O"),
            ["endDate"] = endDate?.ToString("O")
        };

        // Act
        var query = _sut.BuildFetchLogsQuery<PostgresLogModel>(schema, tableName, FetchLogsQuery.ParseQuery(queryLogs));

        // Assert
        query.Should().Be(expectedQuery);
    }

    [Fact]
    public void BuildFetchLogsQuery_not_includes_Exception_if_custom_log_model()
    {
        // Arrange
        var queryLogs = new Dictionary<string, StringValues>
        {
            ["level"] = "level",
            ["search"] = "criteria"
        };

        // Act
        var query = _sut.BuildFetchLogsQuery<PostgresTestModel>("test", "logs", FetchLogsQuery.ParseQuery(queryLogs));

        // Assert
        query.ToLowerInvariant().Should().StartWith("select *");
        query.ToLowerInvariant().Should().NotContain("exception");
    }

    public class QueryBuilderTestData : IEnumerable<object[]>
    {
        private readonly List<object?[]> _data =
        [
            [
                "dbo", "logs", null!, null!, null!, null!,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            ],
            [
                "dbo", "logs", null!, null!, DateTime.Now, null!,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Timestamp\" >= @StartDate ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            ],
            [
                "dbo", "logs", null!, null!, null!, DateTime.Now,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Timestamp\" <= @EndDate ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            ],
            [
                "dbo", "logs", null!, null!, DateTime.Now, DateTime.Now,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Timestamp\" >= @StartDate AND \"Timestamp\" <= @EndDate ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            ],
            [
                "dbo", "logs", "Information", null!, null!, null!,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Level\" = @Level ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            ],
            [
                "dbo", "logs", null!, "Test", null!, null!,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND (\"Message\" LIKE @Search OR \"Exception\" LIKE @Search) ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            ],
            [
                "dbo", "logs", "Information", "Test", null!, null!,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Level\" = @Level AND (\"Message\" LIKE @Search OR \"Exception\" LIKE @Search) ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            ],
            [
                "dbo", "logs", "Information", "Test", DateTime.Now, DateTime.Now,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Level\" = @Level AND (\"Message\" LIKE @Search OR \"Exception\" LIKE @Search) AND \"Timestamp\" >= @StartDate AND \"Timestamp\" <= @EndDate ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            ],
        ];

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}