using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Attributes;
using Serilog.Ui.Core.Models;
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
            var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "10" };
            var res = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            res.Item1.Should().HaveCount(10);
            res.Item1.Should().AllSatisfy(x => x.Properties.Should().NotBeNullOrWhiteSpace());
            res.Item2.Should().Be(LogCollector.DataSet.Count);
        }

        [Fact]
        public virtual async Task It_finds_data_with_all_filters()
        {
            var query = new Dictionary<string, StringValues>
            {
                ["page"] = "1",
                ["count"] = "10",
                ["level"] = LogCollector.Example.Level,
                ["search"] = LogCollector.Example.Message[3..],
                ["startDate"] = LogCollector.Example.Timestamp.AddSeconds(-2).ToString("O"),
                ["endDate"] = LogCollector.Example.Timestamp.AddSeconds(2).ToString("O")
            };
            var (logs, count) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

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
            var query = new Dictionary<string, StringValues>
            {
                ["page"] = "1",
                ["count"] = "1000",
                ["startDate"] = lastTimeStamp.ToString("O")
            };

            // ACT
            var (logs, count) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
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
            // ARRANGE
            var firstTimeStamp = LogCollector.TimesSamples
                .ElementAt(LogCollector.TimesSamples.Count() - 1).AddSeconds(50);
            var beforeTimeStampCount = LogCollector.DataSet.Count(p => p.Timestamp < firstTimeStamp);
            var query = new Dictionary<string, StringValues>
            {
                ["page"] = "1",
                ["count"] = "1000",
                ["endDate"] = firstTimeStamp.ToString("O")
            };

            // ACT
            var (logs, count) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            // ASSERT
            var enumerateLogs = logs.ToList();
            enumerateLogs.Should().NotBeEmpty();
            enumerateLogs.Should().HaveCount(beforeTimeStampCount);
            count.Should().Be(beforeTimeStampCount);
            enumerateLogs.Select(log => log.Timestamp).Should().AllSatisfy(p => { p.Should().BeBefore(firstTimeStamp); });
        }

        [Fact]
        public virtual async Task It_finds_only_data_emitted_in_dates_range()
        {
            // ARRANGE
            var firstTimeStamp = LogCollector.TimesSamples.First().AddSeconds(-50);
            var lastTimeStamp = LogCollector.TimesSamples.Last();
            var inTimeStampCount = LogCollector.DataSet
                .Count(p => p.Timestamp >= firstTimeStamp && p.Timestamp <= lastTimeStamp);
            var query = new Dictionary<string, StringValues>
            {
                ["page"] = "1",
                ["count"] = "10",
                ["startDate"] = firstTimeStamp.ToString("O"),
                ["endDate"] = lastTimeStamp.ToString("O")
            };

            // ACT
            var (logs, count) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            // ASSERT
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
            // ARRANGE
            var chosenLvl = LogCollector.CountByLevel.FirstOrDefault(p => p.Key == "Error");
            var query = new Dictionary<string, StringValues>
            {
                ["page"] = "1",
                ["count"] = "10",
                ["level"] = chosenLvl.Key
            };

            // ACT
            var (logs, count) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            // ASSERT
            var results = logs.ToList();
            results.Should().NotBeEmpty();
            results.Should().OnlyContain(p => p.Level == chosenLvl.Key);
            count.Should().Be(chosenLvl.Value);

            // checking that an Exception is serialized
            var removedAttribute = results.First().GetType().GetProperty(nameof(LogModel.Exception))!.GetCustomAttribute<RemovedColumnAttribute>();
            results.Should().AllSatisfy(p =>
            {
                p.Exception.Should().Match(x =>
                    removedAttribute != null ||
                    (!string.IsNullOrWhiteSpace(x)));
            });
        }

        [Fact]
        public virtual async Task It_finds_only_data_with_specific_message_content()
        {
            // ARRANGE
            var msg = LogCollector.MessagePiecesSamples.FirstOrDefault();
            var query = new Dictionary<string, StringValues>
            {
                ["page"] = "1",
                ["count"] = "10",
                ["search"] = msg
            };

            // ACT
            var (logs, count) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            // ASSERT
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
            // ARRANGE
            var chosenLvl = LogCollector.CountByLevel.FirstOrDefault(p => p.Value > 0);
            var query = new Dictionary<string, StringValues>
            {
                ["page"] = "3",
                ["count"] = "3",
                ["level"] = chosenLvl.Key,
            };

            // ACT
            var (logs, count) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            var (logs2Nd, count2Nd) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            // ASSERT
            logs.Should().BeEquivalentTo(logs2Nd);
            count.Should().Be(count2Nd);
        }
    }
}