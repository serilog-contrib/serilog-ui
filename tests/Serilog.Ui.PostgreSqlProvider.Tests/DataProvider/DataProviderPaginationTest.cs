using FluentAssertions;
using Npgsql;
using Postgres.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.PostgreSqlProvider;
using System.Threading.Tasks;
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
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => Provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<NpgsqlException>();
        }
    }
}