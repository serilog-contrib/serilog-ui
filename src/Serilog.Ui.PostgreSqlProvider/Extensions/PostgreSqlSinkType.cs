// ReSharper disable InconsistentNaming
namespace Serilog.Ui.PostgreSqlProvider;

/// <summary>
/// Available Postgres sink types.
/// </summary>
public enum PostgreSqlSinkType
{
    /// <remarks>
    /// <a href="https://github.com/b00ted/serilog-sinks-postgresql">Sink</a> 
    /// </remarks>
    SerilogSinksPostgreSQL,
    /// <remarks>
    /// <a href="https://github.com/serilog-contrib/Serilog.Sinks.Postgresql.Alternative">Sink</a> 
    /// </remarks>
    SerilogSinksPostgreSQLAlternative
}