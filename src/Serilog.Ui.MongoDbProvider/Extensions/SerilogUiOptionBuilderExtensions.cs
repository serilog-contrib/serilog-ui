using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.MongoDbProvider
{
    /// <summary>
    /// MongoDB data provider specific extension methods for <see cref="SerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        /// Configures the SerilogUi to connect to a MongoDB database.
        /// If IMongoClient isn't registered in the DI, it will register a IMongoClient Singleton.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="collectionName">Name of the collection name.</param>
        /// <exception cref="ArgumentNullException">throw if connectionString is null</exception>
        /// <exception cref="ArgumentNullException">throw is collectionName is null</exception>
        public static void UseMongoDb(
            this SerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string collectionName
        )
            => optionsBuilder.Register(new MongoDbOptions
            {
                ConnectionString = connectionString,
                DatabaseName = MongoUrl.Create(connectionString).DatabaseName,
                CollectionName = collectionName
            });

        /// <summary>
        /// Configures the SerilogUi to connect to a MongoDB database.
        /// If IMongoClient isn't registered in the DI, it will register a IMongoClient Singleton.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="databaseName">Name of the data table.</param>
        /// <param name="collectionName">Name of the collection name.</param>
        /// <exception cref="ArgumentNullException">throw if connectionString is null</exception>
        /// <exception cref="ArgumentNullException">throw is databaseName is null</exception>
        /// <exception cref="ArgumentNullException">throw is collectionName is null</exception>
        public static void UseMongoDb(
            this SerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string databaseName,
            string collectionName
        )
            => optionsBuilder.Register(new MongoDbOptions
            {
                ConnectionString = connectionString,
                DatabaseName = databaseName,
                CollectionName = collectionName
            });

        /// <summary>
        /// Configures the SerilogUi to connect to a MongoDB database.
        /// If IMongoClient isn't registered in the DI, it will register a IMongoClient Singleton.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="databaseName">Name of the data table.</param>
        /// <param name="collectionName">Name of the collection name.</param>
        /// <param name="useLinq3">Use Linq3 provider in MongoClient registration.</param>
        /// <exception cref="ArgumentNullException">throw if connectionString is null</exception>
        /// <exception cref="ArgumentNullException">throw is databaseName is null</exception>
        /// <exception cref="ArgumentNullException">throw is collectionName is null</exception>
        public static void UseMongoDb(
            this SerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string databaseName,
            string collectionName,
            bool useLinq3
        )
            => optionsBuilder.Register(new MongoDbOptions
            {
                ConnectionString = connectionString,
                DatabaseName = databaseName,
                CollectionName = collectionName,
                UseLinq3 = useLinq3
            });

        /// <summary>
        /// Configures the SerilogUi to connect to a MongoDB database.
        /// If IMongoClient isn't registered in the DI, it will register a IMongoClient Singleton.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="mongoDbOptions">The provider options.</param>
        /// <exception cref="ArgumentNullException">throw if connectionString is null</exception>
        /// <exception cref="ArgumentNullException">throw is databaseName is null</exception>
        /// <exception cref="ArgumentNullException">throw is collectionName is null</exception>
        public static void UseMongoDb(
            this SerilogUiOptionsBuilder optionsBuilder,
            MongoDbOptions mongoDbOptions
        )
            => optionsBuilder.Register(mongoDbOptions);

        private static void Register(this SerilogUiOptionsBuilder optionsBuilder, MongoDbOptions providerOptions)
        {
            Guard.Against.NullOrWhiteSpace(providerOptions.ConnectionString, nameof(providerOptions.ConnectionString));
            Guard.Against.NullOrWhiteSpace(providerOptions.DatabaseName, nameof(providerOptions.DatabaseName));
            Guard.Against.NullOrWhiteSpace(providerOptions.CollectionName, nameof(providerOptions.CollectionName));

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton(providerOptions);
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.TryAddSingleton<IMongoClient>(o =>
            {
                var client = new MongoClient(providerOptions.ConnectionString);
                if (providerOptions.UseLinq3) { client.Settings.LinqProvider = LinqProvider.V3; }
                return client;
            });
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddScoped<IDataProvider, MongoDbDataProvider>();
        }
    }
}