using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Raven.Client.Documents;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core.Models;
using Serilog.Ui.RavenDbProvider;
using Serilog.Ui.RavenDbProvider.Extensions;

namespace RavenDb.Tests.DataProvider;

[Trait("Unit-Base", "RavenDb")]
public class DataProviderBaseTest : IUnitBaseTests
{
    private readonly RavenDbOptions _ravenDbOptions = new RavenDbOptions().WithCollectionName("collection");

    [Fact]
    public void It_throws_when_any_dependency_is_null()
    {
        var suts = new List<Func<RavenDbDataProvider>>
        {
            () => { return new RavenDbDataProvider(null!, _ravenDbOptions); },
            () => new RavenDbDataProvider(new DocumentStore(), null!)
        };

        suts.ForEach(sut => sut.Should().ThrowExactly<ArgumentNullException>());
    }

    [Fact]
    public Task It_logs_and_throws_when_db_read_breaks_down()
    {
        var sut = new RavenDbDataProvider(new DocumentStore(), _ravenDbOptions);

        var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "10" };
        var assert = () => sut.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
        return assert.Should().ThrowExactlyAsync<InvalidOperationException>();
    }
}