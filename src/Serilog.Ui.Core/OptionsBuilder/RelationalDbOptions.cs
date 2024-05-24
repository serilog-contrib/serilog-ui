using System;
using Ardalis.GuardClauses;

namespace Serilog.Ui.Core.OptionsBuilder;

/// <summary>
/// RelationDbOptions class
/// </summary>
public class RelationalDbOptions : BaseDbOptions
{
    /// <summary>
    /// Creates a new instance of the class, setting a default schema name.
    /// </summary>
    /// <param name="defaultSchemaName"></param>
    public RelationalDbOptions(string defaultSchemaName)
    {
        Schema = defaultSchemaName;
    }

    /// <summary>
    /// Required parameter.
    /// </summary>
    public string TableName { get; private set; } = string.Empty;

    /// <summary>
    /// Optional parameter, defaults to dbo.
    /// </summary>
    public string Schema { get; private set; }

    /// <summary>
    /// Get the provider name.
    /// </summary>
    public string GetProviderName(string provider)
        => !string.IsNullOrWhiteSpace(CustomProviderName) ? CustomProviderName : ToDataProviderName(provider);

    /// <summary>
    /// Throws if ConnectionString, TableName, Schema is null or whitespace.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public override void Validate()
    {
        Guard.Against.NullOrWhiteSpace(TableName);
        Guard.Against.NullOrWhiteSpace(Schema);

        base.Validate();
    }

    /// <summary>
    /// Fluently sets TableName.
    /// </summary>
    /// <param name="tableName"></param>
    internal void WithTable(string tableName)
    {
        TableName = tableName;
    }

    /// <summary>
    /// Fluently sets SchemaName.
    /// </summary>
    /// <param name="schemaName"></param>
    internal void WithSchema(string schemaName)
    {
        Schema = schemaName;
    }

    /// <summary>
    /// Generates a complete data provider name, by using its properties.
    /// </summary>
    /// <param name="providerName">Data provider name.</param>
    /// <returns></returns>
    private string ToDataProviderName(string providerName) => string.Join(".", providerName, Schema, TableName);
}

/// <summary>
/// RelationalDbOptionsExtensions.
/// </summary>
public static class RelationalDbOptionsExtensions
{
    /// <summary>
    /// Fluently sets TableName.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="tableName"></param>
    public static T WithTable<T>(this T options, string tableName)
        where T : RelationalDbOptions
    {
        options.WithTable(tableName);
        return options;
    }

    /// <summary>
    /// Fluently sets SchemaName.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="schemaName"></param>
    public static T WithSchema<T>(this T options, string schemaName)
        where T : RelationalDbOptions
    {
        options.WithSchema(schemaName);
        return options;
    }
}