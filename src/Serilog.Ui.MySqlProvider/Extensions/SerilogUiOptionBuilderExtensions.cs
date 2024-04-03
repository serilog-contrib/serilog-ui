using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.MySqlProvider.Extensions
{
    /// <summary>
    /// MySQL data provider specific extension methods for <see cref="ISerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        /// Configures the SerilogUi to connect to a MySQL/MariaDb database using Serilog.Sinks.MySQL.
        /// Provider expects sink to store timestamp in utc.
        /// </summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="setupOptions">The MySql options action.</param>
        public static ISerilogUiOptionsBuilder UseMySqlServer(
            this ISerilogUiOptionsBuilder optionsBuilder,
            Action<RelationalDbOptions> setupOptions
        )
        {
            var dbOptions = new RelationalDbOptions("dbo");
            setupOptions(dbOptions);
            dbOptions.Validate();

            optionsBuilder.Services.AddScoped<IDataProvider, MySqlDataProvider>(p =>
                ActivatorUtilities.CreateInstance<MySqlDataProvider>(p, dbOptions));

            return optionsBuilder;
        }

        /// <summary>
        /// Configures the SerilogUi to connect to a MySQL/MariaDb database using Serilog.Sinks.MariaDB.
        /// Provider expects sink to store timestamp in utc.
        /// </summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="setupOptions">The MySql options action.</param>
        public static ISerilogUiOptionsBuilder UseMariaDbServer(
            this ISerilogUiOptionsBuilder optionsBuilder,
            Action<RelationalDbOptions> setupOptions
        )
        {
            var dbOptions = new RelationalDbOptions("dbo");
            setupOptions(dbOptions);
            dbOptions.Validate();

            optionsBuilder.Services.AddScoped<IDataProvider, MariaDbDataProvider>(p => ActivatorUtilities.CreateInstance<MariaDbDataProvider>(p, dbOptions));
            
            return optionsBuilder;
        }
    }
}