using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Ui.Common.Tests.DataSamples;
using Serilog.Ui.Common.Tests.TestSuites;
using Serilog.Ui.Core;
using Serilog.Ui.ElasticSearchProvider;
using System;
using System.Threading.Tasks;

namespace ElasticSearch.Tests.Util
{
    public class ElasticTestProvider : Elasticsearch7XTestBase, IIntegrationRunner
    {
        private readonly ElasticSearchDbDataProvider provider;
        private LogModelPropsCollector? logModelPropsCollector;
        private bool disposedValue;

        public ElasticTestProvider(Elasticsearch7XCluster cl) : base(cl)
        {
            provider = new ElasticSearchDbDataProvider(Client, new ElasticSearchDbOptions
            {
                IndexName = $"{SetupSerilog.IndexPrefix}{DateTime.UtcNow:yyyy.MM.dd}"
            });
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public IDataProvider GetDataProvider() => provider;

        public LogModelPropsCollector GetPropsCollector() => logModelPropsCollector!;

        public Task InitializeAsync()
        {
            logModelPropsCollector = Cluster.Collector;

            return Client.Indices.RefreshAsync(SetupSerilog.IndexPrefix + "*");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public sealed class SetupSerilog
    {
        public const string IndexPrefix = "logs-7x-default-";
        public const string TemplateName = "serilog-logs-7x";
        private readonly LoggerConfiguration loggerConfig;

        public SetupSerilog()
        {
            loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri($"http://localhost:9200"))
                    {
                        IndexFormat = IndexPrefix + "{0:yyyy.MM.dd}",
                        TemplateName = TemplateName,
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    });
        }

        public LogModelPropsCollector InitializeLogs()
        {
            using var logger = loggerConfig.CreateLogger();
            return ElasticSearchLogModelFaker.Logs(logger);
        }
    }
}