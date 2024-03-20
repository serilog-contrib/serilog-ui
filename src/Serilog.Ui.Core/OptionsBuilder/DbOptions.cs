namespace Serilog.Ui.Core.OptionsBuilder;

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