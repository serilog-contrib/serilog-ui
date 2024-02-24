using FluentAssertions;
using Microsoft.Data.SqlClient;
using MsSql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.MsSqlServerProvider;
using System.Threading.Tasks;
using Xunit;

namespace MsSql.Tests.DataProvider
{
    [Collection(nameof(SqlServerDataProvider))]
    [Trait("Integration-Pagination", "MsSql")]
    public class DataProviderPaginationTest : IntegrationPaginationTests<MsSqlServerTestProvider>
    {
        public DataProviderPaginationTest(MsSqlServerTestProvider instance) : base(instance)
        {
        }

        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => Provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<SqlException>();
        }
    }
}