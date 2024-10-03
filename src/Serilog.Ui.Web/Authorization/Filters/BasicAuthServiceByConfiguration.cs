using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Serilog.Ui.Web.Authorization.Filters;

internal class BasicAuthServiceByConfiguration(IConfiguration configuration) : IBasicAuthenticationService
{
    private readonly string? _userName = configuration["SerilogUi:UserName"];
    private readonly string? _password = configuration["SerilogUi:Password"];

    public Task<bool> CanAccessAsync(AuthenticationHeaderValue basicHeader)
    {
        var header = basicHeader.Parameter;

        return Task.FromResult(EvaluateAuthResult(header));
    }

    private bool EvaluateAuthResult(string? header)
    {
        var (userName, password) = ExtractAuthenticationTokens(header);
        return userName == _userName && password == _password;
    }

    private static (string userName, string password) ExtractAuthenticationTokens(string? authValues)
    {
        var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues ?? string.Empty));
        var parts = parameter.Split(':');
        return (parts[0], parts[1]);
    }
}