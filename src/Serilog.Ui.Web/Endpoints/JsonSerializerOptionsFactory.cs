namespace Serilog.Ui.Web.Endpoints;

internal static class JsonSerializerOptionsFactory
{
    public static JsonSerializerOptions GetDefaultOptions => new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}