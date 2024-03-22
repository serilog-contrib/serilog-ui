using Serilog.Ui.Core;
using Serilog.Ui.MySqlProvider.Shared;

namespace Serilog.Ui.MySqlProvider;

public class MariaDbDataProvider(RelationalDbOptions options) : DataProvider(options)
{
    protected override string ColumnLevelName { get; } = "LogLevel";

    public override string Name => Options.ToDataProviderName("MariaDb");
}