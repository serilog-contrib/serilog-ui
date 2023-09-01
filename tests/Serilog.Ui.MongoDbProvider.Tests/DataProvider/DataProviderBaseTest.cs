using FluentAssertions;
using MongoDB.Driver;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.MongoDbProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MongoDb.Tests.DataProvider
{
    [Trait("Unit-Base", "MongoDb")]
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
            var mockClient = Substitute.For<IMongoClient>();
            var mockDb = Substitute.For<IMongoDatabase>();
            var mockColl = Substitute.For<IMongoCollection<MongoDbLogModel>>();
            mockClient
                .GetDatabase(Arg.Any<string>(), null)
                .Returns(mockDb);
            mockDb.GetCollection<MongoDbLogModel>(Arg.Any<string>(), null).Returns(mockColl);
            mockColl.Indexes.Throws(new ArithmeticException());

            var sut = new MongoDbDataProvider(mockClient,
                new MongoDbOptions() { CollectionName = "coll", ConnectionString = "some", DatabaseName = "db" });
            var assert = () => sut.FetchDataAsync(1, 10, searchCriteria: "break-db");
            return assert.Should().ThrowAsync<ArithmeticException>();
        }
    }
}
