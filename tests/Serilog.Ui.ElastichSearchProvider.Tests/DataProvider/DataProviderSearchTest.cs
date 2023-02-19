using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using ElasticSearch.Tests.Util;
using MsSql.Tests.DataProvider;
using Xunit;

namespace ElasticSearch.Tests.DataProvider
{
    [Trait("Integration-Search", "Elastic")]

    public class DataProviderSearchTest :
        IntegrationSearchTests<ElasticSearchTestProvider, ElasticsearchTestcontainer, ElasticsearchTestcontainerConfiguration>
    {
        public DataProviderSearchTest(ElasticSearchTestProvider instance) : base(instance)
        {
        }
    }
}