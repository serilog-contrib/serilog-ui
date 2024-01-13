using Serilog.Ui.Core;

namespace Serilog.Ui.PostgreSqlProvider;

internal class PostgreSqlDbOptions : RelationalDbOptions
{
    public PostgreSqlSinkType SinkType { get; set; }
}