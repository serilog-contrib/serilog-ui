using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog.Ui.Core;
using Serilog.Ui.Web;
using System;
using System.Collections.Generic;
using Xunit;

namespace Serilog.Ui.MongoDbProvider.Tests.Extensions
{
    public class SerilogUiOptionBuilderExtensionsTest
    {
        private readonly ServiceCollection serviceCollection;

        public SerilogUiOptionBuilderExtensionsTest()
        {
            serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void It_registers_provider_dependencies_with_connstring_and_collection()
        {
            serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseMongoDb("mongodb://mongodb0.example.com:27017/my-db", "my-collection");
            });
            var services = serviceCollection.BuildServiceProvider();

            services.GetRequiredService<MongoDbOptions>().Should().NotBeNull();
            services.GetRequiredService<IDataProvider>().Should().NotBeNull().And.BeOfType<MongoDbDataProvider>();
            services.GetRequiredService<IMongoClient>().Should().NotBeNull();
        }

        [Fact]
        public void It_registers_provider_dependencies_with_connstring_collection_and_dbname()
        {
            serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseMongoDb("mongodb://mongodb0.example.com:27017", "my-db", "my-collection");
            });
            var services = serviceCollection.BuildServiceProvider();

            services.GetRequiredService<MongoDbOptions>().Should().NotBeNull();
            services.GetRequiredService<IDataProvider>().Should().NotBeNull().And.BeOfType<MongoDbDataProvider>();
            services.GetRequiredService<IMongoClient>().Should().NotBeNull();
            services.GetRequiredService<IMongoClient>().Settings.ApplicationName.Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public void It_registers_IMongoClient_only_when_not_registered()
        {
            serviceCollection.AddSingleton<IMongoClient>(p =>
                new MongoClient(new MongoClientSettings { ApplicationName = "my-app" }));
            serviceCollection.AddSerilogUi((builder) =>
            {
                builder.UseMongoDb("mongodb://mongodb0.example.com:27017", "my-db", "my-collection");
            });
            var services = serviceCollection.BuildServiceProvider();

            services.GetRequiredService<IMongoClient>().Settings.ApplicationName.Should().Be("my-app");
        }

        [Fact]
        public void It_throws_with_invalid_deps()
        {
            var nullables = new List<Func<IServiceCollection>>
            {
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb(null, "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb(" ", "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb("", "name")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb("name", null)),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb("name", " ")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb("name", "")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb("name", "name", null)),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb("name", "name", "")),
                () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb("name", "name", " ")),
            };

            foreach (var nullable in nullables)
            {
                nullable.Should().ThrowExactly<ArgumentNullException>();
            }

            var act = () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb("mongodb://mongodb0.example.com:27017", "name"));
            act.Should().ThrowExactly<ArgumentException>();

            var actConfig = () => serviceCollection.AddSerilogUi((builder) => builder.UseMongoDb("name", "name"));
            actConfig.Should().ThrowExactly<MongoConfigurationException>();
        }
    }
}
