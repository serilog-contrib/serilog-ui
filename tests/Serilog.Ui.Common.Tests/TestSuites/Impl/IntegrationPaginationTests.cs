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
        public virtual async Task It_fetches_with_timestamp_sort()
        {
            // default sorting!
            var (descLogs, _) = await Provider.FetchDataAsync(1, 20);

            descLogs.Should().BeInDescendingOrder(model => model.Timestamp);

            var (ascLogs, _) = await Provider.FetchDataAsync(1, 20, sortOn: SearchOptions.SortProperty.Timestamp, sortBy: SearchOptions.SortDirection.Asc);

            ascLogs.Should().BeInAscendingOrder(model => model.Timestamp);
        }
        
        [Fact]
        public virtual async Task It_fetches_with_message_sort()
        {
            var (descLogs, _) = await Provider.FetchDataAsync(1, 20, sortOn: SearchOptions.SortProperty.Message, sortBy: SearchOptions.SortDirection.Desc);

            descLogs.Should().BeInDescendingOrder(model => model.Message);

            var (ascLogs, _) = await Provider.FetchDataAsync(1, 20, sortOn: SearchOptions.SortProperty.Message, sortBy: SearchOptions.SortDirection.Asc);

            ascLogs.Should().BeInAscendingOrder(model => model.Message);
        }

        [Fact]
        public virtual async Task It_fetches_with_level_sort()
        {
            var (descLogs, _) = await Provider.FetchDataAsync(1, 20, sortOn: SearchOptions.SortProperty.Level, sortBy: SearchOptions.SortDirection.Desc);

            descLogs.Should().BeInDescendingOrder(model => model.Level);

            var (ascLogs, _) = await Provider.FetchDataAsync(1, 20, sortOn: SearchOptions.SortProperty.Level, sortBy: SearchOptions.SortDirection.Asc);

            ascLogs.Should().BeInAscendingOrder(model => model.Level);
        }
        
        public abstract Task It_throws_when_skip_is_zero();
    }
}
