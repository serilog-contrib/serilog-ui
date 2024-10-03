using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Endpoints;

/// <summary>
/// Provides methods to set Serilog UI options.
/// </summary>
public interface ISerilogUiOptionsSetter
{
    /// <summary>
    /// Gets the current UI options.
    /// </summary>
    UiOptions? Options { get; }

    /// <summary>
    /// Sets the UI options.
    /// </summary>
    /// <param name="options">The UI options to set.</param>
    void SetOptions(UiOptions options);
}