using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.Core.QueryBuilder.Sql;
using Serilog.Ui.MySqlProvider.Models;

namespace Serilog.Ui.MySqlProvider.Extensions;

public class MySqlDbOptions(string defaultSchemaName) : RelationalDbOptions(defaultSchemaName)
{
    internal SinkColumnNames ColumnNames { get; set; } = new MySqlSinkColumnNames();
}