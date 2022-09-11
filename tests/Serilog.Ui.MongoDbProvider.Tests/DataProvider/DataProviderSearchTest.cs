using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.MongoDbProvider.Tests.Util;
using Serilog.Ui.MongoDbProvider.Tests.Util.Builders;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.MongoDbProvider.Tests.DataProvider
{
    public class DataProviderSearchTest : BaseIntegrationTest<MongoDbDataProviderBuilder>, IAsyncLifetime, IIntegrationSearchTests
    {
        public Task DisposeAsync() => Task.CompletedTask;

        public async Task InitializeAsync()
        {
            _builder = await MongoDbDataProviderBuilder.Build(false);
        }

        [Fact]
        public async Task It_finds_all_data_with_default_search()
        {
            var (Logs, Count) = await _builder._sut.FetchDataAsync(1, 10);

            Logs.Should().NotBeEmpty().And.HaveCount(10);
            Count.Should().Be(20);
        }
    }
}
