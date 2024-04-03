using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;

namespace Serilog.Ui.PostgreSqlProvider.Extensions
{
    /// <summary>
    ///  PostgreSQL data provider specific extension methods for <see cref="ISerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        ///  Configures the SerilogUi to connect to a PostgreSQL database.
        /// </summary>
        /// <param name="optionsBuilder"> The Serilog UI option builder. </param>
        /// <param name="setupOptions">The Postgres Sql options action.</param>
        public static ISerilogUiOptionsBuilder UseNpgSql(
            this ISerilogUiOptionsBuilder optionsBuilder,
            Action<PostgreSqlDbOptions> setupOptions
        )
        {
            var dbOptions = new PostgreSqlDbOptions("public");
            setupOptions(dbOptions);
            dbOptions.Validate();

            QueryBuilder.SetSinkType(dbOptions.SinkType);

            optionsBuilder.Services.AddScoped<IDataProvider, PostgresDataProvider>(p => ActivatorUtilities.CreateInstance<PostgresDataProvider>(p, dbOptions));
            
            return optionsBuilder;
        }
    }
}