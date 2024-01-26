using Serilog.Ui.Core;

namespace Serilog.Ui.PostgreSqlProvider;

public class PostgreSqlDbOptions : RelationalDbOptions
{
    public PostgreSqlSinkType SinkType { get; set; }
}