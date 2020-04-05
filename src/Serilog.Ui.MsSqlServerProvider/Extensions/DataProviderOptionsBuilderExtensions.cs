using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.MsSqlServerProvider
{
    public static class DataProviderOptionBuilderExtensions
    {
        public static void UseSqlServer(
            this DataProviderOptionsBuilder optionsBuilder,
            string connectionString,
            string tableName,
            string schemaName = null
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

            ((IDataProviderOptionsBuilder)optionsBuilder).Services.AddSingleton(relationProvider);
        }
    }
}