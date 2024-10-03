using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Models;
using Serilog.Ui.MySqlProvider;
using Serilog.Ui.MySqlProvider.Extensions;
using Xunit;

namespace MySql.Tests.DataProvider.MySql;

[Trait("Unit-Base", "MySql")]
public class DataProviderBaseTest : IUnitBaseTests
{
    [Fact(Skip = "Not required")]
    public void It_throws_when_any_dependency_is_null()
        => throw new NotImplementedException();

    [Fact]
    public Task It_logs_and_throws_when_db_read_breaks_down()
    {
        // Arrange
        MySqlDataProvider sut = new(
            new MySqlDbOptions("dbo").WithConnectionString("connString").WithTable("logs"),
            new MySqlQueryBuilder<MySqlLogModel>());

        Dictionary<string, StringValues> query = new() { ["page"] = "1", ["count"] = "10" };

        // Act
        var assert = () => sut.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

        // Assert
        return assert.Should().ThrowExactlyAsync<ArgumentException>();
    }
}