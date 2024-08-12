using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using MongoDb.Tests.Util;
using Serilog.Ui.Common.Tests.TestSuites.Impl;
using Serilog.Ui.Core.Models;
using Serilog.Ui.MongoDbProvider;
using Xunit;

namespace MongoDb.Tests.DataProvider
{
    [Collection(nameof(MongoDbDataProvider))]
    [Trait("Integration-Pagination", "MongoDb")]
    public class DataProviderPaginationTest(BaseIntegrationTest instance) : IntegrationPaginationTests<BaseIntegrationTest>(instance)
    {
        [Fact]
        public override Task It_throws_when_skip_is_zero()
        {
            var query = new Dictionary<string, StringValues> { ["page"] = "0", ["count"] = "1" };
            var test = () => Provider.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            return test.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
    }
}