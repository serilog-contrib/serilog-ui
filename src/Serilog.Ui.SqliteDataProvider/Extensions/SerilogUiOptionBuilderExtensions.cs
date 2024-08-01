using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.SqliteDataProvider
{
    /// <summary>
    ///     Sqlite data provider specific extension methods for <see cref="SerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        ///     Configures the SerilogUi to connect to a Sqlite database.
        /// </summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="tableName"> Name of the table. </param>
        /// <exception cref="ArgumentNullException"> throw if connectionString is null </exception>
        /// <exception cref="ArgumentNullException"> throw is tableName is null </exception>
        public static void UseSqliteServer(
            this SerilogUiOptionsBuilder optionsBuilder,
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

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services
                .AddScoped<IDataProvider, SqliteDataProvider>(p => ActivatorUtilities.CreateInstance<SqliteDataProvider>(p, relationProvider));

        }
    }
}
