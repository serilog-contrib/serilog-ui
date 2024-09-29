using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.Core.QueryBuilder.Sql;
using Serilog.Ui.MsSqlServerProvider.Models;

namespace Serilog.Ui.MsSqlServerProvider.Extensions;

public class SqlServerDbOptions(string defaultSchemaName) : RelationalDbOptions(defaultSchemaName)
{
    public SinkColumnNames ColumnNames { get; } = new SqlServerSinkColumnNames();
}