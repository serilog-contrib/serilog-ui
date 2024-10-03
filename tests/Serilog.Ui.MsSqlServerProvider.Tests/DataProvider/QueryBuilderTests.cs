using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using MsSql.Tests.Util;
using Serilog.Ui.Core.Models;
using Serilog.Ui.MsSqlServerProvider;
using Serilog.Ui.MsSqlServerProvider.Models;
using Xunit;

namespace MsSql.Tests.DataProvider;

[Trait("Unit-QueryBuilder", "MsSql")]
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

        SqlServerSinkColumnNames sinkColumns = new();
        SqlServerQueryBuilder<SqlServerLogModel> sut = new();

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

        SqlServerSinkColumnNames sinkColumns = new();
        SqlServerQueryBuilder<SqlServerTestModel> sut = new();

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
                "SELECT [Id], [Message], [Level], [TimeStamp], [Exception], [Properties] FROM [dbo].[logs] ORDER BY [TimeStamp] DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY"
            ],
            [
                "dbo", "logs", null!, null!, null!, DateTime.Now,
                "SELECT [Id], [Message], [Level], [TimeStamp], [Exception], [Properties] FROM [dbo].[logs] WHERE 1 = 1 AND [TimeStamp] <= @EndDate ORDER BY [TimeStamp] DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY"
            ],
            [
                "dbo", "logs", null!, null!, DateTime.Now, DateTime.Now,
                "SELECT [Id], [Message], [Level], [TimeStamp], [Exception], [Properties] FROM [dbo].[logs] WHERE 1 = 1 AND [TimeStamp] >= @StartDate AND [TimeStamp] <= @EndDate ORDER BY [TimeStamp] DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY"
            ],
            [
                "dbo", "logs", "Information", null!, null!, null!,
                "SELECT [Id], [Message], [Level], [TimeStamp], [Exception], [Properties] FROM [dbo].[logs] WHERE 1 = 1 AND [Level] = @Level ORDER BY [TimeStamp] DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY"
            ],
            [
                "dbo", "logs", null!, "Test", null!, null!,
                "SELECT [Id], [Message], [Level], [TimeStamp], [Exception], [Properties] FROM [dbo].[logs] WHERE 1 = 1 AND ([Message] LIKE @Search OR [Exception] LIKE @Search) ORDER BY [TimeStamp] DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY"
            ],
            [
                "dbo", "logs", "Information", "Test", null!, null!,
                "SELECT [Id], [Message], [Level], [TimeStamp], [Exception], [Properties] FROM [dbo].[logs] WHERE 1 = 1 AND [Level] = @Level AND ([Message] LIKE @Search OR [Exception] LIKE @Search) ORDER BY [TimeStamp] DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY"
            ],
            [
                "dbo", "logs", "Information", "Test", DateTime.Now, DateTime.Now,
                "SELECT [Id], [Message], [Level], [TimeStamp], [Exception], [Properties] FROM [dbo].[logs] WHERE 1 = 1 AND [Level] = @Level AND ([Message] LIKE @Search OR [Exception] LIKE @Search) AND [TimeStamp] >= @StartDate AND [TimeStamp] <= @EndDate ORDER BY [TimeStamp] DESC OFFSET @Offset ROWS FETCH NEXT @Count ROWS ONLY"
            ]
        ];

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}