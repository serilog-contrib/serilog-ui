namespace Serilog.Ui.Web.Authorization;

/// <summary>
/// Provides services for authorization filtering.
/// </summary>
internal interface IAuthorizationFilterService
{
    /// <summary>
    /// Checks access and executes the appropriate callback based on the result.
    /// </summary>
    /// <param name="onSuccess">The callback to execute if access is granted.</param>
    /// <param name="onFailure">The optional callback to execute if access is denied.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CheckAccessAsync(Func<Task> onSuccess, Func<Task>? onFailure = null);
}