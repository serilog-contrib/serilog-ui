using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using ElasticSearch.Tests.Util;
using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using System.Threading.Tasks;
using Xunit;

namespace ElasticSearch.Tests.DataProvider
{
    [Trait("Integration-Pagination", "Elastic")]
    public class DataProviderPaginationTest : IntegrationPaginationTests<ElasticTestProvider>,
        IClassFixture<ElasticTestProvider>,
        IClusterFixture<Elasticsearch7XCluster>
    {
        public DataProviderPaginationTest(ElasticTestProvider instance) : base(instance) { }

        [I]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => Provider.FetchDataAsync(0, 1);
            return test.Should().NotThrowAsync("because Elastic Client catches the error");
        }
    }
}
