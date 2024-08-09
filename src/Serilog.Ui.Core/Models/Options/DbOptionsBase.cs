using System;
using Ardalis.GuardClauses;

namespace Serilog.Ui.Core.Models.Options;

/// <summary>
/// Base options for provider.
/// </summary>
public abstract class DbOptionsBase : IDbOptions
{
    /// <summary>
    /// Required parameter.
    /// </summary>
    public string? ConnectionString { get; private set; }

    /// <summary>
    /// Optional parameter.
    /// </summary>
    protected string CustomProviderName { get; private set; } = string.Empty;

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

    internal void WithCustomProviderName(string customProviderName)
    {
        CustomProviderName = customProviderName;
    }
}