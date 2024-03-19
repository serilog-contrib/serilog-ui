using FluentAssertions;
using MsSql.Tests.DataProvider;
using RavenDb.Tests.Util;
using Serilog.Ui.RavenDbProvider;

namespace RavenDb.Tests.DataProvider;

[Collection(nameof(RavenDbDataProvider))]
[Trait("Integration-Search", "RavenDb")]
public class DataProviderSearchTest : IntegrationSearchTests<RavenDbTestProvider>
{
    public DataProviderSearchTest(RavenDbTestProvider instance) : base(instance)
    {
    }

    public override Task It_finds_all_data_with_default_search()
        => base.It_finds_all_data_with_default_search();

    [Fact(Skip = "Will be addressed later")]
    public override Task It_finds_data_with_all_filters()
        => base.It_finds_data_with_all_filters();

    public override Task It_finds_only_data_emitted_after_date()
        => base.It_finds_only_data_emitted_after_date();

    public override Task It_finds_only_data_emitted_before_date()
        => base.It_finds_only_data_emitted_before_date();

    public override Task It_finds_only_data_with_specific_level()
        => base.It_finds_only_data_with_specific_level();

    public override Task It_finds_only_data_with_specific_message_content()
        => base.It_finds_only_data_with_specific_message_content();

    public override Task It_finds_same_data_on_same_repeated_search()
        => base.It_finds_same_data_on_same_repeated_search();

    public override async Task It_finds_only_data_emitted_in_dates_range()
    {
        var firstTimeStamp = logCollector!.TimesSamples.First().AddSeconds(-50);
        var lastTimeStamp = logCollector.TimesSamples.Last();
        var inTimeStampCount = logCollector!.DataSet
            .Count(p => p.Timestamp >= firstTimeStamp && p.Timestamp <= lastTimeStamp);
        var (logs, count) = await provider.FetchDataAsync(1, 1000, startDate: firstTimeStamp, endDate: lastTimeStamp);

        logs.Should().NotBeEmpty();
        logs.Should().HaveCount(inTimeStampCount);
        count.Should().Be(inTimeStampCount);
        logs.Should().OnlyContain(p => p.Timestamp >= firstTimeStamp && p.Timestamp < lastTimeStamp);
    }
}