using System;
using Serilog.Ui.Core.Models;

namespace Serilog.Ui.Core.Attributes;

/// <summary>
/// Used to identify a <see cref="LogModel"/> column that is removed from the provider.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class RemovedColumnAttribute : Attribute;
