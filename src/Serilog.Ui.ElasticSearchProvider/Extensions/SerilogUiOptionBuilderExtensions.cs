using System;
using System.Linq;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.ElasticSearchProvider.Serializers;

namespace Serilog.Ui.ElasticSearchProvider.Extensions
{
    /// <summary>
    /// ElasticSearch data provider specific extension methods for <see cref="ISerilogUiOptionsBuilder"/>.
    /// It uses <see cref="Nest"/> to query data.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        /// Configures the SerilogUi to connect to a Elastic Search database.
        /// </summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="setupOptions">The ElasticSearch options action.</param>
        /// <exception cref="ArgumentNullException"> throw if endpoint is null </exception>
        /// <exception cref="ArgumentNullException"> throw is indexName is null </exception>
        public static ISerilogUiOptionsBuilder UseElasticSearchDb(this ISerilogUiOptionsBuilder optionsBuilder, Action<ElasticSearchDbOptions> setupOptions)
        {
            var options = new ElasticSearchDbOptions();
            setupOptions.Invoke(options);
            options.Validate();

            // sorting by property is disabled for Elastic Search
            optionsBuilder.RegisterDisabledSortForProviderKey(options.ProviderName);
            
            var pool = new SingleNodeConnectionPool(options.Endpoint);
            var connectionSettings = new ConnectionSettings(pool, sourceSerializer: (_, _) => new VanillaSerializer());

            optionsBuilder.Services.AddSingleton<IElasticClient>(new ElasticClient(connectionSettings));

            optionsBuilder.Services.AddScoped<IDataProvider, ElasticSearchDbDataProvider>(sp => 
                new ElasticSearchDbDataProvider(sp.GetRequiredService<IElasticClient>(), options));

            return optionsBuilder;
        }
    }
}