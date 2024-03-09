using System.Linq;
using MongoDb.Tests.Util;
using Serilog.Ui.MongoDbProvider;
using System.Threading.Tasks;
using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Xunit;

namespace MongoDb.Tests.DataProvider
{
    [Collection(nameof(MongoDbDataProvider))]
    [Trait("Integration-Search", "MongoDb")]
    public class DataProviderSearchTest : IntegrationSearchTests<BaseIntegrationTest>
    {
        public DataProviderSearchTest(BaseIntegrationTest instance) : base(instance)
        {
        }
        
        public override async Task It_finds_only_data_emitted_in_dates_range()
        {
            var firstTimeStamp = LogCollector!.TimesSamples.First().AddSeconds(50);
            var lastTimeStamp = LogCollector.TimesSamples.Last().AddSeconds(50);
            var inTimeStampCount = LogCollector!.DataSet
                .Count(p => p.Timestamp >= firstTimeStamp && p.Timestamp < lastTimeStamp);
            var (logs, count) = await Provider.FetchDataAsync(1, 1000, startDate: firstTimeStamp, endDate: lastTimeStamp);

            var enumerateLogs = logs.ToList();
            enumerateLogs.Should().NotBeEmpty();
            enumerateLogs.Should().HaveCount(inTimeStampCount);
            count.Should().Be(inTimeStampCount);
            enumerateLogs.Should().OnlyContain(p =>
                p.Timestamp.ToUniversalTime() >= firstTimeStamp &&
                p.Timestamp.ToUniversalTime() < lastTimeStamp);
        }
    }
}