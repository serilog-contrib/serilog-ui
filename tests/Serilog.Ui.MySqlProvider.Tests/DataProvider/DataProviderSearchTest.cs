using MySql.Tests.Util;
using Serilog.Ui.MySqlProvider;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Xunit;

namespace MySql.Tests.DataProvider
{
    [Collection(nameof(MySqlDataProvider))]
    [Trait("Integration-Search", "MySql")]
    public class DataProviderSearchTest : IntegrationSearchTests_Sink<MySqlTestProvider>
    {
        public DataProviderSearchTest(MySqlTestProvider instance) : base(instance)
        {
        }
    }
}