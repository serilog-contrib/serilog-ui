namespace Serilog.Ui.PostgreSqlProvider.Models;

internal class PostgreSqlAlternativeSinkColumnNames : SinkColumnNames
{
    public PostgreSqlAlternativeSinkColumnNames()
    {
        Exception = "Exception";
        Level = "Level";
        LogEventSerialized = "LogEvent";
        MessageTemplate = "MessageTemplate";
        RenderedMessage = "Message";
        Timestamp = "Timestamp";
    }
}