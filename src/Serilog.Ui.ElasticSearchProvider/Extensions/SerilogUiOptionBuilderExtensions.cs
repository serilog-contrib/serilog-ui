using System;
using System.Linq;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Serilog.Ui.Core;
using Serilog.Ui.Core.OptionsBuilder;

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

            optionsBuilder.Services.AddSingleton(options);
            // sorting by property is disabled for Elastic Search
            ProvidersOptions.RegisterDisabledSortName(options.ProviderName);

            var pool = new SingleNodeConnectionPool(options.Endpoint);
            var connectionSettings = new ConnectionSettings(pool, sourceSerializer: (_, _) => new VanillaSerializer());

            optionsBuilder.Services.AddSingleton<IElasticClient>(o => new ElasticClient(connectionSettings));

            // TODO: Fixup ES to allow multiple registrations.
            // Think about multiple ES clients (singletons) used in data providers (scoped)
            if (optionsBuilder.Services.Any(c => c.ImplementationType == typeof(ElasticSearchDbDataProvider)))
                throw new NotSupportedException(
                    $"Adding multiple registrations of '{typeof(ElasticSearchDbDataProvider).FullName}' is not (yet) supported.");
            
            optionsBuilder.Services.AddScoped<IDataProvider, ElasticSearchDbDataProvider>();

            return optionsBuilder;
        }
    }
}