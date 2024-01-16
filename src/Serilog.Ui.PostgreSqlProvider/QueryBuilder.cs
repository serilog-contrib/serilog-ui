using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Ui.PostgreSqlProvider;

internal static class QueryBuilder
{
    private static SinkColumnNames _columns;

    internal static void SetSinkType(PostgreSqlSinkType sinkType)
    {
        _columns = sinkType == PostgreSqlSinkType.SerilogSinksPostgreSQL
            ? new PostgreSqlSinkColumnNames()
            : new PostgreSqlAlternativeSinkColumnNames();
    }

    internal static string BuildFetchLogsQuery(
        string schema,
        string tableName,
        string level,
        string searchCriteria,
        ref DateTime? startDate,
        ref DateTime? endDate)
    {
        StringBuilder queryBuilder = new();

        queryBuilder
            .Append("SELECT ")
            .Append($"{_columns.RenderedMessage}, {_columns.MessageTemplate}, {_columns.Level}, {_columns.Timestamp}, {_columns.Exception}, {_columns.LogEventSerialized} AS \"Properties\"")
            .Append(" FROM \"")
            .Append(schema)
            .Append("\".\"")
            .Append(tableName)
            .Append("\"");

        GenerateWhereClause(queryBuilder, level, searchCriteria, ref startDate, ref endDate);

        queryBuilder.Append(" ORDER BY ");
        queryBuilder.Append(_columns.Timestamp);
        queryBuilder.Append(" DESC LIMIT @Count OFFSET @Offset ");

        return queryBuilder.ToString();
    }

    internal static string BuildCountLogsQuery(
        string schema,
        string tableName,
        string level,
        string searchCriteria,
        ref DateTime? startDate,
        ref DateTime? endDate)
    {
        StringBuilder queryBuilder = new();

        queryBuilder.Append($"SELECT COUNT(\"{_columns.RenderedMessage}\") FROM \"");
        queryBuilder.Append(schema);
        queryBuilder.Append("\".\"");
        queryBuilder.Append(tableName);
        queryBuilder.Append("\"");

        GenerateWhereClause(queryBuilder, level, searchCriteria, ref startDate, ref endDate);

        return queryBuilder.ToString();
    }

    private static void GenerateWhereClause(
        StringBuilder queryBuilder,
        string level,
        string searchCriteria,
        ref DateTime? startDate,
        ref DateTime? endDate)
    {
        var conditions = new List<string>();

        if (!string.IsNullOrEmpty(level))
        {
            conditions.Add($"{_columns.Level} = @Level");
        }

        if (!string.IsNullOrEmpty(searchCriteria))
        {
            conditions.Add($"({_columns.RenderedMessage} LIKE @Search OR {_columns.Exception} LIKE @Search)");
        }

        if (startDate.HasValue)
        {
            conditions.Add($"{_columns.Timestamp} >= @StartDate");
        }

        if (endDate.HasValue)
        {
            conditions.Add($"{_columns.Timestamp} <= @EndDate");
        }

        if (conditions.Count > 0)
        {
            queryBuilder
                .Append(" WHERE TRUE AND ")
                .Append(string.Join(" AND ", conditions)); ;
        }
    }
}