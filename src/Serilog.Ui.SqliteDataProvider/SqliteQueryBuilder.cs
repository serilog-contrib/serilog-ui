using System;
using System.Text;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.QueryBuilder.Sql;

namespace Serilog.Ui.SqliteDataProvider;

/// <summary>
/// Provides methods to build SQL queries specifically for Sqlite to fetch and count logs.
/// </summary>
/// <typeparam name="TModel">The type of the log model.</typeparam>
public class SqliteQueryBuilder : SqlQueryBuilder<LogModel>
{
    ///<inheritdoc />
    public override string BuildFetchLogsQuery(SinkColumnNames columns, string schema, string tableName, FetchLogsQuery query)
    {
        StringBuilder queryStr = new();

        GenerateSelectClause(queryStr, columns, schema, tableName);

        GenerateWhereClause(queryStr, columns, query.Level, query.SearchCriteria, query.StartDate, query.EndDate);

        queryStr.Append($"{GenerateSortClause(columns, query.SortOn, query.SortBy)} LIMIT @Offset, @Count");

        return queryStr.ToString();
    }

    /// <inheritdoc/>
    public override string BuildCountLogsQuery(SinkColumnNames columns, string schema, string tableName, FetchLogsQuery query)
    {
        StringBuilder queryStr = new();

        queryStr.Append($"SELECT COUNT(Id) FROM {tableName} ");

        GenerateWhereClause(queryStr, columns, query.Level, query.SearchCriteria, query.StartDate, query.EndDate);

        return queryStr.ToString();
    }

    protected override string GenerateSortClause(SinkColumnNames columns, SearchOptions.SortProperty sortOn, SearchOptions.SortDirection sortBy)
        => $"ORDER BY {GetSortColumnName(columns, sortOn)} {sortBy.ToString().ToUpper()}";

    /// <inheritdoc/>
    private static void GenerateSelectClause(StringBuilder queryBuilder, SinkColumnNames columns, string schema, string tableName)
    {
        queryBuilder.Append($"SELECT Id, {columns.Message} AS Message, {columns.Level}, {columns.Timestamp}, {columns.Exception}, {columns.LogEventSerialized} ");
        queryBuilder.Append($"FROM {tableName} ");
    }

    /// <inheritdoc/>
    private static void GenerateWhereClause(
        StringBuilder queryBuilder,
        SinkColumnNames columns,
        string? level,
        string? searchCriteria,
        DateTime? startDate,
        DateTime? endDate)
    {
        var conditionStart = "WHERE";

        if (!string.IsNullOrWhiteSpace(level))
        {
            queryBuilder.Append($"{conditionStart} {columns.Level} = @Level ");
            conditionStart = "AND";
        }

        if (!string.IsNullOrWhiteSpace(searchCriteria))
        {
            queryBuilder.Append($"{conditionStart} ({columns.Message} LIKE @Search OR {columns.Exception} LIKE @Search) ");
            conditionStart = "AND";
        }

        if (startDate != null)
        {
            queryBuilder.Append($"{conditionStart} {columns.Timestamp} >= @StartDate ");
            conditionStart = "AND";
        }

        if (endDate != null)
        {
            queryBuilder.Append($"{conditionStart} {columns.Timestamp} <= @EndDate ");
        }
    }
}