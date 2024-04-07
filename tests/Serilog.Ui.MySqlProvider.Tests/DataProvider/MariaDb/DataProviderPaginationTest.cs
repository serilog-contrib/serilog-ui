using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using MySqlConnector;
using MySql.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.Core.Models;
using Xunit;

namespace MySql.Tests.DataProvider.MariaDb
{
    [Collection(nameof(MariaDbTestProvider))]
    [Trait("Integration-Pagination", "MariaDb")]
    public class DataProviderPaginationTest(MariaDbTestProvider instance) : IntegrationPaginationTests<MariaDbTestProvider>(instance)
    {
        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var query = new Dictionary<string, StringValues> { ["page"] = "0", ["count"] = "1" };
            var test = () => Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            return test.Should().ThrowAsync<MySqlException>();
        }
    }

    [Collection(nameof(MariaDbAdditionalColsTestProvider))]
    [Trait("Integration-Pagination", "MariaDb")]
    public class DataProviderPaginationAdditionalColsTest(MariaDbAdditionalColsTestProvider instance) : IntegrationPaginationTests<MariaDbAdditionalColsTestProvider>(instance)
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