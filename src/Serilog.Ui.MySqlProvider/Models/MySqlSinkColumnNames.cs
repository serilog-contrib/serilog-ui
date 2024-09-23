using Serilog.Ui.Core.QueryBuilder.Sql;

namespace Serilog.Ui.MySqlProvider.Models;

internal class MySqlSinkColumnNames : SinkColumnNames
{
    public MySqlSinkColumnNames()
    {
        Exception = "Exception";
        Level = "Level";
        LogEventSerialized = "Properties";
        Message = "Message";
        MessageTemplate = "";
        Timestamp = "TimeStamp";
    }
}