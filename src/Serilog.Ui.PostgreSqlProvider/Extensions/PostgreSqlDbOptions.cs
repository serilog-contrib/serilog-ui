using Serilog.Ui.Core;

namespace Serilog.Ui.PostgreSqlProvider;

/// <inheritdoc/> - Postgres implementation
public class PostgreSqlDbOptions : RelationalDbOptions
{
    /// <summary>
    /// It gets or sets SinkType.
    /// </summary>
    public PostgreSqlSinkType SinkType { get; set; }
}