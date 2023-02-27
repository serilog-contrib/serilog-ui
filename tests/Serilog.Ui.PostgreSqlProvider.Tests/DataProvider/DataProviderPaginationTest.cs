using FluentAssertions;
using Npgsql;
using Postgres.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using System.Threading.Tasks;
using Xunit;

namespace Postgres.Tests.DataProvider
{
    [CollectionDefinition(nameof(PostgresTestProvider))]
    [Trait("Integration-Pagination", "Postgres")]
    public class DataProviderPaginationTest : IntegrationPaginationTests<PostgresTestProvider>
    {
        public DataProviderPaginationTest(PostgresTestProvider instance) : base(instance)
        {
        }

        public override Task It_fetches_with_limit() => base.It_fetches_with_limit();

        public override Task It_fetches_with_limit_and_skip() => base.It_fetches_with_limit_and_skip();

        public override Task It_fetches_with_skip() => base.It_fetches_with_skip();

        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<NpgsqlException>();
        }
    }
}