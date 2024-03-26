using RavenDb.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.RavenDbProvider;

namespace RavenDb.Tests.DataProvider;

[Collection(nameof(RavenDbDataProvider))]
[Trait("Integration-Search", "RavenDb")]
public class DataProviderSearchTest : IntegrationSearchTests<RavenDbTestProvider>
{
    public DataProviderSearchTest(RavenDbTestProvider instance) : base(instance)
    {
    }
}