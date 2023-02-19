using Ardalis.GuardClauses;
using FluentAssertions;
using Npgsql;
using Postgres.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Postgres.Tests.DataProvider
{
    public class DataProviderPaginationTest : IClassFixture<PostgresTestProvider>, IIntegrationPaginationTests
    {
        private readonly PostgresTestProvider instance;
        private readonly IDataProvider provider;

        public DataProviderPaginationTest(PostgresTestProvider instance)
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

        [Fact]
        public Task It_throws_when_skip_is_zero()
        {
            var test = () => provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<PostgresException>();
        }
    }
}
