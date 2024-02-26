using MongoDb.Tests.Util;
using Serilog.Ui.MongoDbProvider;
using System.Threading.Tasks;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Xunit;

namespace MongoDb.Tests.DataProvider
{
    [Collection(nameof(MongoDbDataProvider))]
    [Trait("Integration-Search", "MongoDb")]
    public class DataProviderSearchTest : IntegrationSearchTests<BaseIntegrationTest>
    {
        public DataProviderSearchTest(BaseIntegrationTest instance) : base(instance) { }

        public override Task It_finds_data_with_all_filters()
            => It_finds_data_with_all_filters_by_utc(true, true);

        public override Task It_finds_only_data_emitted_after_date()
            => It_finds_only_data_emitted_after_date_by_utc(true);

        public override Task It_finds_only_data_emitted_before_date()
            => It_finds_only_data_emitted_before_date_by_utc(true);

        public override Task It_finds_only_data_emitted_in_dates_range()
            => It_finds_only_data_emitted_in_dates_range_by_utc(true);
    }
}
