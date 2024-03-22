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
        private readonly ElasticSearchDbDataProvider _provider;

        private LogModelPropsCollector? _logModelPropsCollector;

        private bool _disposedValue;

        public ElasticTestProvider(Elasticsearch7XCluster cl) : base(cl)
        {
            _provider = new ElasticSearchDbDataProvider(Client, new ElasticSearchDbOptions
            {
                IndexName = $"{Elasticsearch7XCluster.IndexPrefix}{DateTime.UtcNow:yyyy.MM.dd}"
            });
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public IDataProvider GetDataProvider() => _provider;

        public LogModelPropsCollector GetPropsCollector() => _logModelPropsCollector!;

        public Task InitializeAsync()
        {
            _logModelPropsCollector = Cluster.Collector;

            return Client.Indices.RefreshAsync(Elasticsearch7XCluster.IndexPrefix + "*");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                // dispose managed state (managed objects)
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}