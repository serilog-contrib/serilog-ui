using Serilog.Ui.Core.Models.Options;

namespace Serilog.Ui.Core.Extensions;

/// <summary>
/// DbOptionsBaseExtensions.
/// </summary>
public static class DbOptionsBaseExtensions
{
    /// <summary>
    /// Fluently sets ConnectionString.
    /// </summary>
    /// <param name="options">Options inheriting <see cref="DbOptionsBase"/></param>
    /// <param name="connectionString"></param>
    public static T WithConnectionString<T>(this T options, string connectionString)
        where T : DbOptionsBase
    {
        options.WithConnectionString(connectionString);
        return options;
    }

    /// <summary>
    /// Fluently sets a custom provider name.
    /// </summary>
    /// <param name="options">Options inheriting <see cref="DbOptionsBase"/></param>
    /// <param name="customProviderName"></param>
    public static T WithCustomProviderName<T>(this T options, string customProviderName)
        where T : DbOptionsBase
    {
        options.WithCustomProviderName(customProviderName);
        return options;
    }
}