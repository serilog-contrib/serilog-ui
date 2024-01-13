using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.PostgreSqlProvider
{
    /// <summary>
    ///     PostgreSQL data provider specific extension methods for <see cref="SerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        ///     Configures the SerilogUi to connect to a PostgreSQL database.
        /// </summary>
        /// <param name="sinkType">
        ///     The sink that used to store logs in the PostgreSQL database. This data provider supports two sinks,
        ///     <see href="https://github.com/b00ted/serilog-sinks-postgresql">Serilog.Sinks.Postgresql</see> and
        ///     <see href="https://github.com/serilog-contrib/Serilog.Sinks.Postgresql.Alternative">Serilog.Sinks.Postgresql.Alternative</see>.
        /// </param>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="tableName"> Name of the table. </param>
        /// <param name="schemaName">
        ///     Name of the table schema. default value is <c> public </c>
        /// </param>
        /// <exception cref="ArgumentNullException"> throw if connectionString is null </exception>
        /// <exception cref="ArgumentNullException"> throw is tableName is null </exception>
        public static void UseNpgSql(
            this SerilogUiOptionsBuilder optionsBuilder,
            PostgreSqlSinkType sinkType,
            string connectionString,
            string tableName,
            string schemaName = "public"
        )
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var relationProvider = new PostgreSqlDbOptions
            {
                ConnectionString = connectionString,
                TableName = tableName,
                Schema = !string.IsNullOrWhiteSpace(schemaName) ? schemaName : "public",
                SinkType = sinkType
            };

            QueryBuilder.SetSinkType(sinkType);

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services
                .AddScoped<IDataProvider, PostgresDataProvider>(p => ActivatorUtilities.CreateInstance<PostgresDataProvider>(p, relationProvider));
        }
    }
}