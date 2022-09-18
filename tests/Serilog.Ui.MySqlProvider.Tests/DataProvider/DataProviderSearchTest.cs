using Ardalis.GuardClauses;
using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;
using Serilog.Ui.MySqlProvider.Tests.Util;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.MySqlProvider.Tests.DataProvider
{
    public class DataProviderSearchTest : IClassFixture<MySqlTestProvider>, IIntegrationSearchTests
    {
        private readonly MySqlTestProvider instance;
        private readonly IDataProvider provider;

        public DataProviderSearchTest(MySqlTestProvider instance)
        {
            this.instance = instance;
            provider = Guard.Against.Null(instance.Provider!);
        }

        [Fact]
        public async Task It_finds_all_data_with_default_search()
        {
            var res = await provider.FetchDataAsync(1, 10);

            res.Item1.Should().HaveCount(10);
            res.Item2.Should().Be(100);
        }

        [Fact]
        public async Task It_finds_data_with_all_filters()
        {
            var (Logs, Count) = await provider.FetchDataAsync(1,
                10,
                instance.Collector!.Example.Level,
                instance.Collector!.Example.Message[3..],
                instance.Collector!.Example.Timestamp.AddSeconds(-2),
                instance.Collector!.Example.Timestamp.AddSeconds(2));

            var log = Logs.First();
            log.Message.Should().Be(instance.Collector.Example.Message);
            log.Level.Should().Be(instance.Collector.Example.Level);
            log.Properties.Should().Be(instance.Collector.Example.Properties);
            log.Timestamp.Should().BeCloseTo(instance.Collector.Example.Timestamp, TimeSpan.FromMinutes(5));
            Count.Should().BeCloseTo(1, 2);
        }

        [Fact]
        public async Task It_finds_only_data_emitted_after_date()
        {
            var lastTimeStamp = instance.Collector!.TimesSamples
                .ElementAt(instance.Collector.TimesSamples.Count() - 1).AddSeconds(-50);
            var (Logs, Count) = await provider.FetchDataAsync(1, 10, startDate: lastTimeStamp);

            Logs.Should().NotBeEmpty();
            Logs.Should().OnlyContain(p => p.Timestamp > lastTimeStamp);
            Count.Should().BeLessThan(100).And.BeGreaterThan(2);
        }

        [Fact]
        public async Task It_finds_only_data_emitted_before_date()
        {
            var firstTimeStamp = instance.Collector!.TimesSamples
                .ElementAt(instance.Collector.TimesSamples.Count() - 1).AddSeconds(50);
            var (Logs, Count) = await provider.FetchDataAsync(1, 10, endDate: firstTimeStamp);

            Logs.Should().NotBeEmpty();
            Logs.Should().OnlyContain(p => p.Timestamp < firstTimeStamp);
            Count.Should().BeLessThan(100).And.BeGreaterThan(2);
        }

        [Fact]
        public async Task It_finds_only_data_emitted_in_dates_range()
        {
            var firstTimeStamp = instance.Collector!.TimesSamples.First().AddSeconds(50);
            var lastTimeStamp = instance.Collector.TimesSamples.Last().AddSeconds(50);
            var (Logs, Count) = await provider.FetchDataAsync(1, 10, startDate: firstTimeStamp, endDate: lastTimeStamp);

            Logs.Should().NotBeEmpty();
            Logs.Should().OnlyContain(p => p.Timestamp > firstTimeStamp && p.Timestamp < lastTimeStamp);
            Count.Should().BeLessThan(100).And.BeGreaterThanOrEqualTo(3);
        }

        [Fact]
        public async Task It_finds_only_data_with_specific_level()
        {
            var choosenLvl = instance.Collector!.CountByLevel.FirstOrDefault(p => p.Value > 0);

            var (Logs, Count) = await provider.FetchDataAsync(1, 10, level: choosenLvl.Key);

            Logs.Should().NotBeEmpty();
            Logs.Should().OnlyContain(p => p.Level == choosenLvl.Key);
            Count.Should().Be(choosenLvl.Value);
        }

        [Fact]
        public async Task It_finds_only_data_with_specific_message_content()
        {
            var msg = instance.Collector!.MessagePiecesSamples.ElementAt(1);

            var (Logs, Count) = await provider.FetchDataAsync(1, 10, searchCriteria: msg);

            Logs.Should().NotBeEmpty();
            Logs.Should().OnlyContain(p =>
                p.Message
                .Split(" ", StringSplitOptions.None)
                .Intersect(msg!.Split(" ", StringSplitOptions.None)).Any());
            Count.Should().BeLessThan(100).And.Be(1);
        }

        [Fact]
        public async Task It_finds_same_data_on_same_repeated_search()
        {
            var choosenLvl = instance.Collector!.CountByLevel.FirstOrDefault(p => p.Value > 0);

            var (Logs, Count) = await provider.FetchDataAsync(3, 3, level: choosenLvl.Key);

            var (Logs2nd, Count2nd) = await provider.FetchDataAsync(3, 3, level: choosenLvl.Key);

            Logs.Should().BeEquivalentTo(Logs2nd);
            Count.Should().Be(Count2nd);
        }
    }
}
