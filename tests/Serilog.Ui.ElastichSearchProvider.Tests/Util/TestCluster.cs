using Elastic.Elasticsearch.Ephemeral.Plugins;
using Elastic.Elasticsearch.Ephemeral;
using Elastic.Elasticsearch.Xunit;
using Elastic.Stack.ArtifactsApi.Products;
using Serilog.Ui.Common.Tests.DataSamples;

namespace ElasticSearch.Tests.Util
{
    // test cluster configurations from:
    // https://github.com/serilog-contrib/serilog-sinks-elasticsearch/blob/dev/test/Serilog.Sinks.Elasticsearch.IntegrationTests/Bootstrap/ClientTestClusterBase.cs
    public class Elasticsearch7XCluster : TestCluster
    {
        public LogModelPropsCollector? Collector;
        public Elasticsearch7XCluster() : base(CreateConfiguration())
        {
        }

        private static ClientTestClusterConfiguration CreateConfiguration()
        {
            return new ClientTestClusterConfiguration("7.8.0")
            {
                MaxConcurrency = 1
            };
        }

        protected override void SeedCluster() { }

        protected override void OnAfterStarted()
        {
            var serilog = new SetupSerilog();
            Collector = serilog.InitializeLogs();
        }
    }

    public abstract class TestCluster : XunitClusterBase<ClientTestClusterConfiguration>
    {
        protected TestCluster(ClientTestClusterConfiguration configuration) : base(configuration) { }
    }

    public class ClientTestClusterConfiguration : XunitClusterConfiguration
    {
        public ClientTestClusterConfiguration(
            string elasticsearchVersion,
            ClusterFeatures features = ClusterFeatures.None,
            int numberOfNodes = 1,
            params ElasticsearchPlugin[] plugins
        )
            : base(elasticsearchVersion, features, new ElasticsearchPlugins(plugins), numberOfNodes)
        {
            HttpFiddlerAware = true;
            CacheEsHomeInstallation = true;

            Add(AttributeKey("testingcluster"), "true");

            Add($"script.max_compilations_per_minute", "10000", "<6.0.0-rc1");
            Add($"script.max_compilations_rate", "10000/1m", ">=6.0.0-rc1");

            Add($"script.inline", "true", "<5.5.0");
            Add($"script.stored", "true", ">5.0.0-alpha1 <5.5.0");
            Add($"script.indexed", "true", "<5.0.0-alpha1");
            Add($"script.allowed_types", "inline,stored", ">=5.5.0");
        }
    }
}
