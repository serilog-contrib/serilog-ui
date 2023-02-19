using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using MsSql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using System.Threading.Tasks;
using Xunit;

namespace MsSql.Tests.DataProvider
{
    [Trait("Integration-Pagination", "MsSql")]
    public class DataProviderPaginationTest :
        IntegrationPaginationTests<MsSqlServerTestProvider, MsSqlTestcontainer, MsSqlTestcontainerConfiguration>
    {
        public DataProviderPaginationTest(MsSqlServerTestProvider instance) : base(instance)
        {
        }

        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<SqlException>();
        }
    }
}
