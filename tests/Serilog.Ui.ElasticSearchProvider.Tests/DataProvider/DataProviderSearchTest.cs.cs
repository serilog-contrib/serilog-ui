using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using ElasticSearch.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Xunit;

namespace ElasticSearch.Tests.DataProvider
{
    [Trait("Integration-Search", "Elastic")]
    public class DataProviderSearchTest : IntegrationSearchTests<ElasticTestProvider>,
        IClassFixture<ElasticTestProvider>,
        IClusterFixture<Elasticsearch7XCluster>
    {
        public DataProviderSearchTest(ElasticTestProvider instance) : base(instance)
        {
        }
    }
}