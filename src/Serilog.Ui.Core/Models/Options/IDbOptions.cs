namespace Serilog.Ui.Core.Models.Options;

/// <summary>
/// IDbOptions interface.
/// </summary>
public interface IDbOptions
{
    /// <summary>
    /// Validate the options after the configuration.
    /// </summary>
    void Validate();
}