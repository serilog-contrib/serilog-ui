using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.MongoDbProvider.Tests.Util;
using Serilog.Ui.MongoDbProvider.Tests.Util.Builders;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.MongoDbProvider.Tests.DataProvider
{
    public class DataProviderSearchTest : BaseIntegrationTest<MongoDbDataProviderBuilder>, IAsyncLifetime, IIntegrationSearchTests
    {
        public Task DisposeAsync() => Task.CompletedTask;

        public async Task InitializeAsync()
        {
            _builder = await MongoDbDataProviderBuilder.Build(false);
        }

        [Fact]
        public async Task It_finds_all_data_with_default_search()
        {
            var (Logs, Count) = await _builder._sut.FetchDataAsync(1, 10);

            Logs.Should().NotBeEmpty().And.HaveCount(10);
            Count.Should().Be(100);
        }

        [Fact]
        public async Task It_finds_data_with_all_filters()
        {
            var (Logs, Count) = await _builder._sut.FetchDataAsync(1,
                10,
                _builder._collector!.Example.Level,
                _builder._collector.Example.Message[3..],
                _builder._collector.Example.Timestamp.AddSeconds(-2),
                _builder._collector.Example.Timestamp.AddSeconds(2));

            var log = Logs.First();
            log.Message.Should().Be(_builder._collector.Example.Message);
            log.Level.Should().Be(_builder._collector.Example.Level);
            log.Properties.Should().Be(_builder._collector.Example.Properties);
            log.Timestamp.ToUniversalTime().Should().BeCloseTo(_builder._collector.Example.Timestamp, TimeSpan.FromMinutes(5));
            Count.Should().BeCloseTo(1, 2);
        }

        [Fact]
        public async Task It_finds_only_data_emitted_after_date()
        {
            var lastTimeStamp = _builder._collector?.TimesSamples
                .ElementAt(_builder._collector!.TimesSamples.Count() - 1).AddSeconds(-50);
            var afterTimeStampCount = _builder._collector!.DataSet.Count(p => p.Timestamp > lastTimeStamp);
            var (Logs, Count) = await _builder._sut.FetchDataAsync(1, 1000, startDate: lastTimeStamp);

            Logs.Should().NotBeEmpty();
            Logs.Should().HaveCount(afterTimeStampCount);
            Count.Should().Be(afterTimeStampCount);
            Logs.Should().OnlyContain(p => p.Timestamp.ToUniversalTime() > lastTimeStamp);
        }

        [Fact]
        public async Task It_finds_only_data_emitted_before_date()
        {
            var firstTimeStamp = _builder._collector?.TimesSamples
                .ElementAt(_builder._collector!.TimesSamples.Count() - 1).AddSeconds(50);
            var beforeTimeStampCount = _builder._collector!.DataSet.Count(p => p.Timestamp < firstTimeStamp);
            var (Logs, Count) = await _builder._sut.FetchDataAsync(1, 1000, endDate: firstTimeStamp);

            Logs.Should().NotBeEmpty();
            Logs.Should().HaveCount(beforeTimeStampCount);
            Count.Should().Be(beforeTimeStampCount);
            Logs.Should().OnlyContain(p => p.Timestamp.ToUniversalTime() < firstTimeStamp);
        }

        [Fact]
        public async Task It_finds_only_data_emitted_in_dates_range()
        {
            var firstTimeStamp = _builder._collector?.TimesSamples.First().AddSeconds(50);
            var lastTimeStamp = _builder._collector?.TimesSamples.Last().AddSeconds(50);
            var inTimeStampCount = _builder._collector!.DataSet
                .Count(p => p.Timestamp >= firstTimeStamp && p.Timestamp < lastTimeStamp); 
            var (Logs, Count) = await _builder._sut.FetchDataAsync(1, 1000, startDate: firstTimeStamp, endDate: lastTimeStamp);

            Logs.Should().NotBeEmpty();
            Logs.Should().HaveCount(inTimeStampCount);
            Count.Should().Be(inTimeStampCount);
            Logs.Should().OnlyContain(p => 
                p.Timestamp.ToUniversalTime() > firstTimeStamp && p.Timestamp.ToUniversalTime() < lastTimeStamp);
            Count.Should().BeLessThan(100).And.BeGreaterThanOrEqualTo(3);
        }

        [Fact]
        public async Task It_finds_only_data_with_specific_level()
        {
            var choosenLvl = _builder._collector!.CountByLevel.FirstOrDefault(p => p.Value > 0);

            var (Logs, Count) = await _builder._sut.FetchDataAsync(1, 10, level: choosenLvl.Key);

            Logs.Should().NotBeEmpty();
            Logs.Should().OnlyContain(p => p.Level == choosenLvl.Key);
            Count.Should().Be(choosenLvl.Value);
        }

        [Fact]
        public async Task It_finds_only_data_with_specific_message_content()
        {
            var msg = _builder._collector!.MessagePiecesSamples.FirstOrDefault();

            var (Logs, Count) = await _builder._sut.FetchDataAsync(1, 10, searchCriteria: msg);

            Logs.Should().NotBeEmpty();
            Logs.Should().OnlyContain(p =>
                p.Message
                .Split(" ", System.StringSplitOptions.None)
                .Intersect(msg!.Split(" ", System.StringSplitOptions.None)).Any());
            Count.Should().BeLessThan(100).And.BeGreaterThan(1);
        }

        [Fact]
        public async Task It_finds_same_data_on_same_repeated_search()
        {
            var choosenLvl = _builder._collector!.CountByLevel.FirstOrDefault(p => p.Value > 0);

            var (Logs, Count) = await _builder._sut.FetchDataAsync(3, 3, level: choosenLvl.Key);

            var (Logs2nd, Count2nd) = await _builder._sut.FetchDataAsync(3, 3, level: choosenLvl.Key);

            Logs.Should().BeEquivalentTo(Logs2nd);
            Count.Should().Be(Count2nd);
        }
    }
}
