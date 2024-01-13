using System;
using System.Text;

namespace Serilog.Ui.PostgreSqlProvider;

internal static class QueryBuilder
{
    private static SinkColumnNames _columns;

    private static void SetSinkType(PostgreSqlSinkType sinkType)
    {
        _columns = sinkType == PostgreSqlSinkType.SerilogSinksPostgreSQL
            ? new PostgreSqlSinkColumnNames()
            : new PostgreSqlAlternativeSinkColumnNames();
    }

    public static string BuildFetchLogsQuery(
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

        queryBuilder.Append(" ORDER BY timestamp DESC LIMIT @Count OFFSET @Offset ");

        return queryBuilder.ToString();
    }

    private static void GenerateWhereClause(
        StringBuilder queryBuilder,
        string level,
        string searchCriteria,
        ref DateTime? startDate,
        ref DateTime? endDate)
    {
        var whereIncluded = false;

        if (!string.IsNullOrEmpty(level))
        {
            queryBuilder.Append($" WHERE {_columns.Level} = @Level ");
            whereIncluded = true;
        }

        if (!string.IsNullOrEmpty(searchCriteria))
        {
            queryBuilder
                .Append(AndOrWhere(whereIncluded))
                .Append($"{_columns.RenderedMessage} LIKE @Search OR exception LIKE @Search ");
            whereIncluded = true;
        }

        if (startDate != null)
        {
            queryBuilder
                .Append(AndOrWhere(whereIncluded))
                .Append($"{_columns.Timestamp} >= @StartDate ");
            whereIncluded = true;
        }

        if (endDate != null)
        {
            queryBuilder
                .Append(AndOrWhere(whereIncluded))
                .Append($"{_columns.Timestamp} <= @EndDate ");
        }

        static string AndOrWhere(bool whereIncluded)
        {
            const string and = " AND ";
            const string where = " WHERE ";

            return whereIncluded ? and : where;
        }
    }
}