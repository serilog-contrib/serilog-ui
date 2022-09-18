using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.MongoDbProvider.Tests.Util;
using Serilog.Ui.MongoDbProvider.Tests.Util.Builders;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.MongoDbProvider.Tests.DataProvider
{
    public class DataProviderPaginationTest : BaseIntegrationTest<MongoDbDataProviderBuilder>, IAsyncLifetime, IIntegrationPaginationTests
    {
        public Task DisposeAsync() => Task.CompletedTask;

        public async Task InitializeAsync()
        {
            _builder = await MongoDbDataProviderBuilder.Build(false);
        }

        [Fact]
        public async Task It_fetches_with_limit()
        {
            var (Logs, _) = await _builder._sut.FetchDataAsync(1, 5);

            Logs.Should().NotBeEmpty().And.HaveCount(5);
        }

        [Fact]
        public async Task It_fetches_with_limit_and_skip()
        {
            var example = _builder._collector!.Example;
            var (Logs, _) = await _builder._sut.FetchDataAsync(2, 1, level: example.Level);

            Logs.Should().NotBeEmpty().And.HaveCount(1);
            Logs.First().Level.Should().Be(example.Level);
            Logs.First().Message.Should().NotBe(example.Message);
        }

        [Fact]
        public async Task It_fetches_with_skip()
        {
            var example = _builder._collector!.Example;
            var (Logs, _) = await _builder._sut.FetchDataAsync(2, 1, level: example.Level);

            Logs.First().Level.Should().Be(example.Level);
            Logs.First().Message.Should().NotBe(example.Message);
        }

        [Fact]
        public Task It_throws_when_skip_is_zero()
        {
            var test = () => _builder._sut.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
    }
}
