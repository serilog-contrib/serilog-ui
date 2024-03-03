using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Serilog.Ui.Core;
using System;
using System.Linq;

namespace Serilog.Ui.ElasticSearchProvider
{
    /// <summary>
    /// ElasticSearch data provider specific extension methods for <see cref="SerilogUiOptionsBuilder"/>.
    /// It uses <see cref="Nest"/> to query data.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        /// Configures the SerilogUi to connect to a Elastic Search database.
        /// </summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="endpoint"> The url of ElasticSearch server. </param>
        /// <param name="indexName"> Name of the log index. </param>
        /// <exception cref="ArgumentNullException"> throw if endpoint is null </exception>
        /// <exception cref="ArgumentNullException"> throw is indexName is null </exception>
        public static void UseElasticSearchDb(this SerilogUiOptionsBuilder optionsBuilder, Uri endpoint, string indexName)
        {
            if (endpoint == null)
                throw new ArgumentNullException(nameof(endpoint));

            if (string.IsNullOrWhiteSpace(indexName))
                throw new ArgumentNullException(nameof(indexName));

            var options = new ElasticSearchDbOptions
            {
                IndexName = indexName
            };

            var builder = ((ISerilogUiOptionsBuilder)optionsBuilder);

            // TODO: Fixup ES to allow multiple registrations.
            // Think about multiple ES clients (singletons) used in data providers (scoped)
            if (builder.Services.Any(c => c.ImplementationType == typeof(ElasticSearchDbDataProvider)))
                throw new NotSupportedException(
                    $"Adding multiple registrations of '{typeof(ElasticSearchDbDataProvider).FullName}' is not (yet) supported.");

            builder.Services.AddSingleton(options);

            var pool = new SingleNodeConnectionPool(endpoint);
            var connectionSettings = new ConnectionSettings(pool, sourceSerializer: (_, _) => new VanillaSerializer());

            builder.Services.AddSingleton<IElasticClient>(o => new ElasticClient(connectionSettings));
            builder.Services.AddScoped<IDataProvider, ElasticSearchDbDataProvider>();
        }
    }
}