using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace Serilog.Ui.Core.Models;

/// <summary>
/// The query parameters available to fetch logs.
/// </summary>
public class FetchLogsQuery
{
    private int _page;

    private FetchLogsQuery()
    {
    }

    /// <summary>
    /// Gets or sets the DatabaseKey.
    /// </summary>
    public string DatabaseKey { get; private set; }

    /// <summary>
    /// Gets or sets the Page.
    /// </summary>
    public int Page
    {
        get => _page - 1; // providers will use page as 0-based
        private set => _page = value;
    }

    /// <summary>
    /// Gets or sets the Count.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Gets or sets the Level.
    /// </summary>
    public string Level { get; private set; }

    /// <summary>
    /// Gets or sets the SearchCriteria.
    /// </summary>
    public string SearchCriteria { get; private set; }

    /// <summary>
    /// Gets or sets the StartDate.
    /// </summary>
    public DateTime? StartDate { get; private set; }

    /// <summary>
    /// Gets or sets the EndDate.
    /// </summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>
    /// Gets or sets the SortOn.
    /// </summary>
    public SearchOptions.SortProperty SortOn { get; private set; } = SearchOptions.SortProperty.Timestamp;

    /// <summary>
    /// Gets or sets the SortBy.
    /// </summary>
    public SearchOptions.SortDirection SortBy { get; private set; } = SearchOptions.SortDirection.Desc;

    /// <summary>
    /// Convert the dates to UTC, if not null.
    /// </summary>
    public void ToUtcDates()
    {
        StartDate = StartDate?.ToUniversalTime();
        EndDate = EndDate?.ToUniversalTime();
    }


    /// <summary>
    /// Specify the dates kind to UTC, if not null.
    /// </summary>
    public void ToUtcKindDates()
    {
        if (StartDate != null && StartDate.Value.Kind != DateTimeKind.Utc)
        {
            StartDate = DateTime.SpecifyKind(StartDate.Value, DateTimeKind.Utc);
        }

        if (EndDate != null && EndDate.Value.Kind != DateTimeKind.Utc)
        {
            EndDate = DateTime.SpecifyKind(EndDate.Value, DateTimeKind.Utc);
        }
    }

    /// <summary>
    /// Convert the dates to local time, if not null.
    /// </summary>
    public void ToLocalDates()
    {
        StartDate = StartDate?.ToLocalTime();
        EndDate = EndDate?.ToLocalTime();
    }

    /// <summary>
    /// Parse a IQueryCollection dictionary.
    /// </summary>
    /// <param name="queryParams"></param>
    /// <returns>The query instance.</returns>
    public static FetchLogsQuery ParseQuery(Dictionary<string, StringValues> queryParams)
    {
        var (currentPage, currentCount) = ParseRequiredParams(queryParams);
        var (outputStartDate, outputEndDate) = ParseDates(queryParams);
        var (sortOn, sortBy) = ParseSort(queryParams);
        queryParams.TryGetValue("key", out var keyStr);
        queryParams.TryGetValue("level", out var levelStr);
        queryParams.TryGetValue("search", out var searchStr);

        return new FetchLogsQuery
        {
            Count = currentCount,
            DatabaseKey = keyStr,
            EndDate = outputEndDate,
            Level = levelStr,
            Page = currentPage,
            SearchCriteria = searchStr,
            SortBy = sortBy,
            SortOn = sortOn,
            StartDate = outputStartDate
        };
    }

    private static (int currentPage, int currentCount) ParseRequiredParams(IReadOnlyDictionary<string, StringValues> queryParams)
    {
        queryParams.TryGetValue("page", out var pageStr);
        queryParams.TryGetValue("count", out var countStr);
        var canPageBeParsed = int.TryParse(pageStr, out var currentPage);
        var canCountBeParsed = int.TryParse(countStr, out var currentCount);
        currentPage = !canPageBeParsed ? 1 : currentPage;
        currentCount = !canCountBeParsed ? 10 : currentCount;

        return (currentPage, currentCount);
    }

    private static (DateTime?, DateTime?) ParseDates(IReadOnlyDictionary<string, StringValues> queryParams)
    {
        queryParams.TryGetValue("startDate", out var startDateStar);
        queryParams.TryGetValue("endDate", out var endDateStar);

        var couldConvertStart = DateTime.TryParse(startDateStar, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var startDate);
        var couldConvertEnd = DateTime.TryParse(endDateStar, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var endDate);

        var outputStartDate = couldConvertStart ? (DateTime?)null : startDate;
        var outputEndDate = couldConvertEnd ? (DateTime?)null : endDate;

        return (outputStartDate, outputEndDate);
    }

    private static (SearchOptions.SortProperty, SearchOptions.SortDirection) ParseSort(IReadOnlyDictionary<string, StringValues> queryParams)
    {
        queryParams.TryGetValue("sortOn", out var sortStrOn);
        queryParams.TryGetValue("sortBy", out var sortStrBy);

        Enum.TryParse<SearchOptions.SortProperty>(sortStrOn, out var sortProperty);
        Enum.TryParse<SearchOptions.SortDirection>(sortStrBy, out var sortDirection);

        return (sortProperty, sortDirection);
    }
}