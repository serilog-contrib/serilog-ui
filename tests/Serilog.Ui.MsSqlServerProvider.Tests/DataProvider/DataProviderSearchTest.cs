using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MsSql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Xunit;

namespace MsSql.Tests.DataProvider
{
    [Trait("Integration-Search", "MsSql")]
    public class DataProviderSearchTest :
        IntegrationSearchTests<MsSqlServerTestProvider, MsSqlTestcontainer, MsSqlTestcontainerConfiguration>
    {
        public DataProviderSearchTest(MsSqlServerTestProvider instance) : base(instance)
        {
        }
    }
}