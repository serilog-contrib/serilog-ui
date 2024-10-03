using Serilog.Ui.MySqlProvider.Extensions;
using Serilog.Ui.MySqlProvider.Shared;

namespace Serilog.Ui.MySqlProvider;

public class MariaDbDataProvider(MariaDbOptions options, MySqlQueryBuilder<MySqlLogModel> queryBuilder)
    : MariaDbDataProvider<MySqlLogModel>(options, queryBuilder);

public class MariaDbDataProvider<T>(MariaDbOptions options, MySqlQueryBuilder<T> queryBuilder) : DataProvider<T>(options, queryBuilder)
    where T : MySqlLogModel

{
    internal const string ProviderName = "MariaDb";

    public override string Name => options.GetProviderName(ProviderName);
}