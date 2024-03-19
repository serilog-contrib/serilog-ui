using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.MySqlProvider
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
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="tableName"> Name of the table. </param>
        /// <exception cref="ArgumentNullException"> throw if connectionString is null </exception>
        /// <exception cref="ArgumentNullException"> throw is tableName is null </exception>
        public static void UseMySqlServer(
            this ISerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string tableName
        )
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var relationProvider = new RelationalDbOptions
            {
                ConnectionString = connectionString,
                TableName = tableName
            };

            optionsBuilder.Services.AddScoped<IDataProvider, MySqlDataProvider>(p => ActivatorUtilities.CreateInstance<MySqlDataProvider>(p, relationProvider));
        }

        /// <summary>
        /// Configures the SerilogUi to connect to a MySQL/MariaDb database using Serilog.Sinks.MariaDB.
        /// Provider expects sink to store timestamp in utc.
        /// </summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="tableName"> Name of the table. </param>
        /// <exception cref="ArgumentNullException"> throw if connectionString is null </exception>
        /// <exception cref="ArgumentNullException"> throw is tableName is null </exception>
        public static void UseMariaDbServer(
            this ISerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string tableName
        )
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var relationProvider = new RelationalDbOptions
            {
                ConnectionString = connectionString,
                TableName = tableName
            };

            optionsBuilder.Services.AddScoped<IDataProvider, MariaDbDataProvider>(p => ActivatorUtilities.CreateInstance<MariaDbDataProvider>(p, relationProvider));
        }
    }
}