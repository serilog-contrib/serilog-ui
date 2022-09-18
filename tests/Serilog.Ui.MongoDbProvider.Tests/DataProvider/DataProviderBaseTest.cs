using FluentAssertions;
using MongoDB.Driver;
using Moq;
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

        [Fact]
        public void It_logs_and_throws_when_db_read_breaks_down()
        {
            var mockClient = new Mock<IMongoClient>();
            var mockDb = new Mock<IMongoDatabase>();
            mockClient.Setup(p => p.GetDatabase(It.IsAny<string>(), null))
                .Returns(mockDb.Object);
            mockDb.Setup(p => p.GetCollection<MongoDbLogModel>(It.IsAny<string>(), null)).Returns(() => null);

            var sut = new MongoDbDataProvider(mockClient.Object, new MongoDbOptions());

            var assert = () => sut.FetchDataAsync(1, 10);
            assert.Should().ThrowExactlyAsync<NullReferenceException>();
        }
    }
}
