using MsSql.Tests.Util;
using Serilog.Ui.MsSqlServerProvider;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Xunit;

namespace MsSql.Tests.DataProvider
{
    [Collection(nameof(SqlServerDataProvider))]
    [Trait("Integration-Search", "MsSql")]
    public class DataProviderSearchTest : IntegrationSearchTests<MsSqlServerTestProvider>
    {
        public DataProviderSearchTest(MsSqlServerTestProvider instance) : base(instance) { }
    }
}