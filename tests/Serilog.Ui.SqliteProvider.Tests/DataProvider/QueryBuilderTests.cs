using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Core.Models;
using Serilog.Ui.SqliteDataProvider;
using Serilog.Ui.SqliteDataProvider.Models;
using Xunit;

namespace Sqlite.Tests.DataProvider;

[Trait("Unit-QueryBuilder", "Sqlite")]
public class QueryBuilderTests
{
    [Theory]
    [ClassData(typeof(QueryBuilderTestData))]
    public void BuildFetchLogsQuery_ForSink_ReturnsCorrectQuery(
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

        SqliteSinkColumnNames sinkColumns = new();
        SqliteQueryBuilder sut = new();

        // Act
        string query = sut.BuildFetchLogsQuery(sinkColumns, schema, tableName, FetchLogsQuery.ParseQuery(queryLogs));

        // Assert
        query.Should().Be(expectedQuery);
    }

    public class QueryBuilderTestData : IEnumerable<object[]>
    {
        private readonly List<object?[]> _data =
        [
            [
                string.Empty, "Logs", null!, null!, null!, null!,
                "SELECT Id, RenderedMessage AS Message, Level, Timestamp, Exception, Properties FROM Logs ORDER BY Timestamp DESC LIMIT @Offset, @Count"
            ],
            [
                string.Empty, "Logs", null!, null!, null!, DateTime.Now,
                "SELECT Id, RenderedMessage AS Message, Level, Timestamp, Exception, Properties FROM Logs WHERE Timestamp <= @EndDate ORDER BY Timestamp DESC LIMIT @Offset, @Count"
            ],
            [
                string.Empty, "Logs", null!, null!, DateTime.Now, DateTime.Now,
                "SELECT Id, RenderedMessage AS Message, Level, Timestamp, Exception, Properties FROM Logs WHERE Timestamp >= @StartDate AND Timestamp <= @EndDate ORDER BY Timestamp DESC LIMIT @Offset, @Count"
            ],
            [
                string.Empty, "Logs", "Information", null!, null!, null!,
                "SELECT Id, RenderedMessage AS Message, Level, Timestamp, Exception, Properties FROM Logs WHERE Level = @Level ORDER BY Timestamp DESC LIMIT @Offset, @Count"
            ],
            [
                string.Empty, "Logs", null!, "Test", null!, null!,
                "SELECT Id, RenderedMessage AS Message, Level, Timestamp, Exception, Properties FROM Logs WHERE (RenderedMessage LIKE @Search OR Exception LIKE @Search) ORDER BY Timestamp DESC LIMIT @Offset, @Count"
            ],
            [
                string.Empty, "Logs", "Information", "Test", null!, null!,
                "SELECT Id, RenderedMessage AS Message, Level, Timestamp, Exception, Properties FROM Logs WHERE Level = @Level AND (RenderedMessage LIKE @Search OR Exception LIKE @Search) ORDER BY Timestamp DESC LIMIT @Offset, @Count"
            ],
            [
                string.Empty, "Logs", "Information", "Test", DateTime.UtcNow, DateTime.UtcNow,
                "SELECT Id, RenderedMessage AS Message, Level, Timestamp, Exception, Properties FROM Logs WHERE Level = @Level AND (RenderedMessage LIKE @Search OR Exception LIKE @Search) AND Timestamp >= @StartDate AND Timestamp <= @EndDate ORDER BY Timestamp DESC LIMIT @Offset, @Count"
            ]
        ];

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}