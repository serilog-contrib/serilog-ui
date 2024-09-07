using System;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.Core.Attributes;

/// <summary>
/// Used to identify a <see cref="AdditionalColumn"/> that can be rendered as code in the UI.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class CodeColumnAttribute(CodeType codeType) : Attribute
{
    /// <summary>
    /// Gets the CodeType.
    /// </summary>
    public CodeType CodeType { get; } = codeType;
}