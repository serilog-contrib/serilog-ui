using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Serilog.Ui.Core;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Serilog.Ui.Core.Models.SearchOptions;

namespace Serilog.Ui.Web.Endpoints
{
    internal class SerilogUiEndpoints : ISerilogUiEndpoints
    {
        private readonly ILogger<SerilogUiEndpoints> _logger;
        private readonly AggregateDataProvider _aggregateDataProvider;

        public SerilogUiEndpoints(ILogger<SerilogUiEndpoints> logger, AggregateDataProvider aggregateDataProvider)
        {
            _logger = logger;
            _aggregateDataProvider = aggregateDataProvider;

            _providerKeys = _aggregateDataProvider.Keys.ToArray();
        }
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly string[] _providerKeys;

        public UiOptions Options { get; private set; }

        public async Task GetApiKeysAsync(HttpContext httpContext)
        {
            try
            {
                SetResponseContentType(httpContext);

                var result = JsonSerializer.Serialize(_providerKeys, JsonSerializerOptions);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await httpContext.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                await OnError(httpContext, ex);
            }
        }

        public async Task GetLogsAsync(HttpContext httpContext)
        {
            try
            {
                SetResponseContentType(httpContext);

                var result = await FetchLogsAsync(httpContext);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await httpContext.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                await OnError(httpContext, ex);
            }
        }

        public void SetOptions(UiOptions options)
        {
            Options = options;
        }

        private async Task<string> FetchLogsAsync(HttpContext httpContext)
        {
            var (currentPage, count, dbKey, level, textSearch, start, end, on, by) = ParseQuery(httpContext.Request.Query);

            if (!string.IsNullOrWhiteSpace(dbKey))
            {
                _aggregateDataProvider.SwitchToProvider(dbKey);
            }

            var (logs, total) = await _aggregateDataProvider.FetchDataAsync(currentPage, count, level, textSearch, start, end, on, by);

            var result = JsonSerializer.Serialize(new { logs, total, count, currentPage }, JsonSerializerOptions);

            return result;
        }

        private static (int currPage, int count, string dbKey, string level, string textSearch, DateTime? start, DateTime? end, SortProperty sortOn, SortDirection sortBy) ParseQuery(IQueryCollection queryParams)
        {
            var (currentPage, currentCount) = ParseRequiredParams(queryParams);
            var (outputStartDate, outputEndDate) = ParseDates(queryParams);
            var (sortOn, sortBy) = ParseSort(queryParams);
            queryParams.TryGetValue("key", out var keyStr);
            queryParams.TryGetValue("level", out var levelStr);
            queryParams.TryGetValue("search", out var searchStr);

            return (currentPage, currentCount, keyStr, levelStr, searchStr, outputStartDate, outputEndDate, sortOn, sortBy);
        }

        private static (int currentPage, int currentCount) ParseRequiredParams(IQueryCollection queryParams)
        {
            queryParams.TryGetValue("page", out var pageStr);
            queryParams.TryGetValue("count", out var countStr);
            var canPageBeParsed = int.TryParse(pageStr, out var currentPage);
            var canCountBeParsed = int.TryParse(countStr, out var currentCount);
            currentPage = !canPageBeParsed ? 1 : currentPage;
            currentCount = !canCountBeParsed ? 10 : currentCount;

            return (currentPage, currentCount);
        }

        private static (DateTime?, DateTime?) ParseDates(IQueryCollection queryParams)
        {
            queryParams.TryGetValue("startDate", out var startDateStar);
            queryParams.TryGetValue("endDate", out var endDateStar);

            DateTime.TryParse(startDateStar, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var startDate);
            DateTime.TryParse(endDateStar, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var endDate);
            var outputStartDate = startDate == default ? (DateTime?)null : startDate;
            var outputEndDate = endDate == default ? (DateTime?)null : endDate;

            return (outputStartDate, outputEndDate);
        }

        private static (SortProperty, SortDirection) ParseSort(IQueryCollection queryParams)
        {
            queryParams.TryGetValue("sortOn", out var sortStrOn);
            queryParams.TryGetValue("sortBy", out var sortStrBy);

            Enum.TryParse<SortProperty>(sortStrOn, out var sortProperty);
            Enum.TryParse<SortDirection>(sortStrBy, out var sortDirection);

            return (sortProperty, sortDirection);
        }

        /// <summary>
        /// Returns an industry standard ProblemDetails object.
        /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7807"/>
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Task OnError(HttpContext httpContext, Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/problem+json";

            var includeDetails = httpContext.Request.IsLocal();

            var title = includeDetails ? "An error occured: " + ex.Message : "An error occured";
            var details = includeDetails ? ex.ToString() : null;

            var problem = new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Title = title,
                Detail = details,
                Extensions =
                {
                    ["traceId"] = httpContext.TraceIdentifier
                }
            };

            var stream = httpContext.Response.Body;
            return JsonSerializer.SerializeAsync(stream, problem);
        }

        private void SetResponseContentType(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json;charset=utf-8";
        }
    }
}
