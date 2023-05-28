using Dapper;
using System;
using System.Data;
using System.Globalization;

namespace Serilog.Ui.MsSqlServerProvider;

public class DapperDateTimeHandler : SqlMapper.TypeHandler<DateTime>
{
    private static string[] Formats = new[] {
        "M/d/yyyy h:mm:ss tt zzz",
        "M/dd/yyyy h:mm:ss tt zzz",
        "M/d/yyyy h:mm:ss tt",
        "M/dd/yyyy h:mm:ss tt",
        "M/dd/yyyy hh:mm:ss",
        "MM/dd/yyyy HH:mm:ss",
        "MM/d/yyyy hh:mm:ss tt",
        "M/dd/yyyy hh:mm:ss tt",
        "MM/dd/yyyy hh:mm:ss tt",
    };

    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.Value = value;
    }

    public override DateTime Parse(object value)
    {
        var valueStr = value.ToString();
        DateTime.TryParse(valueStr, out var timeStamp);

        if (timeStamp == default)
            DateTime.TryParseExact(valueStr, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out timeStamp);

        return timeStamp;
    }
}