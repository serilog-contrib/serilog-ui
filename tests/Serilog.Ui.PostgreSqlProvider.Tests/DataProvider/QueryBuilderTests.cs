using Serilog.Ui.PostgreSqlProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Postgres.Tests.DataProvider;

[Trait("Unit-QueryBuilder", "Postgres")]
public class QueryBuilderTests
{
    [Theory]
    [ClassData(typeof(QueryBuilderTestData))]
    public void BuildFetchLogsQuery_ForAlternativeSink_ReturnsCorrectQuery(
        string schema,
        string tableName,
        string level,
        string searchCriteria,
        ref DateTime? startDate,
        ref DateTime? endDate,
        string expectedQuery)
    {
        // Arrange
        QueryBuilder.SetSinkType(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative);

        // Act
        var query = QueryBuilder.BuildFetchLogsQuery(schema, tableName, level, searchCriteria, ref startDate, ref endDate);

        // Assert
        Assert.Equal(expectedQuery, query);
    }

    public class QueryBuilderTestData : IEnumerable<object[]>
    {
        private readonly List<object?[]> _data = new()
        {
            new object[]
            {
                "dbo", "logs", null!, null!, null!, null!,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            },
            new object[]
            {
                "dbo", "logs", null!, null!, DateTime.Now, null!,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Timestamp\" >= @StartDate ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            },
            new object[]
            {
                "dbo", "logs", null!, null!, null!, DateTime.Now,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Timestamp\" <= @EndDate ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            },
            new object[]
            {
                "dbo", "logs", null!, null!, DateTime.Now, DateTime.Now,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Timestamp\" >= @StartDate AND \"Timestamp\" <= @EndDate ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            },
            new object[]
            {
                "dbo", "logs", "Information", null!, null!, null!,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Level\" = @Level ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            },
            new object[]
            {
                "dbo", "logs", null!, "Test", null!, null!,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND (\"Message\" LIKE @Search OR \"Exception\" LIKE @Search) ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            },
            new object[]
            {
                "dbo", "logs", "Information", "Test", null!, null!,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Level\" = @Level AND (\"Message\" LIKE @Search OR \"Exception\" LIKE @Search) ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            },
            new object[]
            {
                "dbo", "logs", "Information", "Test", DateTime.Now, DateTime.Now,
                "SELECT \"Message\", \"MessageTemplate\", \"Level\", \"Timestamp\", \"Exception\", \"LogEvent\" AS \"Properties\" FROM \"dbo\".\"logs\" WHERE TRUE AND \"Level\" = @Level AND (\"Message\" LIKE @Search OR \"Exception\" LIKE @Search) AND \"Timestamp\" >= @StartDate AND \"Timestamp\" <= @EndDate ORDER BY \"Timestamp\" DESC LIMIT @Count OFFSET @Offset"
            },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}