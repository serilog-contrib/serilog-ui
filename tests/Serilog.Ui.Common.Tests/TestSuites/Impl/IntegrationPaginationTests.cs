using System;
using Ardalis.GuardClauses;
using FluentAssertions;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.Common.Tests.TestSuites.Impl
{
    public abstract class IntegrationPaginationTests<DbRunner> : IIntegrationPaginationTests
        where DbRunner : class, IIntegrationRunner
    {
        private readonly LogModelPropsCollector _logCollector;

        protected readonly IDataProvider Provider;

        protected IntegrationPaginationTests(DbRunner instance)
        {
            _logCollector = instance.GetPropsCollector();
            Provider = Guard.Against.Null(instance.GetDataProvider());
        }

        [Fact]
        public virtual async Task It_fetches_with_limit()
        {
            var (logs, _) = await Provider.FetchDataAsync(1, 5);

            logs.Should().NotBeEmpty().And.HaveCount(5);
        }

        [Fact]
        public virtual async Task It_fetches_with_limit_and_skip()
        {
            var example = _logCollector.Example;
            var (logs, _) = await Provider.FetchDataAsync(2, 1, level: example.Level);

            logs.Should().NotBeEmpty().And.HaveCount(1);
            logs.First().Level.Should().Be(example.Level);
            logs.First().Message.Should().NotBe(example.Message);
        }

        [Fact]
        public virtual async Task It_fetches_with_skip()
        {
            var example = _logCollector.Example;
            var (logs, _) = await Provider.FetchDataAsync(2, 1, level: example.Level);

            logs.First().Level.Should().Be(example.Level);
            logs.First().Message.Should().NotBe(example.Message);
        }

        [Fact]
        public virtual async Task It_fetches_with_sort_by_timestamp()
        {
            // default sorting!
            var (descLogs, _) = await Provider.FetchDataAsync(1, 50);

            descLogs.Should().NotBeEmpty().And.BeInDescendingOrder(model => model.Timestamp);

            var (ascLogs, _) = await Provider.FetchDataAsync(1, 50, sortOn: SearchOptions.SortProperty.Timestamp, sortBy: SearchOptions.SortDirection.Asc);

            ascLogs.Should().NotBeEmpty().And.BeInAscendingOrder(model => model.Timestamp);
        }

        [Fact]
        public virtual async Task It_fetches_with_sort_by_message()
        {
            var (descLogs, _) = await Provider.FetchDataAsync(1, 50, sortOn: SearchOptions.SortProperty.Message, sortBy: SearchOptions.SortDirection.Desc);

            descLogs.Should().NotBeEmpty().And.BeInDescendingOrder(model => model.Message, StringComparer.OrdinalIgnoreCase);

            var (ascLogs, _) = await Provider.FetchDataAsync(1, 50, sortOn: SearchOptions.SortProperty.Message, sortBy: SearchOptions.SortDirection.Asc);

            ascLogs.Should().NotBeEmpty().And.BeInAscendingOrder(model => model.Message, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public virtual async Task It_fetches_with_sort_by_level()
        {
            var (descLogs, _) = await Provider.FetchDataAsync(1, 50, sortOn: SearchOptions.SortProperty.Level, sortBy: SearchOptions.SortDirection.Desc);

            descLogs.Should().NotBeEmpty().And.BeInDescendingOrder(model => model.Level, StringComparer.OrdinalIgnoreCase);

            var (ascLogs, _) = await Provider.FetchDataAsync(1, 50, sortOn: SearchOptions.SortProperty.Level, sortBy: SearchOptions.SortDirection.Asc);

            ascLogs.Should().NotBeEmpty().And.BeInAscendingOrder(model => model.Level, StringComparer.OrdinalIgnoreCase);
        }

        public abstract Task It_throws_when_skip_is_zero();
    }
}
