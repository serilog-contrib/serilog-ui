using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.PostgreSqlProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Ui.Core.OptionsBuilder;
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

            var assert = () => sut.FetchDataAsync(1, 10);
            return assert.Should().ThrowExactlyAsync<ArgumentException>();
        }
    }
}