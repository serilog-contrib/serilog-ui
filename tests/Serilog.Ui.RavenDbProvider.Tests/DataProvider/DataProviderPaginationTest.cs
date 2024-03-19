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

    public override Task It_fetches_with_limit() => base.It_fetches_with_limit();

    public override Task It_fetches_with_limit_and_skip() => base.It_fetches_with_limit_and_skip();

    public override Task It_fetches_with_skip() => base.It_fetches_with_skip();

    [Fact]
    public override Task It_throws_when_skip_is_zero()
    {
        var test = () => provider.FetchDataAsync(0, 1);
        return test.Should().ThrowAsync<InvalidQueryException>();
    }
}