using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using Elastic.Elasticsearch.Xunit;
using Nest;
using Elasticsearch.Net;
using Elastic.Elasticsearch.Ephemeral;

namespace ElasticSearch.Tests.Util
{
    // from https://github.com/serilog-contrib/serilog-sinks-elasticsearch/blob/dev/test/Serilog.Sinks.Elasticsearch.IntegrationTests/Elasticsearch7/Bootstrap/Elasticsearch7XTestBase.cs
    public abstract class Elasticsearch7XTestBase : IClusterFixture<Elasticsearch7XCluster>
    {
        protected Elasticsearch7XTestBase(Elasticsearch7XCluster cluster) => Cluster = cluster;

        private Elasticsearch7XCluster Cluster { get; }

        protected IElasticClient Client => this.CreateClient();

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
}
