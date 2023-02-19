using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Elasticsearch.Net;
using ElasticSearch.Tests.Util;
using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using System.Threading.Tasks;
using Xunit;

namespace ElasticSearch.Tests.DataProvider
{
    [Trait("Integration-Pagination", "Elastic")]
    public class DataProviderPaginationTest :
        IntegrationPaginationTests<ElasticSearchTestProvider, ElasticsearchTestcontainer, ElasticsearchTestcontainerConfiguration>
    {
        public DataProviderPaginationTest(ElasticSearchTestProvider instance) : base(instance)
        {
        }

        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<ElasticsearchClientException>();
        }
    }
}
