using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Serilog.Ui.Common.Tests.TestSuites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public Task It_logs_and_throws_when_db_read_breaks_down()
        {
            var mockClient = new Mock<IMongoClient>();
            var mockDb = new Mock<IMongoDatabase>();
            var mockColl = new Mock<IMongoCollection<MongoDbLogModel>>();
            mockClient.Setup(p => p.GetDatabase(It.IsAny<string>(), null))
                .Returns(mockDb.Object);
            mockDb.Setup(p => p.GetCollection<MongoDbLogModel>(It.IsAny<string>(), null)).Returns(() => mockColl.Object);

            var sut = new MongoDbDataProvider(mockClient.Object,
                new MongoDbOptions() { CollectionName = "coll", ConnectionString = "some", DatabaseName = "db" });
            var assert = () => sut.FetchDataAsync(1, 10);
            return assert.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
