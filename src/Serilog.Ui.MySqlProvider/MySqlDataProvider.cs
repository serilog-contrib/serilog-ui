using Serilog.Ui.MySqlProvider.Extensions;
using Serilog.Ui.MySqlProvider.Shared;

namespace Serilog.Ui.MySqlProvider;

public class MySqlDataProvider(MySqlDbOptions options, MySqlQueryBuilder<MySqlLogModel> queryBuilder)
    : DataProvider<MySqlLogModel>(options, queryBuilder)
{
    private readonly MySqlDbOptions _options = options;

    internal const string MySqlProviderName = "MySQL";

    public override string Name => _options.GetProviderName(MySqlProviderName);
}