using Ardalis.GuardClauses;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.Core;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.Common.Tests.TestSuites.Impl
{
    public abstract class IntegrationPaginationTests<DatabaseInstance, T, T1> : IClassFixture<DatabaseInstance>, IIntegrationPaginationTests
        where DatabaseInstance : DatabaseInstance<T, T1> 
        where T : TestcontainerDatabase
        where T1 : TestcontainerDatabaseConfiguration, new()
    {
        protected readonly DatabaseInstance<T, T1> instance;
        protected readonly IDataProvider provider;

        protected IntegrationPaginationTests(DatabaseInstance<T, T1> instance)
        {
            this.instance = instance;
            provider = Guard.Against.Null(instance.Provider!);
        }

        [Fact]
        public async Task It_fetches_with_limit()
        {
            var (Logs, _) = await provider.FetchDataAsync(1, 5);

            Logs.Should().NotBeEmpty().And.HaveCount(5);
        }

        [Fact]
        public async Task It_fetches_with_limit_and_skip()
        {
            var example = instance.Collector!.Example;
            var (Logs, _) = await provider.FetchDataAsync(2, 1, level: example.Level);

            Logs.Should().NotBeEmpty().And.HaveCount(1);
            Logs.First().Level.Should().Be(example.Level);
            Logs.First().Message.Should().NotBe(example.Message);
        }

        [Fact]
        public async Task It_fetches_with_skip()
        {
            var example = instance.Collector!.Example;
            var (Logs, _) = await provider.FetchDataAsync(2, 1, level: example.Level);

            Logs.First().Level.Should().Be(example.Level);
            Logs.First().Message.Should().NotBe(example.Message);
        }

        public abstract Task It_throws_when_skip_is_zero();
    }
}
