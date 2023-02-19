using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Npgsql;
using Postgres.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using System.Threading.Tasks;
using Xunit;

namespace Postgres.Tests.DataProvider
{
    [Trait("Integration-Pagination", "Postgres")]
    public class DataProviderPaginationTest :
        IntegrationPaginationTests<PostgresTestProvider, PostgreSqlTestcontainer, PostgreSqlTestcontainerConfiguration>
    {
        public DataProviderPaginationTest(PostgresTestProvider instance) : base(instance)
        {
        }

        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<NpgsqlException>();
        }
    }
}