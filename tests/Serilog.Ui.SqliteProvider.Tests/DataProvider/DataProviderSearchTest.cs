using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.SqliteDataProvider;
using Sqlite.Tests.Util;
using Xunit;

namespace Sqlite.Tests.DataProvider;

[Collection(nameof(SqliteDataProvider))]
[Trait("Integration-Search", "Sqlite")]
public class DataProviderSearchTest(SqliteTestProvider instance) : IntegrationSearchTests<SqliteTestProvider>(instance);
