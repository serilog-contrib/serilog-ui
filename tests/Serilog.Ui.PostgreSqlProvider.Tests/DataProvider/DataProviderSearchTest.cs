using Postgres.Tests.Util;
using Serilog.Ui.PostgreSqlProvider;
using System.Threading.Tasks;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Xunit;

namespace Postgres.Tests.DataProvider;

[Collection(nameof(PostgresDataProvider))]
[Trait("Integration-Search", "Postgres")]
public class DataProviderSearchTest : IntegrationSearchTests_Sink<PostgresTestProvider>
{
    public DataProviderSearchTest(PostgresTestProvider instance) : base(instance)
    {
    }

    public override Task It_finds_data_with_all_filters()
        => It_finds_data_with_all_filters_by_utc(true, true);
    
    public override Task It_finds_only_data_emitted_after_date()
        => It_finds_only_data_emitted_after_date_by_utc(true);
    
    public override Task It_finds_only_data_emitted_before_date()
        => It_finds_only_data_emitted_before_date_by_utc(true);
    
    public override Task It_finds_only_data_emitted_in_dates_range()
        => It_finds_only_data_emitted_in_dates_range_by_utc(true);
}