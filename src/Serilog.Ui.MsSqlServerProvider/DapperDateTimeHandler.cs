#nullable enable
using Dapper;
using System;
using System.Data;
using System.Globalization;

namespace Serilog.Ui.MsSqlServerProvider;

public class DapperDateTimeHandler : SqlMapper.TypeHandler<DateTime>
{
    private readonly Func<string, DateTime>? _dateTimeCustomParsing;

    public DapperDateTimeHandler(Func<string, DateTime>? dateTimeCustomParsing = null)
    {
        _dateTimeCustomParsing = dateTimeCustomParsing;
    }

    private static readonly string[] Formats = new[]
    {
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

        if (_dateTimeCustomParsing is not null) return CustomParse(valueStr);

        DateTime.TryParse(valueStr, CultureInfo.CurrentCulture, DateTimeStyles.None, out var timeStamp);

        if (timeStamp == default)
            DateTime.TryParseExact(valueStr, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out timeStamp);

        return timeStamp;
    }

    private DateTime CustomParse(string value)
    {
        var parseDatetime = _dateTimeCustomParsing!(value);

        if (parseDatetime.Kind != DateTimeKind.Utc)
            throw new InvalidOperationException($"The parsed date must have kind: {DateTimeKind.Utc}");

        return parseDatetime;
    }
}