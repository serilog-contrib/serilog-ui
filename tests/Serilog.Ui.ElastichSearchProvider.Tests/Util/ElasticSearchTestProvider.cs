using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using Serilog.Ui.Common.Tests.SqlUtil;
using Serilog.Ui.ElasticSearchProvider;
using System;
using System.Threading.Tasks;

namespace ElasticSearch.Tests.Util
{
    public sealed class ElasticSearchTestProvider : DatabaseInstance<ElasticsearchTestcontainer, ElasticsearchTestcontainerConfiguration>
    {
        protected override string Name => nameof(ElasticsearchTestcontainer);

        private ElasticSearchDbOptions DbOptions { get; set; } = new()
        {
            IndexName = "logstash-2022.08.16"
        };

        public IElasticClient Client { get; set; }

        protected override async Task CheckDbReadinessAsync()
        {
            Client = new ElasticClient(new Uri(Container?.ConnectionString ?? string.Empty));

            var result = await Client.Cluster.HealthAsync();
            var result2 = await Client.Cluster.RemoteInfoAsync().ConfigureAwait(false);
            if (result.Status == Elasticsearch.Net.Health.Red) throw new AccessViolationException();
        }

        protected override async Task InitializeAdditionalAsync()
        {
            var service = new ServiceCollection();
            var e = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", Microsoft.Extensions.Logging.LogLevel.Warning)
                    .AddFilter("System", Microsoft.Extensions.Logging.LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", Microsoft.Extensions.Logging.LogLevel.Debug);
            });
            service.AddSingleton(e);
            //var loggerFactory = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(Container!.ConnectionString)));
            //var p = loggerFactory.CreateLogger();
            //service.AddSingleton<ILogger>(p);
            //service.AddLogging();
            //var pro = service.BuildServiceProvider();
            //var ea = pro.GetRequiredService<ILogger>();
            ////var ee = ea.CreateLogger<ElasticSearchTestProvider>();
            //ea.Debug("dd");
            // await Client.BulkAsync<LogModel>();

            //await dataContext.ExecuteAsync(Costants.MySqlInsertFakeData, LogModelFaker.Logs());

            Provider = new ElasticSearchDbDataProvider(Client, DbOptions);
        }

    }
}
