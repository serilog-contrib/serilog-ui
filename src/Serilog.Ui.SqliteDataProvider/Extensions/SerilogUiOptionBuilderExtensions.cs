using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Core.Models.Options;
using System;

namespace Serilog.Ui.SqliteDataProvider.Extensions;

/// <summary>
/// SQLite data provider specific extension methods for <see cref="ISerilogUiOptionsBuilder"/>.
/// </summary>
public static class SerilogUiOptionBuilderExtensions
{
    /// <summary> Configures the SerilogUi to connect to a SQLite database.</summary>
    /// <param name="optionsBuilder"> The options builder. </param>
    /// <param name="setupOptions">The SQLite options action.</param>
    public static ISerilogUiOptionsBuilder UseSqliteServer(
        this ISerilogUiOptionsBuilder optionsBuilder,
        Action<RelationalDbOptions> setupOptions)
    {
        var dbOptions = new SqliteDbOptions();
        setupOptions(dbOptions);
        dbOptions.Validate();

        optionsBuilder.Services.AddScoped<IDataProvider, SqliteDataProvider>(_ => new SqliteDataProvider(dbOptions, new SqliteQueryBuilder()));

        return optionsBuilder;
    }
}