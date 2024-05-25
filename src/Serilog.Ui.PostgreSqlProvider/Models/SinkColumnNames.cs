namespace Serilog.Ui.PostgreSqlProvider.Models;

internal abstract class SinkColumnNames
{
    public string RenderedMessage { get; set; } = string.Empty;

    public string MessageTemplate { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;

    public string Timestamp { get; set; } = string.Empty;

    public string Exception { get; set; } = string.Empty;

    public string LogEventSerialized { get; set; } = string.Empty;
}