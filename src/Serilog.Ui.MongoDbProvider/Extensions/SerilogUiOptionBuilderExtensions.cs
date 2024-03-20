using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Serilog.Ui.Core;

namespace Serilog.Ui.MongoDbProvider.Extensions
{
    /// <summary>
    ///  MongoDB data provider specific extension methods for <see cref="ISerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        ///  Configures the SerilogUi to connect to a MongoDB database.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="setupOptions">The MongoDb options action.</param>
        public static void UseMongoDb(
            this ISerilogUiOptionsBuilder optionsBuilder,
            Action<MongoDbOptions> setupOptions)
        {
            var dbOptions = new MongoDbOptions();
            setupOptions(dbOptions);
            dbOptions.Validate();

            optionsBuilder.Services.AddSingleton(dbOptions);
            optionsBuilder.Services.TryAddSingleton<IMongoClient>(o => new MongoClient(dbOptions.ConnectionString));

            // TODO Fix up MongoDB to allow multiple registrations. Think about multiple MongoDB clients
            // (singletons) used in data providers (scoped)
            if (optionsBuilder.Services.Any(c => c.ImplementationType == typeof(MongoDbDataProvider)))
                throw new NotSupportedException($"Adding multiple registrations of '{typeof(MongoDbDataProvider).FullName}' is not (yet) supported.");
            optionsBuilder.Services.AddScoped<IDataProvider, MongoDbDataProvider>();
        }
    }
}