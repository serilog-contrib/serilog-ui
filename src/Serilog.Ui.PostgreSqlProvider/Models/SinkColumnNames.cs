namespace Serilog.Ui.PostgreSqlProvider.Models;

internal abstract class SinkColumnNames
{
    public string RenderedMessage { get; set; }

    public string MessageTemplate { get; set; }

    public string Level { get; set; }

    public string Timestamp { get; set; }

    public string Exception { get; set; }

    public string LogEventSerialized { get; set; }
}