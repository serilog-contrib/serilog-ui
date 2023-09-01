using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Ui.Core;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    internal class SerilogUiEndpoints : ISerilogUiEndpoints
    {
        private readonly ILogger<SerilogUiEndpoints> _logger;
        private readonly JsonSerializerSettings _jsonSerializerOptions;
        private string[] _providerKeys;

        public SerilogUiEndpoints(ILogger<SerilogUiEndpoints> logger)
        {
            _logger = logger;
            _jsonSerializerOptions = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
        }

        public UiOptions Options { get; private set; }

        public async Task GetApiKeys(HttpContext httpContext)
        {
            try
            {
                httpContext.Response.ContentType = "application/json;charset=utf-8";
                if (_providerKeys == null)
                {
                    var aggregateDataProvider = httpContext.RequestServices.GetRequiredService<AggregateDataProvider>();
                    _providerKeys = aggregateDataProvider.Keys.ToArray();
                }

                var result = JsonConvert.SerializeObject(_providerKeys);
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await httpContext.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                await OnError(httpContext, ex);
            }
        }

        public async Task GetLogs(HttpContext httpContext)
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

        private async Task<string> FetchLogsAsync(HttpContext httpContext)
        {
            var (currentPage, count, dbKey, level, textSearch, start, end) = ParseQuery(httpContext.Request.Query);

            var provider = httpContext.RequestServices.GetService<AggregateDataProvider>();

            string key = dbKey;
            if (!string.IsNullOrWhiteSpace(key))
            {
                provider.SwitchToProvider(key);
            }

            var (logs, total) = await provider.FetchDataAsync(currentPage, count, level, textSearch, start, end);

            var result = JsonConvert.SerializeObject(new { logs, total, count, currentPage }, _jsonSerializerOptions);

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

        private Task OnError(HttpContext httpContext, Exception ex)
        {
            _logger.LogError(ex, "@Message", ex.Message);
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorMessage = httpContext.Request.IsLocal()
                ? JsonConvert.SerializeObject(new { errorMessage = ex.Message })
                : JsonConvert.SerializeObject(new { errorMessage = "Internal server error" });

            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { errorMessage }));
        }
    }
}
