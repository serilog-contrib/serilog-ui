#nullable enable
using System;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.MsSqlServerProvider.Extensions
{
    /// <summary>
    /// SQL Server data provider specific extension methods for <see cref="ISerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>Configures the SerilogUi to connect to a SQL Server database.</summary>
        /// <param name="optionsBuilder"> The options builder. </param>
        /// <param name="setupOptions">The Ms Sql options action.</param>
        /// <param name="dateTimeCustomParsing">
        /// Delegate to customize the DateTime parsing.
        /// It throws <see cref="InvalidOperationException" /> if the return DateTime isn't UTC kind.
        /// </param>
        public static ISerilogUiOptionsBuilder UseSqlServer(
            this ISerilogUiOptionsBuilder optionsBuilder,
            Action<RelationalDbOptions> setupOptions,
            Func<string, DateTime>? dateTimeCustomParsing = null
        )
        {
            var dbOptions = new RelationalDbOptions("dbo");
            setupOptions(dbOptions);
            dbOptions.Validate();

            SqlMapper.AddTypeHandler(new DapperDateTimeHandler(dateTimeCustomParsing));

            optionsBuilder.Services
                .AddScoped<IDataProvider, SqlServerDataProvider>(p =>
                    ActivatorUtilities.CreateInstance<SqlServerDataProvider>(p, dbOptions));

            return optionsBuilder;
        }
    }
}