using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using Elastic.Elasticsearch.Xunit;
using Nest;
using Elasticsearch.Net;
using Elastic.Elasticsearch.Ephemeral;
using Xunit;
using ElasticSearch.Tests.Util;

[assembly: TestFramework("Elastic.Elasticsearch.Xunit.Sdk.ElasticTestFramework", "Elastic.Elasticsearch.Xunit")]
[assembly: ElasticXunitConfiguration(typeof(SerilogSinkElasticsearchXunitRunOptions))]

namespace ElasticSearch.Tests.Util
{
    // test base and configs from:
    // https://github.com/serilog-contrib/serilog-sinks-elasticsearch/tree/dev/test/Serilog.Sinks.Elasticsearch.IntegrationTests/Bootstrap
    public abstract class Elasticsearch7XTestBase : IClusterFixture<Elasticsearch7XCluster>
    {
        protected Elasticsearch7XTestBase(Elasticsearch7XCluster cl) => Cluster = cl;

        protected Elasticsearch7XCluster Cluster { get; }

        public IElasticClient Client => this.CreateClient();

        protected virtual ConnectionSettings SetClusterSettings(ConnectionSettings s) => s;

        private IElasticClient CreateClient() =>
            Cluster.GetOrAddClient(c =>
            {
                var clusterNodes = c.NodesUris();
                var settings = new ConnectionSettings(new StaticConnectionPool(clusterNodes));
                settings = SetClusterSettings(settings);

                var current = (IConnectionConfigurationValues)settings;
                var notAlreadyAuthenticated = current.BasicAuthenticationCredentials == null && current.ClientCertificates == null;

                var config = Cluster.ClusterConfiguration;
                if (config.EnableSecurity && notAlreadyAuthenticated)
                    settings = settings.BasicAuthentication(ClusterAuthentication.Admin.Username, ClusterAuthentication.Admin.Password);

                var client = new ElasticClient(settings);
                return client;
            });
    }

    public class SerilogSinkElasticsearchXunitRunOptions : ElasticXunitRunOptions
    {
        public SerilogSinkElasticsearchXunitRunOptions()
        {
            RunIntegrationTests = true;
            RunUnitTests = true;
            ClusterFilter = null;
            TestFilter = null;
            IntegrationTestsMayUseAlreadyRunningNode = false;
        }
    }
}
