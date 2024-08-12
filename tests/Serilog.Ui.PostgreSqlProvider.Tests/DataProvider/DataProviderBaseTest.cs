using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Postgres.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Models;
using Serilog.Ui.PostgreSqlProvider;
using Serilog.Ui.PostgreSqlProvider.Extensions;
using Xunit;

namespace Postgres.Tests.DataProvider
{
    [Trait("Unit-Base", "Postgres")]
    public class DataProviderBaseTest : IUnitBaseTests
    {
        [Fact]
        public void It_throws_when_any_dependency_is_null()
        {
            var sut = new List<Action>
            {
                () => { _ = new PostgresDataProvider(null!); },
                () => { _ = new PostgresDataProvider<PostgresTestModel>(null!); },
            };

            sut.ForEach(s => s.Should().ThrowExactly<ArgumentNullException>());
        }

        [Fact]
        public async Task It_logs_and_throws_when_db_read_breaks_down()
        {
            var sut = new PostgresDataProvider(new PostgreSqlDbOptions("dbo")
                .WithConnectionString("connString")
                .WithTable("logs"));
            var sutWithCols = new PostgresDataProvider<PostgresTestModel>(new PostgreSqlDbOptions("dbo")
                .WithConnectionString("connString")
                .WithTable("logs"));
            var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "10" };

            var assert = () => sut.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            var assertWithCols = () => sutWithCols.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            await assert.Should().ThrowExactlyAsync<ArgumentException>();
            await assertWithCols.Should().ThrowExactlyAsync<ArgumentException>();
        }
    }
}