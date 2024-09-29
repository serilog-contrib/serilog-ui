using Serilog.Ui.Core.QueryBuilder.Sql;

namespace Serilog.Ui.PostgreSqlProvider.Models;

internal class PostgreSqlAlternativeSinkColumnNames : SinkColumnNames
{
    public PostgreSqlAlternativeSinkColumnNames()
    {
        Exception = "Exception";
        Level = "Level";
        LogEventSerialized = "LogEvent";
        Message = "Message";
        MessageTemplate = "MessageTemplate";
        Timestamp = "Timestamp";
    }
}