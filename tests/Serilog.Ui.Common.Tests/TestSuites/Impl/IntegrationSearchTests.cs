using Ardalis.GuardClauses;
using FluentAssertions;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.Common.Tests.TestSuites.Impl
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
            if (!excludeProps)
            {
                log.Properties.Should().Be(logCollector.Example.Properties);
            }

            ConvertToUtc(log.Timestamp, checkWithUtc).Should().BeCloseTo(logCollector.Example.Timestamp, TimeSpan.FromMinutes(5));
            Count.Should().BeCloseTo(1, 2);
        }

        [Fact]
        public virtual Task It_finds_only_data_emitted_after_date()
            => It_finds_only_data_emitted_after_date_by_utc(false);

        protected async Task It_finds_only_data_emitted_after_date_by_utc(bool checkWithUtc)
        {
            // ARRANGE
            var lastTimeStamp = logCollector!.TimesSamples.ElementAt(logCollector.TimesSamples.Count() - 1).AddHours(-4);
            var afterTimeStampCount = logCollector!.DataSet.Count(p => p.Timestamp > lastTimeStamp);

            // ACT
            var (logs, count) = await provider.FetchDataAsync(1, 1000, startDate: lastTimeStamp);
            var enumerateLogs = logs.ToList();

            // ASSERT
            enumerateLogs.Should().NotBeEmpty();

            enumerateLogs.Should().HaveCount(afterTimeStampCount);
            count.Should().Be(afterTimeStampCount);

            var convertedLogs = enumerateLogs.Select(log => ConvertToUtc(log.Timestamp, checkWithUtc)).ToList();
            convertedLogs.Should().AllSatisfy(p => { p.Kind.Should().Be(DateTimeKind.Utc); });
            convertedLogs.Should().AllSatisfy(p => { p.Should().BeAfter(lastTimeStamp); });
        }

        [Fact]
        public virtual Task It_finds_only_data_emitted_before_date()
            => It_finds_only_data_emitted_before_date_by_utc(false);

        protected async Task It_finds_only_data_emitted_before_date_by_utc(bool checkWithUtc)
        {
            var firstTimeStamp = logCollector!.TimesSamples
                .ElementAt(logCollector.TimesSamples.Count() - 1).AddSeconds(50);
            var beforeTimeStampCount = logCollector!.DataSet.Count(p => p.Timestamp < firstTimeStamp);
            var (logs, count) = await provider.FetchDataAsync(1, 1000, endDate: firstTimeStamp);

            var enumerateLogs = logs.ToList();
            enumerateLogs.Should().NotBeEmpty();
            enumerateLogs.Should().HaveCount(beforeTimeStampCount);
            count.Should().Be(beforeTimeStampCount);
            enumerateLogs.Select(log => ConvertToUtc(log.Timestamp, checkWithUtc)).Should().AllSatisfy(p => { p.Should().BeBefore(firstTimeStamp); });
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
            var (logs, count) = await provider.FetchDataAsync(1, 1000, startDate: firstTimeStamp, endDate: lastTimeStamp);

            var enumerateLogs = logs.ToList();
            enumerateLogs.Should().NotBeEmpty();
            enumerateLogs.Should().HaveCount(inTimeStampCount);
            count.Should().Be(inTimeStampCount);
            enumerateLogs.Should().OnlyContain(p =>
                ConvertToUtc(p.Timestamp, checkWithUtc) >= firstTimeStamp &&
                ConvertToUtc(p.Timestamp, checkWithUtc) < lastTimeStamp);
        }

        [Fact]
        public virtual async Task It_finds_only_data_with_specific_level()
        {
            var chosenLvl = logCollector!.CountByLevel.FirstOrDefault(p => p.Value > 0);

            var (logs, count) = await provider.FetchDataAsync(1, 10, level: chosenLvl.Key);

            logs.Should().NotBeEmpty();
            logs.Should().OnlyContain(p => p.Level == chosenLvl.Key);
            count.Should().Be(chosenLvl.Value);
        }

        [Fact]
        public virtual async Task It_finds_only_data_with_specific_message_content()
        {
            var msg = logCollector!.MessagePiecesSamples.FirstOrDefault();

            var (logs, count) = await provider.FetchDataAsync(1, 10, searchCriteria: msg);

            logs.Should().NotBeEmpty();
            logs.Should().OnlyContain(p =>
                p.Message
                    .Split(" ", StringSplitOptions.None)
                    .Intersect(msg!.Split(" ", StringSplitOptions.None)).Any());
            count.Should().BeLessThan(100).And.BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public virtual async Task It_finds_same_data_on_same_repeated_search()
        {
            var choosenLvl = logCollector!.CountByLevel.FirstOrDefault(p => p.Value > 0);

            var (logs, count) = await provider.FetchDataAsync(3, 3, level: choosenLvl.Key);

            var (logs2nd, count2nd) = await provider.FetchDataAsync(3, 3, level: choosenLvl.Key);

            logs.Should().BeEquivalentTo(logs2nd);
            count.Should().Be(count2nd);
        }

        private static DateTime ConvertToUtc(DateTime timestamp, bool checkWithUtc)
            => checkWithUtc ? timestamp.ToUniversalTime() : timestamp;
    }
}