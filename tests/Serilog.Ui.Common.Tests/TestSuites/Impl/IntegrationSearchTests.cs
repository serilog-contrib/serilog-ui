using Ardalis.GuardClauses;
using FluentAssertions;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MsSql.Tests.DataProvider
{
    public abstract class IntegrationSearchTests<DbRunner> : IIntegrationSearchTests
        where DbRunner : class, IIntegrationRunner
    {
        protected readonly LogModelPropsCollector logCollector;
        protected readonly IDataProvider provider;

        protected IntegrationSearchTests(DbRunner instance)
        {
            logCollector = instance.GetPropsCollector();
            provider = Guard.Against.Null(instance.GetDataProvider());
        }

        [Fact]
        public virtual async Task It_finds_all_data_with_default_search()
        {
            var res = await provider.FetchDataAsync(1, 10);

            res.Item1.Should().HaveCount(10);
            res.Item2.Should().Be(logCollector.DataSet.Count);
        }

        [Fact]
        public virtual Task It_finds_data_with_all_filters()
            => It_finds_data_with_all_filters_by_utc(false, false);

        protected virtual async Task It_finds_data_with_all_filters_by_utc(bool checkWithUtc, bool excludeProps)
        {
            var (Logs, Count) = await provider.FetchDataAsync(1,
                10,
                logCollector!.Example.Level,
                logCollector!.Example.Message[3..],
                logCollector!.Example.Timestamp.AddSeconds(-2),
                logCollector!.Example.Timestamp.AddSeconds(2));

            var log = Logs.First();
            log.Message.Should().Be(logCollector.Example.Message);
            log.Level.Should().Be(logCollector.Example.Level);
            if (!excludeProps) log.Properties.Should().Be(logCollector.Example.Properties);
            ConvertToUtc(log.Timestamp, checkWithUtc).Should().BeCloseTo(logCollector.Example.Timestamp, TimeSpan.FromMinutes(5));
            Count.Should().BeCloseTo(1, 2);
        }

        [Fact]
        public virtual Task It_finds_only_data_emitted_after_date()
            => It_finds_only_data_emitted_after_date_by_utc(false);

        protected async Task It_finds_only_data_emitted_after_date_by_utc(bool checkWithUtc)
        {
            var lastTimeStamp = logCollector!.TimesSamples
                .ElementAt(logCollector.TimesSamples.Count() - 1).AddHours(-4);
            var afterTimeStampCount = logCollector!.DataSet.Count(p => p.Timestamp > lastTimeStamp);
            var (Logs, Count) = await provider.FetchDataAsync(1, 1000, startDate: lastTimeStamp);

            Logs.Should().NotBeEmpty();
            Logs.Should().HaveCount(afterTimeStampCount);
            Count.Should().Be(afterTimeStampCount);
            Logs.Should().OnlyContain(p => ConvertToUtc(p.Timestamp, checkWithUtc) > lastTimeStamp);
        }

        [Fact]
        public virtual Task It_finds_only_data_emitted_before_date()
            => It_finds_only_data_emitted_before_date_by_utc(false);

        protected async Task It_finds_only_data_emitted_before_date_by_utc(bool checkWithUtc)
        {
            var firstTimeStamp = logCollector!.TimesSamples
                .ElementAt(logCollector.TimesSamples.Count() - 1).AddSeconds(50);
            var beforeTimeStampCount = logCollector!.DataSet.Count(p => p.Timestamp < firstTimeStamp);
            var (Logs, Count) = await provider.FetchDataAsync(1, 1000, endDate: firstTimeStamp);

            Logs.Should().NotBeEmpty();
            Logs.Should().HaveCount(beforeTimeStampCount);
            Count.Should().Be(beforeTimeStampCount);
            Logs.Should().OnlyContain(p => ConvertToUtc(p.Timestamp, checkWithUtc) < firstTimeStamp);
        }

        [Fact]
        public virtual Task It_finds_only_data_emitted_in_dates_range()
            => It_finds_only_data_emitted_in_dates_range_by_utc(false);

        protected async Task It_finds_only_data_emitted_in_dates_range_by_utc(bool checkWithUtc)
        {
            var firstTimeStamp = logCollector!.TimesSamples.First().AddSeconds(50);
            var lastTimeStamp = logCollector.TimesSamples.Last().AddSeconds(50);
            var inTimeStampCount = logCollector!.DataSet
                .Count(p => p.Timestamp >= firstTimeStamp && p.Timestamp < lastTimeStamp);
            var (Logs, Count) = await provider.FetchDataAsync(1, 1000, startDate: firstTimeStamp, endDate: lastTimeStamp);

            Logs.Should().NotBeEmpty();
            Logs.Should().HaveCount(inTimeStampCount);
            Count.Should().Be(inTimeStampCount);
            Logs.Should().OnlyContain(p =>
            ConvertToUtc(p.Timestamp, checkWithUtc) >= firstTimeStamp &&
            ConvertToUtc(p.Timestamp, checkWithUtc) < lastTimeStamp);
        }

        [Fact]
        public virtual async Task It_finds_only_data_with_specific_level()
        {
            var choosenLvl = logCollector!.CountByLevel.FirstOrDefault(p => p.Value > 0);

            var (Logs, Count) = await provider.FetchDataAsync(1, 10, level: choosenLvl.Key);

            Logs.Should().NotBeEmpty();
            Logs.Should().OnlyContain(p => p.Level == choosenLvl.Key);
            Count.Should().Be(choosenLvl.Value);
        }

        [Fact]
        public virtual async Task It_finds_only_data_with_specific_message_content()
        {
            var msg = logCollector!.MessagePiecesSamples.FirstOrDefault();

            var (Logs, Count) = await provider.FetchDataAsync(1, 10, searchCriteria: msg);

            Logs.Should().NotBeEmpty();
            Logs.Should().OnlyContain(p =>
                p.Message
                .Split(" ", StringSplitOptions.None)
                .Intersect(msg!.Split(" ", StringSplitOptions.None)).Any());
            Count.Should().BeLessThan(100).And.BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public virtual async Task It_finds_same_data_on_same_repeated_search()
        {
            var choosenLvl = logCollector!.CountByLevel.FirstOrDefault(p => p.Value > 0);

            var (Logs, Count) = await provider.FetchDataAsync(3, 3, level: choosenLvl.Key);

            var (Logs2nd, Count2nd) = await provider.FetchDataAsync(3, 3, level: choosenLvl.Key);

            Logs.Should().BeEquivalentTo(Logs2nd);
            Count.Should().Be(Count2nd);
        }

        private static DateTime ConvertToUtc(DateTime timestamp, bool checkWithUtc)
            => checkWithUtc ? timestamp.ToUniversalTime() : timestamp;
    }
}