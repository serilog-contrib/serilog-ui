namespace Serilog.Ui.Web.Endpoints;

/// <summary>
/// Provides application routes for Serilog UI and inherits methods to set options.
/// </summary>
public interface ISerilogUiAppRoutes : ISerilogUiOptionsSetter
{
    /// <summary>
    /// Gets or sets a value indicating whether access to the home route is blocked.
    /// </summary>
    protected internal bool BlockHomeAccess { get; set; }

    /// <summary>
    /// Asynchronously retrieves the home page.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task GetHomeAsync();

    /// <summary>
    /// Asynchronously redirects to the home page.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RedirectHomeAsync();
}