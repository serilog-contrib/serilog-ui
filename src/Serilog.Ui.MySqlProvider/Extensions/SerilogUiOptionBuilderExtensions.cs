using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.MySqlProvider.Extensions
{
    /// <summary>
    ///     MySQL data provider specific extension methods for <see cref="SerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        ///     Configures the SerilogUi to connect to a MySQL database.
        /// </summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="tableName"> Name of the table. </param>
        /// <exception cref="ArgumentNullException"> throw if connectionString is null </exception>
        /// <exception cref="ArgumentNullException"> throw is tableName is null </exception>
        public static void UseMySqlServer(
            this SerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string tableName
        )
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var relationProvider = new RelationalDbOptions
            {
                ConnectionString = connectionString,
                TableName = tableName
            };

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton(relationProvider);
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddScoped<IDataProvider, MySqlDataProvider>();
        }
    }
}