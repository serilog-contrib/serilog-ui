﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Serilog.Ui.Core;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Serilog.Ui.Web.Endpoints
{
    internal class SerilogUiEndpoints : ISerilogUiEndpoints
    {
        private readonly ILogger<SerilogUiEndpoints> _logger;
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private string[] _providerKeys;

        public SerilogUiEndpoints(ILogger<SerilogUiEndpoints> logger)
        {
            _logger = logger;
        }

        public UiOptions Options { get; private set; }

        public async Task GetApiKeysAsync(HttpContext httpContext)
        {
            try
            {
                httpContext.Response.ContentType = "application/json;charset=utf-8";
                if (_providerKeys == null)
                {
                    var aggregateDataProvider = httpContext.RequestServices.GetRequiredService<AggregateDataProvider>();
                    _providerKeys = aggregateDataProvider.Keys.ToArray();
                }

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
                httpContext.Response.ContentType = "application/json;charset=utf-8";

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

        private static async Task<string> FetchLogsAsync(HttpContext httpContext)
        {
            var (currentPage, count, dbKey, level, textSearch, start, end) = ParseQuery(httpContext.Request.Query);

            var provider = httpContext.RequestServices.GetService<AggregateDataProvider>();

            if (!string.IsNullOrWhiteSpace(dbKey))
            {
                provider.SwitchToProvider(dbKey);
            }

            var (logs, total) = await provider.FetchDataAsync(currentPage, count, level, textSearch, start, end);

            var result = JsonSerializer.Serialize(new { logs, total, count, currentPage }, JsonSerializerOptions);

            return result;
        }

        private static (int currPage, int count, string dbKey, string level, string textSearch, DateTime? start, DateTime? end) ParseQuery(IQueryCollection queryParams)
        {
            queryParams.TryGetValue("page", out var pageStr);
            queryParams.TryGetValue("count", out var countStr);
            queryParams.TryGetValue("level", out var levelStr);
            queryParams.TryGetValue("search", out var searchStr);
            queryParams.TryGetValue("startDate", out var startDateStar);
            queryParams.TryGetValue("endDate", out var endDateStar);
            queryParams.TryGetValue("key", out var keyStr);

            var canPageBeParsed = int.TryParse(pageStr, out var currentPage);
            var canCountBeParsed = int.TryParse(countStr, out var currentCount);
            currentPage = !canPageBeParsed ? 1 : currentPage;
            currentCount = !canCountBeParsed ? 10 : currentCount;

            DateTime.TryParse(startDateStar, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var startDate);
            DateTime.TryParse(endDateStar, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var endDate);
            var outputStartDate = startDate == default ? (DateTime?)null : startDate;
            var outputEndDate = endDate == default ? (DateTime?)null : endDate;

            return (currentPage, currentCount, keyStr, levelStr, searchStr, outputStartDate, outputEndDate);
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

            httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
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
    }
}
