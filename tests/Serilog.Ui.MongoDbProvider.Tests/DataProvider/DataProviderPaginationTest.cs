using FluentAssertions;
using MongoDb.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.MongoDbProvider;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MongoDb.Tests.DataProvider
{
    [Collection(nameof(MongoDbDataProvider))]
    [Trait("Integration-Pagination", "MongoDb")]
    public class DataProviderPaginationTest : IntegrationPaginationTests<BaseIntegrationTest>
    {
        public DataProviderPaginationTest(BaseIntegrationTest instance) : base(instance)
        {
        }

        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var test = () => Provider.FetchDataAsync(0, 1);
            return test.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
    }
}