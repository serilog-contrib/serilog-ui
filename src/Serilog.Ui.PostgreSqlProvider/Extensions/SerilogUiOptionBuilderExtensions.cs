using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.PostgreSqlProvider.Models;
using System;

namespace Serilog.Ui.PostgreSqlProvider.Extensions;

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
    public static ISerilogUiOptionsBuilder UseNpgSql(this ISerilogUiOptionsBuilder optionsBuilder, Action<PostgreSqlDbOptions> setupOptions)
        => optionsBuilder.UseNpgSql<PostgresLogModel>(setupOptions);

    /// <summary>
    ///  Configures the SerilogUi to connect to a PostgreSQL database.
    /// </summary>
    /// <typeparam name="T">The log model, containing any additional columns. It must inherit <see cref="PostgresLogModel"/>.</typeparam>
    /// <param name="optionsBuilder"> The Serilog UI option builder. </param>
    /// <param name="setupOptions">The Postgres Sql options action.</param>
    public static ISerilogUiOptionsBuilder UseNpgSql<T>(this ISerilogUiOptionsBuilder optionsBuilder, Action<PostgreSqlDbOptions> setupOptions)
        where T : PostgresLogModel
    {
        PostgreSqlDbOptions dbOptions = new("public");
        setupOptions(dbOptions);
        dbOptions.Validate();

        string providerName = dbOptions.GetProviderName(PostgresDataProvider.ProviderName);
        optionsBuilder.RegisterExceptionAsStringForProviderKey(providerName);

        bool customModel = typeof(T) != typeof(PostgresLogModel);
        if (customModel)
        {
            optionsBuilder.RegisterColumnsInfo<T>(providerName);
            optionsBuilder.Services.AddScoped<IDataProvider>(_ => new PostgresDataProvider<T>(dbOptions, new PostgresQueryBuilder<T>()));
        }
        else
        {
            optionsBuilder.Services.AddScoped<IDataProvider>(_ => new PostgresDataProvider(dbOptions, new PostgresQueryBuilder<PostgresLogModel>()));
        }

        return optionsBuilder;
    }
}