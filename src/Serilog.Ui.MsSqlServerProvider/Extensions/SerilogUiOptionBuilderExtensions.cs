using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.MsSqlServerProvider
{
    public static class DataProviderOptionBuilderExtensions
    {
        public static void UseSqlServer(
            this SerilogUiOptionsBuilder optionsBuilder,
            string connectionString,
            string tableName,
            string schemaName = "dbo"
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
            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddScoped<IDataProvider, SqlServerDataProvider>();
        }
    }
}