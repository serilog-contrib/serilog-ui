using Serilog.Ui.Core.Models.Options;

namespace Serilog.Ui.Core.Extensions;

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