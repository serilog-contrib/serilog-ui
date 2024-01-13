namespace Serilog.Ui.PostgreSqlProvider;

internal class PostgreSqlAlternativeSinkColumnNames : SinkColumnNames
{
    public const string Exception = "Exception";

    public const string Level = "Level";

    public const string LogEventSerialized = "LogEvent";

    public const string MessageTemplate = "MessageTemplate";

    public const string RenderedMessage = "Message";

    public const string Timestamp = "Timestamp";
}