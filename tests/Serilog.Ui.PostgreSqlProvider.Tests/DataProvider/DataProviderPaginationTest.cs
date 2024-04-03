using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Npgsql;
using Postgres.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.PostgreSqlProvider;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Core.Models;
using Xunit;

namespace Postgres.Tests.DataProvider
{
    [Collection(nameof(PostgresDataProvider))]
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
}