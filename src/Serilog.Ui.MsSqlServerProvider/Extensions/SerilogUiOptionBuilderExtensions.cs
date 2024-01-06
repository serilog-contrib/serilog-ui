using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;
using Dapper;

namespace Serilog.Ui.MsSqlServerProvider
#nullable enable
{
    /// <summary>
    ///     SQL Server data provider specific extension methods for <see cref="SerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        ///     Configures the SerilogUi to connect to a SQL Server database.
        /// </summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="tableName"> Name of the table. </param>
        /// <param name="schemaName">
        ///     Name of the table schema. default value is <c> dbo </c>
        /// </param>
        /// <param name="dateTimeCustomParsing">Delegate to customize the DateTime parsing. It must return a DateTime with UTC</param>
        /// <exception cref="ArgumentNullException"> throw if connectionString is null </exception>
        /// <exception cref="ArgumentNullException"> throw is tableName is null </exception>
        public static SerilogUiOptionsBuilder UseSqlServer(
            this SerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string tableName,
            string schemaName = "dbo",
            Func<string, DateTime>? dateTimeCustomParsing = null
        )
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var relationProvider = new RelationalDbOptions
            {
                ConnectionString = connectionString,
                TableName = tableName,
                Schema = !string.IsNullOrWhiteSpace(schemaName) ? schemaName : "dbo"
            };

            SqlMapper.AddTypeHandler(new DapperDateTimeHandler(dateTimeCustomParsing));

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services
                .AddScoped<IDataProvider, SqlServerDataProvider>(p =>
                    ActivatorUtilities.CreateInstance<SqlServerDataProvider>(p, relationProvider));

            return optionsBuilder;
        }
    }
}