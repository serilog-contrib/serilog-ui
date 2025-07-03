namespace Serilog.Ui.Web.Endpoints;

/// <summary>
/// Provides endpoints for Serilog UI and inherits methods to set options.
/// </summary>
public interface ISerilogUiEndpoints : ISerilogUiOptionsSetter
{
    /// <summary>
    /// Asynchronously retrieves API keys.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task GetApiKeysAsync();

    /// <summary>
    /// Asynchronously retrieves logs.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task GetLogsAsync();

    /// <summary>
    /// Asynchronously retrieves dashboard statistics.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task GetDashboardAsync();
}