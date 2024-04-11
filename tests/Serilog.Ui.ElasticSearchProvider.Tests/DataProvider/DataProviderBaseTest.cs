using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using Elasticsearch.Net;
using ElasticSearch.Tests.Util;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Nest;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core.Models;
using Serilog.Ui.ElasticSearchProvider;
using Xunit;

namespace ElasticSearch.Tests.DataProvider
{
    [Trait("Unit-Base", "Elastic")]
    public class DataProviderBaseTest : IUnitBaseTests, IClusterFixture<Elasticsearch7XCluster>
    {
        [U]
        public void It_throws_when_any_dependency_is_null()
        {
            var suts = new List<Func<ElasticSearchDbDataProvider>>
            {
                () => new ElasticSearchDbDataProvider(null, new()),
                () => new ElasticSearchDbDataProvider(new ElasticClient(), null),
            };

            suts.ForEach(sut => sut.Should().ThrowExactly<ArgumentNullException>());
        }

        [U]
        public Task It_logs_and_throws_when_db_read_breaks_down()
        {
            var elasticClientMock = Substitute.For<IElasticClient>();
            elasticClientMock
                .SearchAsync<ElasticSearchDbLogModel>(Arg.Any<ISearchRequest>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new ElasticsearchClientException("no connection to db"));

            var sut = new ElasticSearchDbDataProvider(elasticClientMock, new());

            var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "10", };
            var assert = () => sut.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            return assert.Should().ThrowExactlyAsync<ElasticsearchClientException>().WithMessage("no connection to db");
        }
    }
}