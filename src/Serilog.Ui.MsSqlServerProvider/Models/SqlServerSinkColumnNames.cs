using Serilog.Ui.Core.QueryBuilder.Sql;

namespace Serilog.Ui.MsSqlServerProvider.Models;

internal class SqlServerSinkColumnNames : SinkColumnNames
{
    public SqlServerSinkColumnNames()
    {
        Exception = "Exception";
        Level = "Level";
        LogEventSerialized = "Properties";
        Message = "Message";
        MessageTemplate = "";
        Timestamp = "TimeStamp";
    }
}