using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.SqliteDataProvider;
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
                () => new SqliteDataProvider(null),
            };

            suts.ForEach(sut => sut.Should().ThrowExactly<ArgumentNullException>());
        }

        [Fact]
        public Task It_logs_and_throws_when_db_read_breaks_down()
        {
            var sut = new SqliteDataProvider(new() { ConnectionString = "connString", Schema = "dbo", TableName = "logs" });

            var assert = () => sut.FetchDataAsync(1, 10);
            return assert.Should().ThrowExactlyAsync<ArgumentException>();
        }
    }
}
