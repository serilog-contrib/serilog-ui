using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.MongoDbProvider
{
    /// <summary>
    /// SerilogUI option builder extensions.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        /// Adds MongoDB log data provider.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <exception cref="ArgumentNullException">connectionString</exception>
        /// <exception cref="ArgumentNullException">collectionName</exception>
        public static void UseMongoDb(
            this SerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string collectionName
        )
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentNullException(nameof(collectionName));

            var mongoProvider = new MongoDbOptions
            {
                ConnectionString = connectionString,
                DatabaseName = MongoUrl.Create(connectionString).DatabaseName,
                CollectionName = collectionName
            };

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton(mongoProvider);
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton<IMongoClient>(o => new MongoClient(connectionString));
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddScoped<IDataProvider, MongoDbDataProvider>();
        }

        /// <summary>
        /// Adds MongoDB log data provider.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="connectionString">The connection string without database name.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <exception cref="ArgumentNullException">connectionString</exception>
        /// <exception cref="ArgumentNullException">databaseName</exception>
        /// <exception cref="ArgumentNullException">collectionName</exception>
        public static void UseMongoDb(
            this SerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string databaseName,
            string collectionName
        )
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentNullException(nameof(databaseName));

            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentNullException(nameof(collectionName));

            var mongoProvider = new MongoDbOptions
            {
                ConnectionString = connectionString,
                DatabaseName = databaseName,
                CollectionName = collectionName
            };

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton(mongoProvider);
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton<IMongoClient>(o => new MongoClient(connectionString));
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddScoped<IDataProvider, MongoDbDataProvider>();
        }
    }
}