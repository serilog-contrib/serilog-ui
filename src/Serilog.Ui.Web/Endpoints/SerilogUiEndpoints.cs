using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Web.Extensions;
using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Endpoints;

internal class SerilogUiEndpoints(
    IHttpContextAccessor httpContextAccessor,
    ILogger<SerilogUiEndpoints> logger,
    AggregateDataProvider aggregateDataProvider
    ) : ISerilogUiEndpoints
{
    private readonly HttpContext _httpContext = Guard.Against.Null(httpContextAccessor.HttpContext);
    private readonly string[] _providerKeys = aggregateDataProvider.Keys.ToArray();

    public UiOptions? Options { get; private set; }

    public void SetOptions(UiOptions options)
    {
        Options = options;
    }

    public async Task GetApiKeysAsync()
    {
        try
        {
            SetResponseContentType();

            string result = JsonSerializer.Serialize(_providerKeys, JsonSerializerOptionsFactory.GetDefaultOptions);
            _httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            await _httpContext.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            await OnError(ex);
        }
    }

    public async Task GetLogsAsync()
    {
        try
        {
            SetResponseContentType();

            string result = await FetchLogsAsync();
            _httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            await _httpContext.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            await OnError(ex);
        }
    }

    private async Task<string> FetchLogsAsync()
    {
        Dictionary<string, StringValues> queryDictionary = QueryHelpers.ParseQuery(_httpContext.Request.QueryString.Value);
        FetchLogsQuery queryLogs = FetchLogsQuery.ParseQuery(queryDictionary);

        if (!string.IsNullOrWhiteSpace(queryLogs.DatabaseKey))
        {
            aggregateDataProvider.SwitchToProvider(queryLogs.DatabaseKey);
        }

        (IEnumerable<LogModel> logs, int total) = await aggregateDataProvider.FetchDataAsync(queryLogs);

        // due to System.Text.Json design choice:
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/polymorphism
        IEnumerable<object> serializeLogs = logs.ToList();

        string result = JsonSerializer.Serialize(new
        {
            logs = serializeLogs,
            total,
            count = queryLogs.Count,
            currentPage = queryLogs.CurrentPage
        }, JsonSerializerOptionsFactory.GetDefaultOptions);

        return result;
    }

    /// <summary>
    /// Returns an industry standard ProblemDetails object.
    /// See: <see href="https://datatracker.ietf.org/doc/html/rfc7807"/>
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    private Task OnError(Exception ex)
    {
        logger.LogError(ex, "{Message}", ex.Message);

        _httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        _httpContext.Response.ContentType = "application/problem+json";

        bool includeDetails = _httpContext.Request.IsLocal();

        ProblemDetails problem = new()
        {
            Status = _httpContext.Response.StatusCode,
            Title = includeDetails ? $"An error occured: {ex.Message}" : "An error occured",
            Detail = includeDetails ? ex.ToString() : null,
            Extensions =
            {
                ["traceId"] = _httpContext.TraceIdentifier
            }
        };

        Stream stream = _httpContext.Response.Body;

        return JsonSerializer.SerializeAsync(stream, problem);
    }

    private void SetResponseContentType()
    {
        _httpContext.Response.ContentType = "application/json;charset=utf-8";
    }
}