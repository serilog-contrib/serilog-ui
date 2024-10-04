using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Primitives;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.Core.Models;
using Sqlite.Tests.Util;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sqlite.Tests.DataProvider;

[Collection(nameof(SqliteTestProvider))]
[Trait("Integration-Pagination", "Sqlite")]
public class DataProviderPaginationTest(SqliteTestProvider instance) : IntegrationPaginationTests<SqliteTestProvider>(instance)
{
    [Fact]
    public override Task It_throws_when_skip_is_zero()
    {
        var query = new Dictionary<string, StringValues> { ["page"] = "0", ["count"] = "1" };
        var test = () => Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
        return test.Should().ThrowAsync<SqliteException>();
    }
}
