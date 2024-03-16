using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Web.Authorization;

public class BasicAuthenticationFilter(IHttpContextAccessor httpContextAccessor) : IUiAuthorizationFilter
{
    private const string AuthenticationScheme = "Basic";
    internal const string AuthenticationCookieName = "SerilogAuth";

    public string UserName { get; set; }

    public string Password { get; set; }

    public bool Authorize()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null) return false;

        var header = httpContext.Request.Headers["Authorization"];
        var isAuthenticated = false;

        if (header == "null" || string.IsNullOrEmpty(header))
        {
            var authCookie = httpContext.Request.Cookies[AuthenticationCookieName];
            if (!string.IsNullOrWhiteSpace(authCookie))
            {
                var hashedCredentials = EncryptCredentials(UserName, Password);
                isAuthenticated = authCookie.Equals(hashedCredentials, StringComparison.OrdinalIgnoreCase);
            }
        }
        else
        {
            var authValues = AuthenticationHeaderValue.Parse(header);

            if (IsBasicAuthentication(authValues))
            {
                var tokens = ExtractAuthenticationTokens(authValues);

                if (CredentialsMatch(tokens))
                {
                    isAuthenticated = true;
                    var hashedCredentials = EncryptCredentials(UserName, Password);
                    httpContext.Response.Cookies.Append(AuthenticationCookieName, hashedCredentials);
                }
            }
        }

        if (!isAuthenticated)
        {
            SetChallengeResponse(httpContext);
        }

        return isAuthenticated;
    }

    private string EncryptCredentials(string user, string pass)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{user}:{pass}"));
        var hashedCredentials = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        return hashedCredentials;
    }

    private static bool IsBasicAuthentication(AuthenticationHeaderValue authValues)
    {
        return AuthenticationScheme.Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase);
    }

    private static (string, string) ExtractAuthenticationTokens(AuthenticationHeaderValue authValues)
    {
        var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
        var parts = parameter.Split(':');
        return (parts[0], parts[1]);
    }

    private bool CredentialsMatch((string Username, string Password) tokens)
    {
        return tokens.Username == UserName && tokens.Password == Password;
    }

    private void SetChallengeResponse(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = 401;
        httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Serilog UI\"");
        httpContext.Response.WriteAsync("Authentication is required.");
    }
}