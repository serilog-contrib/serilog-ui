using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MongoDbProvider;
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
            var mongoClientMock = Substitute.For<IMongoClient>();
            var mongoDbMock = Substitute.For<IMongoDatabase>();
            var mongoCollectionMock = Substitute.For<IMongoCollection<MongoDbLogModel>>();
            mongoClientMock
                .GetDatabase(Arg.Any<string>())
                .Returns(mongoDbMock);
            mongoDbMock.GetCollection<MongoDbLogModel>(Arg.Any<string>()).Returns(mongoCollectionMock);
            mongoCollectionMock.Indexes.Throws(new ArithmeticException());

            var sut = new MongoDbDataProvider(mongoClientMock,
                new MongoDbOptions().WithConnectionString("some").WithCollectionName("coll").WithDatabaseName("db"));

            var query = new Dictionary<string, StringValues> { ["page"] = "1", ["count"] = "10", ["search"] = "break-db" };
            var assert = () => sut.FetchDataAsync(FetchLogsQuery.ParseQuery(query));
            return assert.Should().ThrowAsync<ArithmeticException>();
        }
    }
}