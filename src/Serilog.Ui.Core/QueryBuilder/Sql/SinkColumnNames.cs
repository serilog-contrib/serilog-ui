namespace Serilog.Ui.Core.QueryBuilder.Sql;

/// <summary>
/// Represents the column names used in the SQL-based sink for logging.
/// </summary>
public abstract class SinkColumnNames
{
    /// <summary>
    /// Gets or sets the message of the log entry.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the message template of the log entry.
    /// </summary>
    public string MessageTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the level of the log entry.
    /// </summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp of the log entry.
    /// </summary>
    public string Timestamp { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the exception of the log entry.
    /// </summary>
    public string Exception { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serialized log event like properties.
    /// </summary>
    public string LogEventSerialized { get; set; } = string.Empty;
}