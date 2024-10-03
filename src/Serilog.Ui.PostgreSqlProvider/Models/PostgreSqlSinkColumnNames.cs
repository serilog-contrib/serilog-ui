using Serilog.Ui.Core.QueryBuilder.Sql;

namespace Serilog.Ui.PostgreSqlProvider.Models;

internal class PostgreSqlSinkColumnNames : SinkColumnNames
{
    public PostgreSqlSinkColumnNames()
    {
        Exception = "exception";
        Level = "level";
        LogEventSerialized = "log_event";
        Message = "message";
        MessageTemplate = "message_template";
        Timestamp = "timestamp";
    }
}