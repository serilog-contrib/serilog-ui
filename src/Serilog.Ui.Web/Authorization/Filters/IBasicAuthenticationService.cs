using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Serilog.Ui.Web.Authorization.Filters;

public interface IBasicAuthenticationService
{
    Task<bool> CanAccessAsync(AuthenticationHeaderValue basicHeader);
}

internal class BasicAuthServiceByConfiguration(IConfiguration configuration) : IBasicAuthenticationService
{
    private string? UserName { get; } = configuration["SerilogUi:UserName"];

    private string? Password { get; } = configuration["SerilogUi:Password"];

    public Task<bool> CanAccessAsync(AuthenticationHeaderValue basicHeader)
    {
        var header = basicHeader.Parameter;

        return Task.FromResult(EvaluateAuthResult(header));
    }

    private bool EvaluateAuthResult(string? header)
    {
        var tokens = ExtractAuthenticationTokens(header);
        var matchCredentials = CredentialsMatch(tokens);

        return matchCredentials;
    }

    private static (string, string) ExtractAuthenticationTokens(string? authValues)
    {
        var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues ?? string.Empty));
        var parts = parameter.Split(':');
        return (parts[0], parts[1]);
    }

    private bool CredentialsMatch((string Username, string Password) tokens)
    {
        return tokens.Username == UserName && tokens.Password == Password;
    }
}