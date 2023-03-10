using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Serilog.Ui.Core;
using System;
using System.Linq;

namespace Serilog.Ui.MongoDbProvider
{
    /// <summary>
    /// MongoDB data provider specific extension methods for <see cref="SerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        /// Configures the SerilogUi to connect to a MongoDB database.
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

            var builder = ((ISerilogUiOptionsBuilder)optionsBuilder);

            // TODO Fixup MongoDb to allow multiple registrations.
            //      Think about multiple ES clients (singletons) used in data providers (scoped)
            if (builder.Services.Any(c => c.ImplementationType == typeof(MongoDbDataProvider)))
                throw new NotSupportedException($"Adding multiple registrations of '{typeof(MongoDbDataProvider).FullName}' is not (yet) supported.");

            builder.Services.AddSingleton(mongoProvider);
            builder.Services.TryAddSingleton<IMongoClient>(o => new MongoClient(connectionString));
            builder.Services.AddScoped<IDataProvider, MongoDbDataProvider>();
        }

        /// <summary>
        /// Configures the SerilogUi to connect to a MongoDB database.
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

            var builder = ((ISerilogUiOptionsBuilder)optionsBuilder);

            // TODO Fixup MongoDb to allow multiple registrations.
            //      Think about multiple ES clients (singletons) used in data providers (scoped)
            if (builder.Services.Any(c => c.ImplementationType == typeof(MongoDbDataProvider)))
                throw new NotSupportedException($"Adding multiple registrations of '{typeof(MongoDbDataProvider).FullName}' is not (yet) supported.");

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton(mongoProvider);
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton<IMongoClient>(o => new MongoClient(connectionString));
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddScoped<IDataProvider, MongoDbDataProvider>();
        }
    }
}