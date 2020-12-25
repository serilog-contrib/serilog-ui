using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.MongoDbProvider
{
    public static class SerilogUiOptionBuilderExtensions
    {
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