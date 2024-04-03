using MySql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.MySqlProvider;
using Xunit;

namespace MySql.Tests.DataProvider.MySql
{
    [Collection(nameof(MySqlDataProvider))]
    [Trait("Integration-Search", "MySql")]
    public class DataProviderSearchTest(MySqlTestProvider instance) : IntegrationSearchTests<MySqlTestProvider>(instance);
}