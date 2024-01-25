namespace Serilog.Ui.PostgreSqlProvider;

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