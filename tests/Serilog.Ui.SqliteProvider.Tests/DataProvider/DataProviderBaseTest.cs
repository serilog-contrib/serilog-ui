using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Models;
using Serilog.Ui.SqliteDataProvider;
using Serilog.Ui.SqliteDataProvider.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sqlite.Tests.DataProvider
{
    [Trait("Unit-Base", "Sqlite")]
    public class DataProviderBaseTest : IUnitBaseTests
    {
        [Fact]
        public void It_throws_when_any_dependency_is_null()
        {
            var suts = new List<Func<SqliteDataProvider>>
            {
                () => new SqliteDataProvider(null!, new SqliteQueryBuilder()),
            };

            suts.ForEach(sut => sut.Should().ThrowExactly<ArgumentNullException>());
        }

        [Fact]
        public Task It_logs_and_throws_when_db_read_breaks_down()
        {
            var sut = new SqliteDataProvider(
                new SqliteDbOptions(string.Empty).WithConnectionString("connString").WithTable("Logs"),
                new SqliteQueryBuilder()
                );

            Dictionary<string, StringValues> query = new() { ["page"] = "1", ["count"] = "10" };

            var assert = () => sut.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            return assert.Should().ThrowExactlyAsync<ArgumentException>();
        }
    }
}
