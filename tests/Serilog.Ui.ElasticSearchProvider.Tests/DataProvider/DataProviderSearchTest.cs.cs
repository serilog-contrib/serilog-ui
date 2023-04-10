using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using ElasticSearch.Tests.Util;
using FluentAssertions;
using MsSql.Tests.DataProvider;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ElasticSearch.Tests.DataProvider
{
    [Trait("Integration-Search", "Elastic")]
    public class DataProviderSearchTest : IntegrationSearchTests<ElasticTestProvider>,
        IClassFixture<ElasticTestProvider>,
        IClusterFixture<Elasticsearch7XCluster>
    {
        public DataProviderSearchTest(ElasticTestProvider instance) : base(instance) { }

        public override Task It_finds_all_data_with_default_search()
            => base.It_finds_all_data_with_default_search();

        public override Task It_finds_data_with_all_filters()
            => It_finds_data_with_all_filters_by_utc(true, true);

        public override Task It_finds_only_data_emitted_after_date()
            => It_finds_only_data_emitted_after_date_by_utc(true);

        public override Task It_finds_only_data_emitted_before_date()
            => It_finds_only_data_emitted_before_date_by_utc(true);

        [I]
        public override async Task It_finds_only_data_emitted_in_dates_range()
        {
            var firstTimeStamp = logCollector!.TimesSamples.First().AddSeconds(-50);
            var lastTimeStamp = logCollector.TimesSamples.Last();
            var inTimeStampCount = logCollector!.DataSet
                .Count(p => p.Timestamp >= firstTimeStamp && p.Timestamp <= lastTimeStamp);
            var (Logs, Count) = await provider.FetchDataAsync(1, 1000, startDate: firstTimeStamp, endDate: lastTimeStamp);

            Logs.Should().NotBeEmpty();
            Logs.Should().HaveCount(inTimeStampCount);
            Count.Should().Be(inTimeStampCount);
            Logs.Should().OnlyContain(p =>
            p.Timestamp.ToUniversalTime() >= firstTimeStamp &&
            p.Timestamp.ToUniversalTime() < lastTimeStamp);
        }
        //=> It_finds_only_data_emitted_in_dates_range_by_utc(true);

        public override Task It_finds_only_data_with_specific_level()
            => base.It_finds_only_data_with_specific_level();

        public override Task It_finds_only_data_with_specific_message_content()
            => base.It_finds_only_data_with_specific_message_content();

        public override Task It_finds_same_data_on_same_repeated_search()
            => base.It_finds_same_data_on_same_repeated_search();
    }
}
