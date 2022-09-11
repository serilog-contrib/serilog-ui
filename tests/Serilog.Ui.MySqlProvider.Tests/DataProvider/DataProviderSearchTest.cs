using Ardalis.GuardClauses;
using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;
using Serilog.Ui.MySqlProvider.Tests.Util;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Ui.MySqlProvider.Tests.DataProvider
{
    public class DataProviderSearchTest : IClassFixture<MySqlTestProvider>, IIntegrationSearchTests
    {
        private readonly MySqlTestProvider instance;
        private readonly IDataProvider provider;

        public DataProviderSearchTest(MySqlTestProvider instance)
        {
            this.instance = instance;
            provider = Guard.Against.Null(instance.Provider!);
        }

        [Fact]
        public async Task It_finds_all_data_with_default_search()
        {
            var res = await provider.FetchDataAsync(1, 10);

            res.Item1.Should().HaveCount(10);
            res.Item2.Should().Be(20);
        }
    }
}
