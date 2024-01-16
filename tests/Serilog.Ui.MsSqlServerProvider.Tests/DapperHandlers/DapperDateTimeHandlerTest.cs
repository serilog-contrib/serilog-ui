using System;
using System.Data;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Serilog.Ui.MsSqlServerProvider;
using Xunit;

namespace MsSql.Tests.DapperHandlers;

public class DapperDateTimeHandlerTest
{
    private readonly DapperDateTimeHandler _sut = new();

    [Fact]
    public void It_parse_simple_datetime_with_local_style_and_current_culture()
    {
        // ARRANGE
        var specificCulture = CultureInfo.CreateSpecificCulture("en-US");
        var time = new DateTime(1999, 12, 31, 01, 02, 40, DateTimeKind.Local);
        var stringify = time.ToString("yyyy-MM-dd hh:mm:ss", specificCulture);
        Thread.CurrentThread.CurrentCulture = specificCulture;
        // ACT
        var result = _sut.Parse(stringify);

        // ASSERT
        result.Should().Be(time.ToUniversalTime());
        result.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Theory]
    [InlineData("M/d/yyyy h:mm:ss tt zzz")]
    [InlineData("M/dd/yyyy h:mm:ss tt zzz")]
    [InlineData("M/d/yyyy h:mm:ss tt")]
    [InlineData("M/dd/yyyy h:mm:ss tt")]
    [InlineData("M/dd/yyyy hh:mm:ss")]
    [InlineData("MM/dd/yyyy HH:mm:ss")]
    [InlineData("MM/d/yyyy hh:mm:ss tt")]
    [InlineData("M/dd/yyyy hh:mm:ss tt")]
    [InlineData("MM/dd/yyyy hh:mm:ss tt")]
    public void It_parse_exact_formats_with_none_style_and_invariant_culture(string customFormat)
    {
        // ARRANGE
        var specificCulture = CultureInfo.CreateSpecificCulture("en-US");
        var time = new DateTime(1999, 12, 31, 01, 02, 40, DateTimeKind.Unspecified);
        var stringify = time.ToString(customFormat, specificCulture);
        Thread.CurrentThread.CurrentCulture = specificCulture;
        // ACT
        var result = _sut.Parse(stringify);

        // ASSERT
        result.Should().Be(time.ToUniversalTime());
        result.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void It_parse_strange_format_using_custom_delegate()
    {
        // ARRANGE
        var sut = new DapperDateTimeHandler(par => DateTime
            .ParseExact(par, "MM/dd/yyyy ss|mm|hh", CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal)
            .ToUniversalTime()
        );
        var time = new DateTime(1999, 12, 31, 01, 02, 40, DateTimeKind.Local);
        var stringifyStrangeFormat = time.ToString("MM/dd/yyyy ss|mm|hh", CultureInfo.CreateSpecificCulture("en-US"));

        // ACT
        var result = sut.Parse(stringifyStrangeFormat);

        // ASSERT
        var originalUtcTime = time.ToUniversalTime();
        result.Should().Be(originalUtcTime);
        result.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void It_fail_parse_with_current_culture_when_original_format_culture_was_different()
    {
        // ARRANGE
        var time = new DateTime(1999, 12, 31, 01, 02, 40, DateTimeKind.Local);
        var stringifyWithDifferentCulture = time.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US"));
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-AR");

        // ACT
        var result = _sut.Parse(stringifyWithDifferentCulture);

        // ASSERT
        result.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public void It_throws_when_using_custom_delegate_not_returns_datetime_with_utc_kind()
    {
        var sut = new DapperDateTimeHandler(par => DateTime
            .ParseExact(par, "MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal)
        );
        var time = new DateTime(1999, 12, 31, 01, 02, 40, DateTimeKind.Local);
        var stringify = time.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US"));

        var result = () => sut.Parse(stringify);

        result.Should().ThrowExactly<InvalidOperationException>().WithMessage($"The parsed date must have kind: {DateTimeKind.Utc}");
    }

    [Fact]
    public void It_sets_value_without_changes()
    {
        var param = new SqlParameter("test", SqlDbType.Date);
        var value = DateTime.Now.AddHours(-2);
        _sut.SetValue(param, value);

        param.Value.Should().BeAssignableTo<DateTime>().And.Be(value);
    }
}