using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using MySql.Tests.Util;
using MySqlConnector;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.Core.Models;
using Serilog.Ui.MySqlProvider;
using Xunit;

namespace MySql.Tests.DataProvider.MySql
{
    [Collection(nameof(MySqlDataProvider))]
    [Trait("Integration-Pagination", "MySql")]
    public class DataProviderPaginationTest(MySqlTestProvider instance) : IntegrationPaginationTests<MySqlTestProvider>(instance)
    {
        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var query = new Dictionary<string, StringValues> { ["page"] = "0", ["count"] = "1" };
            var test = () => Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            return test.Should().ThrowAsync<MySqlException>();
        }
    }
}