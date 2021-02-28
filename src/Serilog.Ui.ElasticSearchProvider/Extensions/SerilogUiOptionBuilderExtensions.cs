using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.ElasticSearchProvider.Extensions
{
    public static class SerilogUiOptionBuilderExtensions
    {
        public static void UseElasticSearchDb(this SerilogUiOptionsBuilder optionsBuilder, Uri endpoint, string indexName)
        {
            if (endpoint == null)
                throw new ArgumentNullException(nameof(endpoint));

            if (string.IsNullOrEmpty(indexName))
                throw new ArgumentNullException(nameof(indexName));

            var options = new ElasticSearchDbOptions
            {
                IndexName = indexName
            };

            var builder = ((ISerilogUiOptionsBuilder)optionsBuilder);

            builder.Services.AddSingleton(options);

            var pool = new SingleNodeConnectionPool(endpoint);
            var connectionSettings = new ConnectionSettings(pool, sourceSerializer: (builtin, values) => new VanillaSerializer());

            builder.Services.AddSingleton<IElasticClient>(o => new ElasticClient(connectionSettings));
            builder.Services.AddScoped<IDataProvider, ElasticSearchDbDataProvider>();
        }
    }
}