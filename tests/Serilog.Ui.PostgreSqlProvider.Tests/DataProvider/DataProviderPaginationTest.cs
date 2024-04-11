using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Npgsql;
using Postgres.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.Core.Models;
using Serilog.Ui.PostgreSqlProvider;
using Xunit;

namespace Postgres.Tests.DataProvider;

[Collection(nameof(PostgresTestProvider))]
[Trait("Integration-Pagination", "Postgres")]
public class DataProviderPaginationTest(PostgresTestProvider instance) : IntegrationPaginationTests<PostgresTestProvider>(instance)
{
    [Fact]
    public override async Task It_fetches_with_sort_by_level()
    {
        var query = new Dictionary<string, StringValues>
        {
            ["page"] = "1",
            ["count"] = "50",
            ["sortOn"] = $"{SearchOptions.SortProperty.Level}",
            ["sortBy"] = $"{SearchOptions.SortDirection.Desc}"
        };
        var (descLogs, _) =
            await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

        var descLogsToOriginalPostgres = descLogs.Select(e => LogLevelConverter.GetLevelValue(e.Level));
        descLogsToOriginalPostgres.Should().NotBeEmpty().And.BeInDescendingOrder();

        query["sortOn"] = $"{SearchOptions.SortProperty.Level}";
        query["sortBy"] = $"{SearchOptions.SortDirection.Asc}";
        var (ascLogs, _) =
            await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

        var ascLogsToOriginalPostgres = ascLogs.Select(e => LogLevelConverter.GetLevelValue(e.Level));
        ascLogsToOriginalPostgres.Should().NotBeEmpty().And.BeInAscendingOrder();
    }

    [Fact]
    public override Task It_throws_when_skip_is_zero()
    {
        var query = new Dictionary<string, StringValues> { ["page"] = "0", ["count"] = "1" };
        var test = () => Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
        return test.Should().ThrowAsync<NpgsqlException>();
    }
}

[Collection(nameof(PostgresAdditionalColsTestProvider))]
[Trait("Integration-Pagination-AdditionalColumns", "Postgres")]
public class DataProviderPaginationWithColsTests(PostgresAdditionalColsTestProvider instance)
    : IntegrationPaginationTests<PostgresAdditionalColsTestProvider>(instance)
{
    [Fact]
    public override async Task It_fetches_with_sort_by_level()
    {
        var query = new Dictionary<string, StringValues>
        {
            ["page"] = "1",
            ["count"] = "50",
            ["sortOn"] = $"{SearchOptions.SortProperty.Level}",
            ["sortBy"] = $"{SearchOptions.SortDirection.Desc}"
        };
        var (descLogs, _) =
            await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

        var descLogsToOriginalPostgres = descLogs.Select(e => LogLevelConverter.GetLevelValue(e.Level));
        descLogsToOriginalPostgres.Should().NotBeEmpty().And.BeInDescendingOrder();

        query["sortOn"] = $"{SearchOptions.SortProperty.Level}";
        query["sortBy"] = $"{SearchOptions.SortDirection.Asc}";
        var (ascLogs, _) =
            await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

        var ascLogsToOriginalPostgres = ascLogs.Select(e => LogLevelConverter.GetLevelValue(e.Level));
        ascLogsToOriginalPostgres.Should().NotBeEmpty().And.BeInAscendingOrder();
    }

    [Fact]
    public override Task It_throws_when_skip_is_zero()
    {
        var query = new Dictionary<string, StringValues> { ["page"] = "0", ["count"] = "1" };
        var test = () => Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
        return test.Should().ThrowAsync<NpgsqlException>();
    }
}