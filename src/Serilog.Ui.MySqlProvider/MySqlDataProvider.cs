using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.MySqlProvider.Shared;

namespace Serilog.Ui.MySqlProvider;

public class MySqlDataProvider(RelationalDbOptions options) : DataProvider<MySqlLogModel>(options)
{
    protected override string SelectQuery
        => $"SELECT Id, {ColumnMessageName}, {ColumnLevelName}, {ColumnTimestampName}, Exception, Properties ";

    protected override string SearchCriteriaWhereQuery() => "OR Exception LIKE @Search";

    public override string Name => Options.GetProviderName("MySQL");
}