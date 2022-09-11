using FluentAssertions;
using MongoDB.Driver;
using Serilog.Ui.Common.Tests.TestSuites;
using System;
using System.Collections.Generic;
using Xunit;

namespace Serilog.Ui.MongoDbProvider.Tests.DataProvider
{
    public class DataProviderBaseTest : IUnitBaseTests
    {
        [Fact]
        public void It_throws_when_any_dependency_is_null()
        {
            var suts = new List<Func<MongoDbDataProvider>>
            {
                () => new MongoDbDataProvider(null, null),
                () => new MongoDbDataProvider(new MongoClient(), null),
                () => new MongoDbDataProvider(null, new MongoDbOptions()),
            };

            suts.ForEach(sut => sut.Should().ThrowExactly<ArgumentNullException>());
        }
    }
}
