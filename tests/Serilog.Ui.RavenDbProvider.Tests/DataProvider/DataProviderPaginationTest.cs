using FluentAssertions;
using Raven.Client.Exceptions;
using RavenDb.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.RavenDbProvider;

namespace RavenDb.Tests.DataProvider;

[Collection(nameof(RavenDbDataProvider))]
[Trait("Integration-Pagination", "RavenDb")]
public class DataProviderPaginationTest : IntegrationPaginationTests<RavenDbTestProvider>
{
    public DataProviderPaginationTest(RavenDbTestProvider instance) : base(instance)
    {
    }

    [Fact]
    public override Task It_throws_when_skip_is_zero()
    {
        var test = () => Provider.FetchDataAsync(0, 1);
        return test.Should().ThrowAsync<InvalidQueryException>();
    }
}