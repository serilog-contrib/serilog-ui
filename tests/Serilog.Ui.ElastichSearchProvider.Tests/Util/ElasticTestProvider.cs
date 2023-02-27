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
        private bool disposedValue;

        public ElasticTestProvider(Elasticsearch7XCluster cluster) : base(cluster)
        {
            provider = new ElasticSearchDbDataProvider(Client, new ElasticSearchDbOptions
            {
                IndexName = $"{SetupSerilog.IndexPrefix}{DateTime.Now:yyyy.MM.dd}"
            });
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public IDataProvider GetDataProvider() => provider;

        public LogModelPropsCollector GetPropsCollector() => new(Array.Empty<LogModel>());

        public Task InitializeAsync()
        {
            var serilog = new SetupSerilog();
            return serilog.InitializeLogsAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    // TODO: dispose managed state (managed objects)
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

    internal sealed class SetupSerilog
    {
        public const string IndexPrefix = "logs-7x-default-";
        public const string TemplateName = "serilog-logs-7x";
        private readonly LoggerConfiguration loggerConfig;

        public SetupSerilog()
        {
            loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri($"http://localhost:9200"))
                    {
                        IndexFormat = IndexPrefix + "{0:yyyy.MM.dd}",
                        TemplateName = TemplateName,
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    });
        }

        public Task InitializeLogsAsync()
        {
            using var logger = loggerConfig.CreateLogger();
            logger.Information("Hello Information");
            logger.Debug("Hello Debug");
            logger.Warning("Hello Warning");
            logger.Error("Hello Error");
            logger.Fatal("Hello Fatal");

            return Task.Delay(2000);
        }
    }
}