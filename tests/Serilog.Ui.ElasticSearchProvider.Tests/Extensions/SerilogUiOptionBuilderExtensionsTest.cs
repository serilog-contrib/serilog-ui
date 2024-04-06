using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using ElasticSearch.Tests.Util;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.ElasticSearchProvider;
using Serilog.Ui.Web.Extensions;
using System;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.ElasticSearchProvider.Extensions;
using Xunit;

namespace ElasticSearch.Tests.Extensions
{
    [Trait("DI-DataProvider", "Elastic")]
    public class SerilogUiOptionBuilderExtensionsTest : IClusterFixture<Elasticsearch7XCluster>
    {
        private readonly ServiceCollection _serviceCollection = [];

        [U]
        public void It_registers_provider_and_dependencies()
        {
            _serviceCollection.AddSerilogUi(builder =>
            {
                builder.UseElasticSearchDb(options => options
                    .WithEndpoint(new Uri("https://elastic.example.com"))
                    .WithIndex("my-index"));
            });
            var services = _serviceCollection.BuildServiceProvider();

            services.GetRequiredService<IDataProvider>().Should().NotBeNull().And.BeOfType<ElasticSearchDbDataProvider>();
            var options = services.GetRequiredService<ElasticSearchDbOptions>();
            options.Should().NotBeNull();
            options.IndexName.Should().Be("my-index");

            services.GetRequiredService<ProvidersOptions>().DisabledSortProviderNames.Should().Contain(options.ProviderName);
        }

        [U]
        public void It_throws_on_invalid_registration()
        {
            var uri = new Uri("https://elastic.example.com");
            var nullable = () => _serviceCollection.AddSerilogUi(builder => builder.UseElasticSearchDb(options => options.WithEndpoint(uri)));

            nullable.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}