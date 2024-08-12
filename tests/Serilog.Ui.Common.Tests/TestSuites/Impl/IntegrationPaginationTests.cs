using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Xunit;

namespace Serilog.Ui.Common.Tests.TestSuites.Impl
{
    public abstract class IntegrationPaginationTests<TDbRunner>(TDbRunner instance) : IIntegrationPaginationTests
        where TDbRunner : class, IIntegrationRunner
    {
        private readonly LogModelPropsCollector _logCollector = instance.GetPropsCollector();

        protected readonly IDataProvider Provider = Guard.Against.Null(instance.GetDataProvider());

        [Fact]
        public virtual async Task It_fetches_with_limit()
        {
            var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "5" };
            var (logs, _) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            logs.Should().NotBeEmpty().And.HaveCount(5);
        }

        [Fact]
        public virtual async Task It_fetches_with_limit_and_skip()
        {
            var example = _logCollector.Example;
            var query = new Dictionary<string, StringValues> { ["page"] = "2", ["count"] = "1", ["level"] = example.Level };
            var (logs, _) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            var results = logs.ToList();
            results.Should().NotBeEmpty().And.HaveCount(1);
            results.First().Level.Should().Be(example.Level);
        }

        [Fact]
        public virtual async Task It_fetches_with_skip()
        {
            var example = _logCollector.Example;
            var query = new Dictionary<string, StringValues> { ["page"] = "2", ["count"] = "1", ["level"] = example.Level };
            var (logs, _) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            var results = logs.ToList();
            results.Should().NotBeEmpty();
            results.First().Level.Should().Be(example.Level);
        }

        [Fact]
        public virtual async Task It_fetches_with_sort_by_timestamp()
        {
            // default sorting!
            var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "50" };
            var (descLogs, _) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            var desc = descLogs.ToList();
            desc.ForEach(p => { p.Timestamp = p.Timestamp.AddNanoseconds(-p.Timestamp.Nanosecond()).AddMicroseconds(-p.Timestamp.Microsecond()); });

            desc.Should().NotBeEmpty().And.BeInDescendingOrder(model => model.Timestamp);

            query["sortOn"] = $"{SearchOptions.SortProperty.Timestamp}";
            query["sortBy"] = $"{SearchOptions.SortDirection.Asc}";
            var (ascLogs, _) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            var asc = ascLogs.ToList();

            asc.ForEach(p => { p.Timestamp = p.Timestamp.AddNanoseconds(-p.Timestamp.Nanosecond()).AddMicroseconds(-p.Timestamp.Microsecond()); });

            asc.Should().NotBeEmpty().And.BeInAscendingOrder(model => model.Timestamp);
        }

        [Fact]
        public virtual async Task It_fetches_with_sort_by_message()
        {
            var query = new Dictionary<string, StringValues>
            {
                ["page"] = "1",
                ["count"] = "50",
                ["sortOn"] = $"{SearchOptions.SortProperty.Message}",
                ["sortBy"] = $"{SearchOptions.SortDirection.Desc}"
            };

            var (descLogs, _) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            descLogs.Should().NotBeEmpty().And.BeInDescendingOrder(model => model.Message, StringComparer.OrdinalIgnoreCase);

            query["sortOn"] = $"{SearchOptions.SortProperty.Message}";
            query["sortBy"] = $"{SearchOptions.SortDirection.Asc}";
            var (ascLogs, _) =
                await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            ascLogs.Should().NotBeEmpty().And.BeInAscendingOrder(model => model.Message, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public virtual async Task It_fetches_with_sort_by_level()
        {
            var query = new Dictionary<string, StringValues>
            {
                ["page"] = "1",
                ["count"] = "50",
                ["sortOn"] = $"{SearchOptions.SortProperty.Level}",
                ["sortBy"] = $"{SearchOptions.SortDirection.Desc}"
            };

            var (descLogs, _) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            descLogs.Should().NotBeEmpty().And.BeInDescendingOrder(model => model.Level, StringComparer.OrdinalIgnoreCase);

            query["sortOn"] = $"{SearchOptions.SortProperty.Level}";
            query["sortBy"] = $"{SearchOptions.SortDirection.Asc}";
            var (ascLogs, _) = await Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            ascLogs.Should().NotBeEmpty().And.BeInAscendingOrder(model => model.Level, StringComparer.OrdinalIgnoreCase);
        }

        public abstract Task It_throws_when_skip_is_zero();
    }
}