using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MySqlProvider;
using Xunit;

namespace MySql.Tests.DataProvider.MariaDb
{
    [Trait("Unit-Base", "MariaDb")]
    public class DataProviderBaseTest : IUnitBaseTests
    {
        [Fact]
        public void It_throws_when_any_dependency_is_null()
        {
            var suts = new List<Action>
            {
                () => {_ = new MariaDbDataProvider(null);},
                () => {_ = new MariaDbDataProvider<MariaDbTestModel>(null);},
            };

            suts.ForEach(sut => sut.Should().ThrowExactly<ArgumentNullException>());
        }

        [Fact]
        public async Task It_logs_and_throws_when_db_read_breaks_down()
        {
            var sut = new MariaDbDataProvider(new RelationalDbOptions("dbo").WithConnectionString("connString").WithTable("logs"));
            var sutWithCols = new MariaDbDataProvider<MariaDbTestModel>(new RelationalDbOptions("dbo").WithConnectionString("connString").WithTable("logs"));
            var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "10", };

            var assert = () => sut.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            var assertWithAdditionalCols = () => sutWithCols.FetchDataAsync(FetchLogsQuery.ParseQuery(query));

            await assert.Should().ThrowExactlyAsync<ArgumentException>();
            await assertWithAdditionalCols.Should().ThrowExactlyAsync<ArgumentException>();
        }
    }
}