using Serilog.Ui.Core.Attributes;

namespace Serilog.Ui.Core.Models;

/// <summary>
/// AdditionalColumn class.
/// </summary>
public class AdditionalColumn
{
    /// <summary>
    /// Gets or sets the Name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the TypeName.
    /// </summary>
    public string TypeName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the CodeType.
    /// </summary>
    public CodeType? CodeType { get; set; }
}