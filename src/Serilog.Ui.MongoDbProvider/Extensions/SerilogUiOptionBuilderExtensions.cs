using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Interfaces;

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
        public static ISerilogUiOptionsBuilder UseMongoDb(
            this ISerilogUiOptionsBuilder optionsBuilder,
            Action<MongoDbOptions> setupOptions)
        {
            var dbOptions = new MongoDbOptions();
            setupOptions(dbOptions);
            dbOptions.Validate();

            optionsBuilder.Services.TryAddSingleton<IMongoClient>(o => new MongoClient(dbOptions.ConnectionString));
            
            optionsBuilder.Services.AddScoped<IDataProvider>(sp => new MongoDbDataProvider(sp.GetRequiredService<IMongoClient>(), dbOptions));

            return optionsBuilder;
        }
    }
}