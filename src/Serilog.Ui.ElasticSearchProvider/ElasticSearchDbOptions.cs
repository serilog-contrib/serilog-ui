using System;
using Ardalis.GuardClauses;
using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.ElasticSearchProvider
{
    public class ElasticSearchDbOptions : IDbOptions
    {
        public string IndexName { get; private set; }

        public Uri Endpoint { get; private set; }

        public void Validate()
        {
            Guard.Against.Null(Endpoint);
            Guard.Against.NullOrWhiteSpace(IndexName);
        }
        
        /// <summary>
        /// Fluently sets IndexName.
        /// </summary>
        /// <param name="indexName"></param>
        public ElasticSearchDbOptions WithIndex(string indexName)
        {
            IndexName = indexName;
            return this;
        }

        /// <summary>
        /// Fluently sets Endpoint.
        /// </summary>
        /// <param name="endpoint"></param>
        public ElasticSearchDbOptions WithEndpoint(Uri endpoint)
        {
            Endpoint = endpoint;
            return this;
        }
    }
}