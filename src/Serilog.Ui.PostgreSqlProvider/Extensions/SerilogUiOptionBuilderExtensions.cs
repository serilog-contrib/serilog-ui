using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;

namespace Serilog.Ui.PostgreSqlProvider.Extensions
{
    public static class SerilogUiOptionBuilderExtensions
    {
        public static void UseNpgSql(
            this SerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string tableName,
            string schemaName = "public"
        )
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var relationProvider = new RelationalDbOptions
            {
                ConnectionString = connectionString,
                TableName = tableName,
                Schema = schemaName
            };

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton(relationProvider);
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddScoped<IDataProvider, PostgresDataProvider>();
        }
    }
}