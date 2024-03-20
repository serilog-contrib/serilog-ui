using System;
using Ardalis.GuardClauses;
using MongoDB.Driver;
using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.MongoDbProvider
{
    public class MongoDbOptions : BaseDbOptions
    {
        /// <summary>
        /// Optional parameter. It must be set, if the database name is not found in the connection string.
        /// </summary>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// Required parameter.
        /// </summary>
        public string CollectionName { get; private set; }

        /// <summary>
        /// Throws if ConnectionString, CollectionName is null or whitespace.
        /// Throws if DatabaseName is null or whitespace and is not found in the connection string.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public override void Validate()
        {
            Guard.Against.NullOrWhiteSpace(CollectionName);
            base.Validate();

            if (string.IsNullOrWhiteSpace(DatabaseName))
            {
                DatabaseName = MongoUrl.Create(ConnectionString).DatabaseName;
            }

            Guard.Against.NullOrWhiteSpace(DatabaseName);
        }

        /// <summary>
        /// Fluently sets DatabaseName.
        /// </summary>
        /// <param name="databaseName"></param>
        public MongoDbOptions WithDatabaseName(string databaseName)
        {
            DatabaseName = databaseName;
            return this;
        }

        /// <summary>
        /// Fluently sets CollectionName.
        /// </summary>
        /// <param name="collection"></param>
        public MongoDbOptions WithCollectionName(string collection)
        {
            CollectionName = collection;
            return this;
        }
    }
}