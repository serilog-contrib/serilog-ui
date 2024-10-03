using Serilog.Ui.Core.QueryBuilder.Sql;

namespace Serilog.Ui.MySqlProvider.Models;

internal class MariaDbSinkColumnNames : SinkColumnNames
{
    public MariaDbSinkColumnNames()
    {
        Exception = "Exception";
        Level = "LogLevel";
        LogEventSerialized = "Properties";
        Message = "Message";
        MessageTemplate = "";
        Timestamp = "TimeStamp";
    }
}