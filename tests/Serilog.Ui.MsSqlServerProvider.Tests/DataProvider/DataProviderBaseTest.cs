using FluentAssertions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.MsSqlServerProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using MsSql.Tests.Util;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.OptionsBuilder;
using Xunit;

namespace MsSql.Tests.DataProvider
{
    [Trait("Unit-Base", "MsSql")]
    public class DataProviderBaseTest : IUnitBaseTests
    {
        [Fact]
        public void It_throws_when_any_dependency_is_null()
        {
            var suts = new List<Action>
            {
                () => { _ = new SqlServerDataProvider(null); },
                () => { _ = new SqlServerDataProvider<SqlServerTestModel>(null); },
            };

            suts.ForEach(sut => sut.Should().ThrowExactly<ArgumentNullException>());
        }

        [Fact]
        public async Task It_logs_and_throws_when_db_read_breaks_down()
        {
            // Arrange
            var sut = new SqlServerDataProvider(new RelationalDbOptions("dbo").WithConnectionString("connString").WithTable("logs"));
            var sutWithAdditionalCols = new SqlServerDataProvider<SqlServerTestModel>(new RelationalDbOptions("dbo").WithConnectionString("connString").WithTable("logs"));
            var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "10", };

            // Act
            var assert = () => sut.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            var assertWithAdditionalCols = () => sutWithAdditionalCols.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            
            await assert.Should().ThrowExactlyAsync<ArgumentException>();
            await assertWithAdditionalCols.Should().ThrowExactlyAsync<ArgumentException>();
        }
    }
}