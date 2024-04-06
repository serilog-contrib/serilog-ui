using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog.Ui.Core;
using Serilog.Ui.MongoDbProvider;
using Serilog.Ui.Web.Extensions;
using System;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.MongoDbProvider.Extensions;
using Xunit;

namespace MongoDb.Tests.Extensions
{
    [Trait("DI-DataProvider", "MongoDb")]
    public class SerilogUiOptionBuilderExtensionsTest
    {
        private readonly ServiceCollection _serviceCollection = [];

        [Fact]
        public void It_registers_provider_and_dependencies_with_connection_string_and_collection()
        {
            _serviceCollection.AddSerilogUi(builder =>
            {
                builder.UseMongoDb(options => options
                    .WithConnectionString("mongodb://mongodb0.example.com:27017/my-db")
                    .WithCollectionName("my-collection"));
            });
            var services = _serviceCollection.BuildServiceProvider();

            services.GetRequiredService<MongoDbOptions>().Should().NotBeNull();
            services.GetRequiredService<IDataProvider>().Should().NotBeNull().And.BeOfType<MongoDbDataProvider>();
            services.GetRequiredService<IMongoClient>().Should().NotBeNull();
        }

        [Fact]
        public void It_registers_provider_and_dependencies_with_connection_string_collection_and_dbname()
        {
            _serviceCollection.AddSerilogUi(builder =>
            {
                builder.UseMongoDb(options => options
                    .WithConnectionString("mongodb://mongodb0.example.com:27017/")
                    .WithDatabaseName("my-db")
                    .WithCollectionName("my-collection"));
            });
            var services = _serviceCollection.BuildServiceProvider();

            services.GetRequiredService<MongoDbOptions>().Should().NotBeNull();
            services.GetRequiredService<IDataProvider>().Should().NotBeNull().And.BeOfType<MongoDbDataProvider>();
            services.GetRequiredService<IMongoClient>().Should().NotBeNull();
            services.GetRequiredService<IMongoClient>().Settings.ApplicationName.Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public void It_registers_IMongoClient_only_when_not_registered()
        {
            _serviceCollection.AddSingleton<IMongoClient>(p =>
                new MongoClient(new MongoClientSettings { ApplicationName = "my-app" }));
            _serviceCollection.AddSerilogUi(builder =>
            {
                builder.UseMongoDb(options => options
                    .WithConnectionString("mongodb://mongodb0.example.com:27017/")
                    .WithDatabaseName("my-db")
                    .WithCollectionName("my-collection"));
            });
            var services = _serviceCollection.BuildServiceProvider();

            services.GetRequiredService<IMongoClient>().Settings.ApplicationName.Should().Be("my-app");
        }

        [Fact]
        public void It_throws_on_invalid_registration()
        {
            var act = () => _serviceCollection.AddSerilogUi(builder => builder.UseMongoDb(options =>
                options.WithConnectionString("mongodb://mongodb0.example.com0:27017").WithCollectionName("name")));
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}