using System;
using System.Linq;
using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using ElasticSearch.Tests.Util;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.ElasticSearchProvider;
using Serilog.Ui.ElasticSearchProvider.Extensions;
using Serilog.Ui.Web.Extensions;
using Xunit;

namespace ElasticSearch.Tests.Extensions;

[Trait("DI-DataProvider", "Elastic")]
public class SerilogUiOptionBuilderExtensionsTest : IClusterFixture<Elasticsearch7XCluster>
{
    private readonly ServiceCollection _serviceCollection = [];

    [U]
    public void It_registers_provider_and_dependencies()
    {
        _serviceCollection.AddSerilogUi(builder =>
        {
            builder.UseElasticSearchDb(options => options.WithEndpoint(new Uri("https://elastic.example.com")).WithIndex("my-index"));
        });
        var services = _serviceCollection.BuildServiceProvider();

        services.GetRequiredService<IDataProvider>().Should().NotBeNull().And.BeOfType<ElasticSearchDbDataProvider>();

        services.GetRequiredService<ProvidersOptions>().DisabledSortProviderNames.Should().HaveCount(1);
    }

    [Fact]
    public void It_registers_multiple_providers()
    {
        _serviceCollection.AddSerilogUi(builder =>
        {
            builder
                .UseElasticSearchDb(options => options.WithEndpoint(new Uri("https://elastic.com")).WithIndex("my-index"))
                .UseElasticSearchDb(options => options.WithEndpoint(new Uri("https://elastic.com")).WithIndex("my-index-2"));
        });

        var serviceProvider = _serviceCollection.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var providers = scope.ServiceProvider.GetServices<IDataProvider>().ToList();
        providers.Should().HaveCount(2).And.AllBeOfType<ElasticSearchDbDataProvider>();
        providers.Select(p => p.Name).Should().OnlyHaveUniqueItems();
        serviceProvider.GetRequiredService<ProvidersOptions>().DisabledSortProviderNames.Should().HaveCount(2);
    }

    [U]
    public void It_throws_on_invalid_registration()
    {
        var uri = new Uri("https://elastic.example.com");
        var nullable = () => _serviceCollection.AddSerilogUi(builder => builder.UseElasticSearchDb(options => options.WithEndpoint(uri)));

        nullable.Should().ThrowExactly<ArgumentNullException>();
    }
}