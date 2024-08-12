using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Raven.Client.Exceptions;
using RavenDb.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.Core.Models;
using Serilog.Ui.RavenDbProvider;

namespace RavenDb.Tests.DataProvider;

[Collection(nameof(RavenDbDataProvider))]
[Trait("Integration-Pagination", "RavenDb")]
public class DataProviderPaginationTest(RavenDbTestProvider instance) : IntegrationPaginationTests<RavenDbTestProvider>(instance)
{
    [Fact]
    public override Task It_throws_when_skip_is_zero()
    {
        var query = new Dictionary<string, StringValues> { ["page"] = "0", ["count"] = "1" };
        var test = () => Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
        return test.Should().ThrowAsync<InvalidQueryException>();
    }
}