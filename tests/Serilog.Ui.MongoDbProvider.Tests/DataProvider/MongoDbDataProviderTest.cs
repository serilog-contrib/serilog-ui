using FluentAssertions;
using Serilog.Ui.MongoDbProvider.Tests.Util;
using Serilog.Ui.MongoDbProvider.Tests.Util.Builders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.MongoDbProvider.Tests.DataProvider
{
    public class MongoDbDataProviderTest : BaseIntegrationTest<MongoDbDataProviderBuilder>, IAsyncLifetime
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

        [Fact]
        public void It_throws_when_any_dependency_is_null()
        {
            var suts = new List<Func<MongoDbDataProvider>>
            {
                () => new MongoDbDataProvider(null, null),
                () => new MongoDbDataProvider(_builder._client, null),
                () => new MongoDbDataProvider(null, _builder._options),
            };

            suts.ForEach(sut => sut.Should().ThrowExactly<ArgumentNullException>());
        }
    }
}
