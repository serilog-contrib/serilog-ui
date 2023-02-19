using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using MySql.Data.MySqlClient;
using MySql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using System.Threading.Tasks;
using Xunit;

namespace MySql.Tests.DataProvider
{
    [Trait("Integration-Pagination", "MySql")]
    public class DataProviderPaginationTest :
        IntegrationPaginationTests<MySqlTestProvider, MySqlTestcontainer, MySqlTestcontainerConfiguration>
    {
        public DataProviderPaginationTest(MySqlTestProvider instance) : base(instance)
        {
        }

        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<MySqlException>();
        }
    }
}
