using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Core.Models.Options;
using System;

namespace Serilog.Ui.MsSqlServerProvider.Extensions;

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
    ) => optionsBuilder.UseSqlServer<SqlServerLogModel>(setupOptions, dateTimeCustomParsing);

    /// <summary>Configures the SerilogUi to connect to a SQL Server database.</summary>
    /// <typeparam name="T">The log model, containing any additional columns. It must inherit <see cref="SqlServerLogModel"/>.</typeparam>
    /// <param name="optionsBuilder"> The options builder. </param>
    /// <param name="setupOptions">The Ms Sql options action.</param>
    /// <param name="dateTimeCustomParsing">
    /// Delegate to customize the DateTime parsing.
    /// It throws <see cref="InvalidOperationException" /> if the return DateTime isn't UTC kind.
    /// </param>
    public static ISerilogUiOptionsBuilder UseSqlServer<T>(
        this ISerilogUiOptionsBuilder optionsBuilder,
        Action<RelationalDbOptions> setupOptions,
        Func<string, DateTime>? dateTimeCustomParsing = null
    ) where T : SqlServerLogModel
    {
        SqlServerDbOptions dbOptions = new("dbo");
        setupOptions(dbOptions);
        dbOptions.Validate();

        string providerName = dbOptions.GetProviderName(SqlServerDataProvider.MsSqlProviderName);
        optionsBuilder.RegisterExceptionAsStringForProviderKey(providerName);
        SqlMapper.AddTypeHandler(new DapperDateTimeHandler(dateTimeCustomParsing));

        bool customModel = typeof(T) != typeof(SqlServerLogModel);
        if (customModel)
        {
            optionsBuilder.RegisterColumnsInfo<T>(providerName);
            optionsBuilder.Services.AddScoped<IDataProvider>(_ => new SqlServerDataProvider<T>(dbOptions, new SqlServerQueryBuilder<T>()));
        }
        else
        {
            optionsBuilder.Services.AddScoped<IDataProvider>(_ =>
                new SqlServerDataProvider(dbOptions, new SqlServerQueryBuilder<SqlServerLogModel>()));
        }

        return optionsBuilder;
    }
}