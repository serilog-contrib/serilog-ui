using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using ElasticSearch.Tests.Util;
using System.Threading.Tasks;
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

        public override Task It_finds_data_with_all_filters()
            => It_finds_data_with_all_filters_by_utc(true, true);

        public override Task It_finds_only_data_emitted_after_date()
            => It_finds_only_data_emitted_after_date_by_utc(true);

        public override Task It_finds_only_data_emitted_before_date()
            => It_finds_only_data_emitted_before_date_by_utc(true);

        public override Task It_finds_only_data_emitted_in_dates_range()
            => It_finds_only_data_emitted_in_dates_range_by_utc(true);
    }
}