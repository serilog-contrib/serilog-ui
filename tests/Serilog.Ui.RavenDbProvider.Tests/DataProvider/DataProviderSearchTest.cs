using RavenDb.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.RavenDbProvider;

namespace RavenDb.Tests.DataProvider;

[Collection(nameof(RavenDbDataProvider))]
[Trait("Integration-Search", "RavenDb")]
public class DataProviderSearchTest(RavenDbTestProvider instance) : IntegrationSearchTests<RavenDbTestProvider>(instance);