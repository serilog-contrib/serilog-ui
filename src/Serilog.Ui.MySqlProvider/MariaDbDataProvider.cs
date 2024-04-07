using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MySqlProvider.Shared;

namespace Serilog.Ui.MySqlProvider;


public class MariaDbDataProvider(RelationalDbOptions options) : MariaDbDataProvider<MySqlLogModel>(options)
{
    protected override string SelectQuery
        => $"SELECT Id, {ColumnMessageName}, {ColumnLevelName} AS 'Level', {ColumnTimestampName}, Exception, Properties ";

    protected override string SearchCriteriaWhereQuery() => "OR Exception LIKE @Search";
}


public class MariaDbDataProvider<T>(RelationalDbOptions options) : DataProvider<T>(options)
    where T : MySqlLogModel

{
    internal const string ProviderName = "MariaDb";

    protected override string ColumnLevelName { get; } = "LogLevel";

    public override string Name => Options.ToDataProviderName(ProviderName);
}