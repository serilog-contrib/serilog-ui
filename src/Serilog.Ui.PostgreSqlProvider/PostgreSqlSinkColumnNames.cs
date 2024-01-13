namespace Serilog.Ui.PostgreSqlProvider;

internal abstract class SinkColumnNames
{
    public string RenderedMessage;

    public string MessageTemplate;

    public string Level;

    public string Timestamp;

    public string Exception;

    public string LogEventSerialized;
}

internal class PostgreSqlSinkColumnNames : SinkColumnNames
{
    public const string RenderedMessage = "message";

    public const string MessageTemplate = "message_template";

    public const string Level = "level";

    public const string Timestamp = "timestamp";

    public const string Exception = "exception";

    public const string LogEventSerialized = "log_event";
}