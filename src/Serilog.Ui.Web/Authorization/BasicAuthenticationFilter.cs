using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Serilog.Ui.Web.Authorization;

public class BasicAuthenticationFilter : IUiAuthorizationFilter
{
    public string User { get; set; }
    public string Pass { get; set; }

    private const string AuthenticationScheme = "Basic";
    private const string AuthenticationCookieName = "SerilogAuth";

    public bool Authorize(HttpContext httpContext)
    {
        var header = httpContext.Request.Headers["Authorization"];
        var isAuthenticated = false;

        if (header == "null" || string.IsNullOrEmpty(header))
        {
            var authCookie = httpContext.Request.Cookies[AuthenticationCookieName];
            if (!string.IsNullOrWhiteSpace(authCookie))
            {
                var hashedCredentials = EncryptCredentials(User, Pass);
                isAuthenticated = string.Equals(authCookie, hashedCredentials, StringComparison.OrdinalIgnoreCase);
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
                    var hashedCredentials = EncryptCredentials(User, Pass);
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

    public string EncryptCredentials(string user, string pass)
    {
        var sha256 = SHA256.Create();
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
        return tokens.Username == User && tokens.Password == Pass;
    }

    private void SetChallengeResponse(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = 401;
        httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"SeriLog Ui\"");
        httpContext.Response.WriteAsync("Authentication is required.");
    }
}