using FluentAssertions;
using Microsoft.Data.Sqlite;
using MySql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.SqliteDataProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sqlite.Tests.DataProvider
{
    [Collection(nameof(SqliteDataProvider))]
    [Trait("Integration-Pagination", "Sqlite")]
    public class DataProviderPaginationTest : IntegrationPaginationTests<SqliteTestProvider>
    {
        public DataProviderPaginationTest(SqliteTestProvider instance) : base(instance)
        {
        }

        public override Task It_fetches_with_limit() => base.It_fetches_with_limit();

        public override Task It_fetches_with_limit_and_skip() => base.It_fetches_with_limit_and_skip();

        public override Task It_fetches_with_skip() => base.It_fetches_with_skip();

        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<SqliteException>();
        }
    }
}
