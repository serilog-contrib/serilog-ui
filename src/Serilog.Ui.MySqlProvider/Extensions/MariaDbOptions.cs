using Serilog.Ui.MySqlProvider.Models;

namespace Serilog.Ui.MySqlProvider.Extensions;

public class MariaDbOptions : MySqlDbOptions
{
    public MariaDbOptions(string defaultSchemaName) : base(defaultSchemaName)
    {
        ColumnNames = new MariaDbSinkColumnNames();
    }
}