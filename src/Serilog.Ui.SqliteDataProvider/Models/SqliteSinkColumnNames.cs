using Serilog.Ui.Core.QueryBuilder.Sql;

namespace Serilog.Ui.SqliteDataProvider.Models;

internal class SqliteSinkColumnNames : SinkColumnNames
{
    public SqliteSinkColumnNames()
    {
        Exception = "Exception";
        Level = "Level";
        LogEventSerialized = "Properties";
        Message = "RenderedMessage";
        MessageTemplate = "";
        Timestamp = "Timestamp";
    }
}