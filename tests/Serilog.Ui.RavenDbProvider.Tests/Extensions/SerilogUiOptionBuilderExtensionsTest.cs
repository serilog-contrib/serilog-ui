using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Serilog.Ui.Core;
using Serilog.Ui.RavenDbProvider;
using Serilog.Ui.RavenDbProvider.Extensions;
using Serilog.Ui.Web;

namespace RavenDb.Tests.Extensions;

[Trait("DI-DataProvider", "RavenDb")]
public class SerilogUiOptionBuilderExtensionsTest
{
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();

    [Fact]
    public void It_registers_provider_and_dependencies_with_documentStore()
    {
        // Arrange
        const string dbName = "test";

        _serviceCollection.AddSerilogUi((builder) =>
        {
            IDocumentStore documentStore = new DocumentStore { Urls = new[] { "http://localhost:8080" }, Database = dbName };
            builder.UseRavenDb(documentStore);
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
    public void It_throws_on_invalid_registration()
    {
        // Act
        var act = () => _serviceCollection.AddSerilogUi(builder => builder.UseRavenDb(null));
        var act2 = () => _serviceCollection.AddSerilogUi(builder => builder.UseRavenDb(new DocumentStore() { Database = "Test" }));
        var act3 = () => _serviceCollection.AddSerilogUi(builder => builder.UseRavenDb(new DocumentStore { Urls = new[] { "http://localhost:8080" } }));
        var act4 = () => _serviceCollection.AddSerilogUi(builder => builder.UseRavenDb(new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = "Test"
        }, null));

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
        act2.Should().ThrowExactly<ArgumentException>();
        act3.Should().ThrowExactly<ArgumentNullException>();
        act4.Should().ThrowExactly<ArgumentNullException>();
    }
}