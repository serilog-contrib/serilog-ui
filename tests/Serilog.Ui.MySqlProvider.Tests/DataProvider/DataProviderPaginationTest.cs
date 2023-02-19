using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MySql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
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
    }
}
