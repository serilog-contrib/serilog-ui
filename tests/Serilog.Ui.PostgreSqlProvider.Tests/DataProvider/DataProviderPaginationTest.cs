using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Postgres.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
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
    }
}