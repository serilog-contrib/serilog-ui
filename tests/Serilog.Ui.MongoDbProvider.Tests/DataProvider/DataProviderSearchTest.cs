using MongoDb.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.MongoDbProvider;
using Xunit;

namespace MongoDb.Tests.DataProvider
{
    [Collection(nameof(MongoDbDataProvider))]
    [Trait("Integration-Search", "MongoDb")]
    public class DataProviderSearchTest(BaseIntegrationTest instance) : IntegrationSearchTests<BaseIntegrationTest>(instance);
}