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
    public abstract class IntegrationSearchTests<TDbRunner>(TDbRunner instance) : IIntegrationSearchTests
        where TDbRunner : class, IIntegrationRunner
    {
        protected readonly LogModelPropsCollector LogCollector = instance.GetPropsCollector();

        protected readonly IDataProvider Provider = Guard.Against.Null(instance.GetDataProvider());

        [Fact]
        public virtual async Task It_finds_all_data_with_default_search()
        {
            var res = await Provider.FetchDataAsync(1, 10);

            res.Item1.Should().HaveCount(10);
            res.Item2.Should().Be(LogCollector.DataSet.Count);
        }

        [Fact]
        public virtual async Task It_finds_data_with_all_filters()
        {
            var (logs, count) = await Provider.FetchDataAsync(1,
                10,
                LogCollector.Example.Level,
                LogCollector.Example.Message[3..],
                LogCollector.Example.Timestamp.AddSeconds(-2),
                LogCollector.Example.Timestamp.AddSeconds(2));

            var log = logs.First();
            log.Message.Should().Be(LogCollector.Example.Message);
            log.Level.Should().Be(LogCollector.Example.Level);

            log.Timestamp.Should().BeCloseTo(LogCollector.Example.Timestamp, TimeSpan.FromMinutes(5));
            count.Should().BeCloseTo(1, 2);
        }

        [Fact]
        public virtual async Task It_finds_only_data_emitted_after_date()
        {
            // ARRANGE
            var lastTimeStamp = LogCollector.TimesSamples.ElementAt(LogCollector.TimesSamples.Count() - 1).AddSeconds(-50);
            var afterTimeStampCount = LogCollector.DataSet.Count(p => p.Timestamp > lastTimeStamp);

            // ACT
            var (logs, count) = await Provider.FetchDataAsync(1, 1000, startDate: lastTimeStamp);
            var enumerateLogs = logs.ToList();

            // ASSERT
            enumerateLogs.Should().NotBeEmpty();

            enumerateLogs.Should().HaveCount(afterTimeStampCount);
            count.Should().Be(afterTimeStampCount);

            var convertedLogs = enumerateLogs.Select(log => log.Timestamp).ToList();
            convertedLogs.Should().AllSatisfy(p =>
            {
                p.Kind.Should().Be(DateTimeKind.Utc);
                p.Should().BeAfter(lastTimeStamp);
            });
        }

        [Fact]
        public virtual async Task It_finds_only_data_emitted_before_date()
        {
            var firstTimeStamp = LogCollector.TimesSamples
                .ElementAt(LogCollector.TimesSamples.Count() - 1).AddSeconds(50);
            var beforeTimeStampCount = LogCollector.DataSet.Count(p => p.Timestamp < firstTimeStamp);
            var (logs, count) = await Provider.FetchDataAsync(1, 1000, endDate: firstTimeStamp);

            var enumerateLogs = logs.ToList();
            enumerateLogs.Should().NotBeEmpty();
            enumerateLogs.Should().HaveCount(beforeTimeStampCount);
            count.Should().Be(beforeTimeStampCount);
            enumerateLogs.Select(log => log.Timestamp).Should().AllSatisfy(p => { p.Should().BeBefore(firstTimeStamp); });
        }

        [Fact]
        public virtual async Task It_finds_only_data_emitted_in_dates_range()
        {
            var firstTimeStamp = LogCollector.TimesSamples.First().AddSeconds(-50);
            var lastTimeStamp = LogCollector.TimesSamples.Last();
            var inTimeStampCount = LogCollector.DataSet
                .Count(p => p.Timestamp >= firstTimeStamp && p.Timestamp <= lastTimeStamp);
            var (logs, count) = await Provider.FetchDataAsync(1, 10, startDate: firstTimeStamp, endDate: lastTimeStamp);

            var results = logs.ToList();
            results.Should().NotBeEmpty();
            results.Should().HaveCount(inTimeStampCount);
            count.Should().Be(inTimeStampCount);

            results.Select(log => log.Timestamp).Should().AllSatisfy(p =>
            {
                p.Kind.Should().Be(DateTimeKind.Utc); // all providers should return utc datetime 
                p.Should().BeOnOrAfter(firstTimeStamp);
                p.Should().BeBefore(lastTimeStamp);
            });
        }

        [Fact]
        public virtual async Task It_finds_only_data_with_specific_level()
        {
            var chosenLvl = LogCollector.CountByLevel.FirstOrDefault(p => p.Value > 0);

            var (logs, count) = await Provider.FetchDataAsync(1, 10, level: chosenLvl.Key);

            var results = logs.ToList();
            results.Should().NotBeEmpty();
            results.Should().OnlyContain(p => p.Level == chosenLvl.Key);
            count.Should().Be(chosenLvl.Value);
        }

        [Fact]
        public virtual async Task It_finds_only_data_with_specific_message_content()
        {
            var msg = LogCollector.MessagePiecesSamples.FirstOrDefault();

            var (logs, count) = await Provider.FetchDataAsync(1, 10, searchCriteria: msg);

            var results = logs.ToList();
            results.Should().NotBeEmpty();
            results.Should().OnlyContain(p =>
                p.Message
                    .Split(" ", StringSplitOptions.None)
                    .Intersect(msg!.Split(" ", StringSplitOptions.None)).Any());
            count.Should().BeLessThan(100).And.BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public virtual async Task It_finds_same_data_on_same_repeated_search()
        {
            var chosenLvl = LogCollector.CountByLevel.FirstOrDefault(p => p.Value > 0);

            var (logs, count) = await Provider.FetchDataAsync(3, 3, level: chosenLvl.Key);

            var (logs2Nd, count2Nd) = await Provider.FetchDataAsync(3, 3, level: chosenLvl.Key);

            logs.Should().BeEquivalentTo(logs2Nd);
            count.Should().Be(count2Nd);
        }
    }
}