using Ardalis.GuardClauses;
using ElasticSearch.Tests.Util;
using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;
using System.Threading.Tasks;
using Xunit;

namespace ElasticSearch.Tests.DataProvider
{
    public class DataProviderSearchTest : IClassFixture<ElasticSearchTestProvider>, IIntegrationSearchTests
    {
        private readonly ElasticSearchTestProvider instance;
        private readonly IDataProvider provider;

        public DataProviderSearchTest(ElasticSearchTestProvider instance)
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

        public Task It_finds_data_with_all_filters()
        {
            throw new System.NotImplementedException();
        }

        public Task It_finds_only_data_emitted_after_date()
        {
            throw new System.NotImplementedException();
        }

        public Task It_finds_only_data_emitted_before_date()
        {
            throw new System.NotImplementedException();
        }

        public Task It_finds_only_data_emitted_in_dates_range()
        {
            throw new System.NotImplementedException();
        }

        public Task It_finds_only_data_with_specific_level()
        {
            throw new System.NotImplementedException();
        }

        public Task It_finds_only_data_with_specific_message_content()
        {
            throw new System.NotImplementedException();
        }

        public Task It_finds_same_data_on_same_repeated_search()
        {
            throw new System.NotImplementedException();
        }
    }
}
