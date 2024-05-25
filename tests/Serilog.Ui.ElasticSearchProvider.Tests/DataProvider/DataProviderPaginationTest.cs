using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using ElasticSearch.Tests.Util;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.Core.Models;
using Xunit;

namespace ElasticSearch.Tests.DataProvider
{
    [Trait("Integration-Pagination", "Elastic")]
    public class DataProviderPaginationTest(ElasticTestProvider instance) : IntegrationPaginationTests<ElasticTestProvider>(instance),
        IClassFixture<ElasticTestProvider>,
        IClusterFixture<Elasticsearch7XCluster>
    {
        [I]
        public override Task It_throws_when_skip_is_zero()
        {
            var query = new Dictionary<string, StringValues> { ["page"] = "0", ["count"] = "1" };
            var test = () => Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            return test.Should().NotThrowAsync("because Elastic Client catches the error");
        }

        [Fact(Skip = "sort by level is disabled in Elastic Search provider.")]
        public override Task It_fetches_with_sort_by_level()
        {
            return Task.CompletedTask;
        }

        [Fact(Skip = "sort by message is disabled in Elastic Search provider.")]
        public override Task It_fetches_with_sort_by_message()
        {
            return Task.CompletedTask;
        }
    }
}