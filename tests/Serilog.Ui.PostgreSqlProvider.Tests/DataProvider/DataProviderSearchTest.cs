using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MsSql.Tests.DataProvider;
using Postgres.Tests.Util;
using Xunit;

namespace Postgres.Tests.DataProvider
{
    [Trait("Integration-Search", "Postgres")]
    public class DataProviderSearchTest :
        IntegrationSearchTests<PostgresTestProvider, PostgreSqlTestcontainer, PostgreSqlTestcontainerConfiguration>
    {
        public DataProviderSearchTest(PostgresTestProvider instance) : base(instance)
        {
        }
    }
}