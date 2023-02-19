using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MsSql.Tests.DataProvider;
using MySql.Tests.Util;
using Xunit;

namespace MySql.Tests.DataProvider
{
    [Trait("Integration-Search", "MySql")]
    public class DataProviderSearchTest :
        IntegrationSearchTests<MySqlTestProvider, MySqlTestcontainer, MySqlTestcontainerConfiguration>
    {
        public DataProviderSearchTest(MySqlTestProvider instance) : base(instance)
        {
        }
    }
}