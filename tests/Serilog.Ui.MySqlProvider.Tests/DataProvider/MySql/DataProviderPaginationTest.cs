using System.Threading.Tasks;
using FluentAssertions;
using MySqlConnector;
using MySql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.MySqlProvider;
using Xunit;

namespace MySql.Tests.DataProvider.MySql
{
    [Collection(nameof(MySqlDataProvider))]
    [Trait("Integration-Pagination", "MySql")]
    public class DataProviderPaginationTest : IntegrationPaginationTests<MySqlTestProvider>
    {
        public DataProviderPaginationTest(MySqlTestProvider instance) : base(instance)
        {
        }

        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => Provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<MySqlException>();
        }
    }
}