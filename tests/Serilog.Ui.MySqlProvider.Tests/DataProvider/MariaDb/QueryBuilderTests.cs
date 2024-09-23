using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using MySql.Tests.Util;
using Serilog.Ui.Core.Models;
using Serilog.Ui.MySqlProvider;
using Serilog.Ui.MySqlProvider.Models;
using Xunit;

namespace MySql.Tests.DataProvider.MariaDb;

[Trait("Unit-QueryBuilder", "MariaDb")]
public class QueryBuilderTests
{
    [Theory]
    [ClassData(typeof(QueryBuilderTestData))]
    public void BuildFetchLogsQuery_ForMariaDblSink_ReturnsCorrectQuery(
        string schema,
        string tableName,
        string level,
        string searchCriteria,
        DateTime? startDate,
        DateTime? endDate,
        string expectedQuery)
    {
        // Arrange
        Dictionary<string, StringValues> queryLogs = new()
        {
            ["level"] = level,
            ["search"] = searchCriteria,
            ["startDate"] = startDate?.ToString("O"),
            ["endDate"] = endDate?.ToString("O")
        };

        MariaDbSinkColumnNames sinkColumns = new();
        MySqlQueryBuilder<MySqlLogModel> sut = new();

        // Act
        string query = sut.BuildFetchLogsQuery(sinkColumns, schema, tableName, FetchLogsQuery.ParseQuery(queryLogs));

        // Assert
        query.Should().Be(expectedQuery);
    }

    [Fact]
    public void BuildFetchLogsQuery_not_includes_Exception_if_custom_log_model()
    {
        // Arrange
        Dictionary<string, StringValues> queryLogs = new()
        {
            ["level"] = "level",
            ["search"] = "criteria"
        };

        MySqlSinkColumnNames sinkColumns = new();
        MySqlQueryBuilder<MariaDbTestModel> sut = new();

        // Act
        string query = sut.BuildFetchLogsQuery(sinkColumns, "test", "logs", FetchLogsQuery.ParseQuery(queryLogs));

        // Assert
        query.ToLowerInvariant().Should().StartWith("select");
        query.ToLowerInvariant().Should().NotContain("exception");
    }

    public class QueryBuilderTestData : IEnumerable<object[]>
    {
        private readonly List<object?[]> _data =
        [
            [
                "dbo", "logs", null!, null!, null!, null!,
                "SELECT Id, Message, LogLevel, TimeStamp, Exception, Properties FROM logs ORDER BY TimeStamp DESC LIMIT @Offset, @Count"
            ],
            [
                "dbo", "logs", null!, null!, null!, DateTime.Now,
                "SELECT Id, Message, LogLevel, TimeStamp, Exception, Properties FROM logs WHERE TRUE AND TimeStamp <= @EndDate ORDER BY TimeStamp DESC LIMIT @Offset, @Count"
            ],
            [
                "dbo", "logs", null!, null!, DateTime.Now, DateTime.Now,
                "SELECT Id, Message, LogLevel, TimeStamp, Exception, Properties FROM logs WHERE TRUE AND TimeStamp >= @StartDate AND TimeStamp <= @EndDate ORDER BY TimeStamp DESC LIMIT @Offset, @Count"
            ],
            [
                "dbo", "logs", "Information", null!, null!, null!,
                "SELECT Id, Message, LogLevel, TimeStamp, Exception, Properties FROM logs WHERE TRUE AND LogLevel = @Level ORDER BY TimeStamp DESC LIMIT @Offset, @Count"
            ],
            [
                "dbo", "logs", null!, "Test", null!, null!,
                "SELECT Id, Message, LogLevel, TimeStamp, Exception, Properties FROM logs WHERE TRUE AND (Message LIKE @Search OR Exception LIKE @Search) ORDER BY TimeStamp DESC LIMIT @Offset, @Count"
            ],
            [
                "dbo", "logs", "Information", "Test", null!, null!,
                "SELECT Id, Message, LogLevel, TimeStamp, Exception, Properties FROM logs WHERE TRUE AND LogLevel = @Level AND (Message LIKE @Search OR Exception LIKE @Search) ORDER BY TimeStamp DESC LIMIT @Offset, @Count"
            ],
            [
                "dbo", "logs", "Information", "Test", DateTime.Now, DateTime.Now,
                "SELECT Id, Message, LogLevel, TimeStamp, Exception, Properties FROM logs WHERE TRUE AND LogLevel = @Level AND (Message LIKE @Search OR Exception LIKE @Search) AND TimeStamp >= @StartDate AND TimeStamp <= @EndDate ORDER BY TimeStamp DESC LIMIT @Offset, @Count"
            ]
        ];

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}