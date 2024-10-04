using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.Core.QueryBuilder.Sql;
using Serilog.Ui.SqliteDataProvider.Models;

namespace Serilog.Ui.SqliteDataProvider.Extensions;

public class SqliteDbOptions() : RelationalDbOptions("ununsed")
{
    public SinkColumnNames ColumnNames { get; } = new SqliteSinkColumnNames();
}
