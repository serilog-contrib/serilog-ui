using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MsSql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
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
    }
}
