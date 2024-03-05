using System.Linq;
using FluentAssertions;
using Npgsql;
using Postgres.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.PostgreSqlProvider;
using System.Threading.Tasks;
using Serilog.Ui.Core.Models;
using Xunit;

namespace Postgres.Tests.DataProvider
{
    [Collection(nameof(PostgresDataProvider))]
    [Trait("Integration-Pagination", "Postgres")]
    public class DataProviderPaginationTest : IntegrationPaginationTests<PostgresTestProvider>
    {
        public DataProviderPaginationTest(PostgresTestProvider instance) : base(instance)
        {
        }
        
        [Fact]
        public override async Task It_fetches_with_sort_by_level()
        {
            var (descLogs, _) =
                await Provider.FetchDataAsync(1, 50, sortOn: SearchOptions.SortProperty.Level, sortBy: SearchOptions.SortDirection.Desc);

            var descLogsToOriginalPostgres = descLogs.Select(e => LogLevelConverter.GetLevelValue(e.Level));
            descLogsToOriginalPostgres.Should().NotBeEmpty().And.BeInDescendingOrder();

            var (ascLogs, _) =
                await Provider.FetchDataAsync(1, 50, sortOn: SearchOptions.SortProperty.Level, sortBy: SearchOptions.SortDirection.Asc);

            var ascLogsToOriginalPostgres = ascLogs.Select(e => LogLevelConverter.GetLevelValue(e.Level));
            ascLogsToOriginalPostgres.Should().NotBeEmpty().And.BeInAscendingOrder();
        }

        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => Provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<NpgsqlException>();
        }
    }
}