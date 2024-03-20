using System;
using Ardalis.GuardClauses;

namespace Serilog.Ui.Core.OptionsBuilder;

/// <summary>
/// Base options for provider.
/// </summary>
public class BaseDbOptions : IDbOptions
{
    /// <summary>
    /// Required parameter.
    /// </summary>
    public string ConnectionString { get; private set; }

    /// <summary>
    /// Throws if ConnectionString is null or whitespace.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public virtual void Validate()
    {
        Guard.Against.NullOrWhiteSpace(ConnectionString);
    }

    internal void WithConnectionString(string connectionString)
    {
        ConnectionString = connectionString;
    }
}

/// <summary>
/// BaseDbOptionsExtensions.
/// </summary>
public static class BaseDbOptionsExtensions
{
    /// <summary>
    /// Fluently sets ConnectionString.
    /// </summary>
    /// <param name="options">Options inheriting <see cref="BaseDbOptions"/></param>
    /// <param name="connectionString"></param>
    public static T WithConnectionString<T>(this T options, string connectionString)
        where T : BaseDbOptions
    {
        options.WithConnectionString(connectionString);
        return options;
    }
}