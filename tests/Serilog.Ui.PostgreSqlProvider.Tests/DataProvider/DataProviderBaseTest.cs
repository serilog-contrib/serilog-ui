using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.PostgreSqlProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.PostgreSqlProvider.Extensions;
using Serilog.Ui.PostgreSqlProvider.Models;
using Xunit;

namespace Postgres.Tests.DataProvider
{
    [Trait("Unit-Base", "Postgres")]
    public class DataProviderBaseTest : IUnitBaseTests
    {
        [Fact]
        public void It_throws_when_any_dependency_is_null()
        {
            var sut = new List<Func<PostgresDataProvider>>
            {
                () => new PostgresDataProvider(null),
            };

            sut.ForEach(s => s.Should().ThrowExactly<ArgumentNullException>());
        }

        [Fact]
        public Task It_logs_and_throws_when_db_read_breaks_down()
        {
            QueryBuilder.SetSinkType(PostgreSqlSinkType.SerilogSinksPostgreSQLAlternative);
            var sut = new PostgresDataProvider(new PostgreSqlDbOptions("dbo")
                .WithConnectionString("connString")
                .WithTable("logs"));

            var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "10" };
            var assert = () => sut.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            return assert.Should().ThrowExactlyAsync<ArgumentException>();
        }
    }
}