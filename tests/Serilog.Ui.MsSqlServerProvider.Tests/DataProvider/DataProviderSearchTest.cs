using MsSql.Tests.Util;
using Serilog.Ui.MsSqlServerProvider;
using System.Threading.Tasks;
using Xunit;

namespace MsSql.Tests.DataProvider
{
    [Collection(nameof(SqlServerDataProvider))]
    [Trait("Integration-Search", "MsSql")]
    public class DataProviderSearchTest : IntegrationSearchTests<MsSqlServerTestProvider>
    {
        public DataProviderSearchTest(MsSqlServerTestProvider instance) : base(instance) { }

        public override Task It_finds_all_data_with_default_search()
            => base.It_finds_all_data_with_default_search();

        public override Task It_finds_data_with_all_filters()
            => base.It_finds_data_with_all_filters();

        public override Task It_finds_only_data_emitted_after_date()
            => base.It_finds_only_data_emitted_after_date();

        public override Task It_finds_only_data_emitted_before_date()
            => base.It_finds_only_data_emitted_before_date();

        public override Task It_finds_only_data_emitted_in_dates_range()
            => base.It_finds_only_data_emitted_in_dates_range();

        public override Task It_finds_only_data_with_specific_level()
            => base.It_finds_only_data_with_specific_level();

        public override Task It_finds_only_data_with_specific_message_content()
            => base.It_finds_only_data_with_specific_message_content();

        public override Task It_finds_same_data_on_same_repeated_search()
            => base.It_finds_same_data_on_same_repeated_search();
    }
}