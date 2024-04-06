using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Serilog.Ui.Core;
using Serilog.Ui.RavenDbProvider;
using Serilog.Ui.RavenDbProvider.Extensions;
using Serilog.Ui.Web.Extensions;

namespace RavenDb.Tests.Extensions;

[Trait("DI-DataProvider", "RavenDb")]
public class SerilogUiOptionBuilderExtensionsTest
{
    private readonly ServiceCollection _serviceCollection = [];

    [Fact]
    public void It_registers_provider_and_dependencies_with_documentStore()
    {
        // Arrange
        const string dbName = "test";

        _serviceCollection.AddSerilogUi(builder =>
        {
            IDocumentStore documentStore = new DocumentStore { Urls = ["http://localhost:8080"], Database = dbName };
            builder.UseRavenDb(opt => opt.WithDocumentStore(documentStore));
        });

        var services = _serviceCollection.BuildServiceProvider();
        // Act

        var dataProvider = services.GetRequiredService<IDataProvider>();
        var documentStore = services.GetRequiredService<IDocumentStore>();

        // Assert
        dataProvider.Should().NotBeNull().And.BeOfType<RavenDbDataProvider>();
        documentStore.Should().NotBeNull();
        documentStore.Urls.Should().NotBeNullOrEmpty();
        documentStore.Database.Should().Be(dbName);
    }
    
    [Fact]
    public void It_registers_multiple_providers()
    {
        _serviceCollection.AddSerilogUi(builder =>
        {
            IDocumentStore documentStore = new DocumentStore { Urls = ["http://localhost:8080"], Database = "test-db" };
            builder
                .UseRavenDb(opt => opt.WithDocumentStore(documentStore)) // default collection name
                .UseRavenDb(opt => opt.WithDocumentStore(documentStore).WithCollectionName("test"));
        });

        var serviceProvider = _serviceCollection.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var providers = scope.ServiceProvider.GetServices<IDataProvider>().ToList();
        providers.Should().HaveCount(2).And.AllBeOfType<RavenDbDataProvider>();
        providers.Select(p => p.Name).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void It_throws_on_invalid_registration()
    {
        // Act
        var nullables = new List<Func<IServiceCollection>>
        {
            () => _serviceCollection.AddSerilogUi(builder => builder.UseRavenDb(_ => { })),
            () => _serviceCollection.AddSerilogUi(
                builder => builder.UseRavenDb(opt => opt.WithDocumentStore(new DocumentStore { Urls = null!, Database = "Test" }))),
            () => _serviceCollection.AddSerilogUi(
                builder => builder.UseRavenDb(opt => opt.WithDocumentStore(new DocumentStore { Urls = [], Database = "Test" }))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseRavenDb(opt => opt.WithDocumentStore(new DocumentStore { Urls = ["http://localhost:8080"], Database = null! }))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseRavenDb(opt => opt.WithDocumentStore(new DocumentStore { Urls = ["http://localhost:8080"], Database = " " }))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseRavenDb(opt => opt.WithDocumentStore(new DocumentStore { Urls = ["http://localhost:8080"], Database = string.Empty }))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseRavenDb(opt => opt
                    .WithDocumentStore(new DocumentStore { Urls = ["http://localhost:8080"], Database = "db" })
                    .WithCollectionName(null!))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseRavenDb(opt => opt
                    .WithDocumentStore(new DocumentStore { Urls = ["http://localhost:8080"], Database = "db" })
                    .WithCollectionName(" "))),
            () => _serviceCollection.AddSerilogUi(builder =>
                builder.UseRavenDb(opt => opt
                    .WithDocumentStore(new DocumentStore { Urls = ["http://localhost:8080"], Database = "db" })
                    .WithCollectionName(string.Empty)))
        };

        // Assert
        foreach (var nullable in nullables)
        {
            nullable.Should().Throw<ArgumentException>();
        }
    }
}