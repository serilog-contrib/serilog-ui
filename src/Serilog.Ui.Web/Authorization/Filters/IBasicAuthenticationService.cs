using System.Net.Http.Headers;

namespace Serilog.Ui.Web.Authorization.Filters;

/// <summary>
/// Provides basic authentication services.
/// </summary>
public interface IBasicAuthenticationService
{
    /// <summary>
    /// Determines whether access is granted based on the provided basic authentication header.
    /// </summary>
    /// <param name="basicHeader">The basic authentication header containing the credentials.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether access is granted.</returns>
    Task<bool> CanAccessAsync(AuthenticationHeaderValue basicHeader);
}