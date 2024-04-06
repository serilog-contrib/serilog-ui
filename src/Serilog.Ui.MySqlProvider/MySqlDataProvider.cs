using Serilog.Ui.Core;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MySqlProvider.Shared;

namespace Serilog.Ui.MySqlProvider;

public class MySqlDataProvider(RelationalDbOptions options) : DataProvider(options)
{
    public override string Name => Options.ToDataProviderName("MySQL");
}