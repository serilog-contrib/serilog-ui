using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Serilog.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Web.Models;
using Serilog.Ui.Web.Extensions;

namespace Serilog.Ui.Web.Endpoints
{
    internal class SerilogUiEndpoints : ISerilogUiEndpoints
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<SerilogUiEndpoints> _logger;

        private readonly AggregateDataProvider _aggregateDataProvider;

        public SerilogUiEndpoints(IHttpContextAccessor httpContextAccessor,
            ILogger<SerilogUiEndpoints> logger,
            AggregateDataProvider aggregateDataProvider)
        {
            _httpContextAccessor = httpContextAccessor;
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

        public async Task GetApiKeysAsync()
        {
            var httpContext = Guard.Against.Null(_httpContextAccessor.HttpContext);

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

        public async Task GetLogsAsync()
        {
            var httpContext = Guard.Against.Null(_httpContextAccessor.HttpContext);

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
            var queryDictionary = QueryHelpers.ParseQuery(httpContext.Request.QueryString.Value);
            var queryLogs = FetchLogsQuery.ParseQuery(queryDictionary);

            if (!string.IsNullOrWhiteSpace(queryLogs.DatabaseKey))
            {
                _aggregateDataProvider.SwitchToProvider(queryLogs.DatabaseKey);
            }

            var (logs, total) = await _aggregateDataProvider.FetchDataAsync(queryLogs);
            
            // due to System.Text.Json design choice:
            // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/polymorphism
            var serializeLogs = logs.ToList().Cast<object>();
            
            var result = JsonSerializer.Serialize(new { logs = serializeLogs, total, count = queryLogs.Count, currentPage = queryLogs.CurrentPage }, JsonSerializerOptions);

            return result;
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

        private static void SetResponseContentType(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json;charset=utf-8";
        }
    }
}