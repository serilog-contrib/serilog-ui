using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.MySqlProvider.Extensions
{
    /// <summary>
    /// MySQL data provider specific extension methods for <see cref="ISerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        /// Configures the SerilogUi to connect to a MySQL/MariaDb database expecting
        /// <seealso href="https://github.com/saleem-mirza/serilog-sinks-mysql">Serilog.Sinks.MySQL.</seealso> defaults.
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

            optionsBuilder.Services.AddScoped<IDataProvider, MySqlDataProvider>(_ => new MySqlDataProvider(dbOptions));

            return optionsBuilder;
        }

        /// <summary>
        /// Configures the SerilogUi to connect to a MySQL/MariaDb database expecting
        /// <seealso href="https://github.com/TeleSoftas/serilog-sinks-mariadb">Serilog.Sinks.MariaDB.</seealso> defaults.
        /// Provider expects sink to store timestamp in utc.
        /// </summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="setupOptions">The MySql options action.</param>
        public static ISerilogUiOptionsBuilder UseMariaDbServer(
            this ISerilogUiOptionsBuilder optionsBuilder,
            Action<RelationalDbOptions> setupOptions
        ) => optionsBuilder.UseMariaDbServer<MySqlLogModel>(setupOptions);

        /// <summary>
        /// Configures the SerilogUi to connect to a MySQL/MariaDb database expecting
        /// <seealso href="https://github.com/TeleSoftas/serilog-sinks-mariadb">Serilog.Sinks.MariaDB.</seealso> defaults.
        /// Provider expects sink to store timestamp in utc.
        /// </summary>
        /// <typeparam name="T">The log model, containing any additional columns. It must inherit <see cref="MySqlLogModel"/>.</typeparam>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="setupOptions">The MySql options action.</param>
        public static ISerilogUiOptionsBuilder UseMariaDbServer<T>(
            this ISerilogUiOptionsBuilder optionsBuilder,
            Action<RelationalDbOptions> setupOptions
        ) where T : MySqlLogModel
        {
            var dbOptions = new RelationalDbOptions("dbo");
            setupOptions(dbOptions);
            dbOptions.Validate();

            var customModel = typeof(T) != typeof(MySqlLogModel);
            if (customModel)
            {
                optionsBuilder.RegisterColumnsInfo<T>(dbOptions.ToDataProviderName(MariaDbDataProvider.ProviderName));
                optionsBuilder.Services.AddScoped<IDataProvider>(_ => new MariaDbDataProvider<T>(dbOptions));
                return optionsBuilder;
            }

            optionsBuilder.Services.AddScoped<IDataProvider>(_ => new MariaDbDataProvider(dbOptions));
            return optionsBuilder;
        }
    }
}