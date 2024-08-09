using System;
using System.Collections.Generic;
using FluentAssertions;
using MongoDB.Driver;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MongoDbProvider;
using Xunit;

namespace MongoDb.Tests.Extensions;

[Trait("Unit-MongoDbOptions", "MongoDb")]
public class MongoDbOptionsTest
{
    [Fact]
    public void It_validates_options()
    {
        var result = () => new MongoDbOptions().WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName("name")
            .WithDatabaseName("db").Validate();

        result.Should().NotThrow();
    }

    [Fact]
    public void It_throws_on_validation_failed()
    {
        var nullables = new List<Action>
        {
            () => new MongoDbOptions().WithConnectionString(null!).WithCollectionName("name").WithDatabaseName("db").Validate(),
            () => new MongoDbOptions().WithConnectionString(" ").WithCollectionName("name").WithDatabaseName("db").Validate(),
            () => new MongoDbOptions().WithConnectionString("").WithCollectionName("name").WithDatabaseName("db").Validate(),
            () => new MongoDbOptions().WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName(null!).WithDatabaseName("db")
                .Validate(),
            () => new MongoDbOptions().WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName(" ").WithDatabaseName("db")
                .Validate(),
            () => new MongoDbOptions().WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName("").WithDatabaseName("db")
                .Validate(),
            () => new MongoDbOptions().WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName("name")
                .WithDatabaseName(null!)
                .Validate(),
            () => new MongoDbOptions().WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName("name").WithDatabaseName("")
                .Validate(),
            () => new MongoDbOptions().WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName("name").WithDatabaseName(" ")
                .Validate(),
        };

        foreach (var nullable in nullables)
        {
            nullable.Should().Throw<ArgumentException>();
        }

        // database name not provided and not found in connection string
        var act = () => new MongoDbOptions().WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName("name").Validate();
        act.Should().ThrowExactly<ArgumentNullException>();

        // invalid connection string
        var actConfig = () => new MongoDbOptions().WithConnectionString("name").WithCollectionName("name").Validate();
        actConfig.Should().ThrowExactly<MongoConfigurationException>();
    }

    [Fact]
    public void It_returns_custom_provider_name()
    {
        var result = new MongoDbOptions().WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName("name")
            .WithDatabaseName("db").WithCustomProviderName("MONGO!");
        result.ProviderName.Should().Be("MONGO!");
    }

    [Fact]
    public void It_returns_default_provider_name()
    {
        var result = new MongoDbOptions().WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName("name")
            .WithDatabaseName("db");
        result.ProviderName.Should().Be("MongoDb.db.name");
    }
}