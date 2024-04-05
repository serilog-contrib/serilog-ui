using System;

namespace Serilog.Ui.Core.OptionsBuilder;

/// <summary>
/// Used to identify a <see cref="AdditionalColumn"/> that can be rendered as code in the UI.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class CodeColumnAttribute(CodeType codeType) : Attribute
{
    /// <summary>
    /// Gets the CodeType.
    /// </summary>
    public readonly CodeType CodeType = codeType;
}

/// <summary>
/// CodeType enum.
/// </summary>
public enum CodeType
{
    /// <summary>
    /// Default value
    /// </summary>
    None,

    /// <summary>
    /// Xml code type
    /// </summary>
    Xml,

    /// <summary>
    /// Json code type
    /// </summary>
    Json
}