using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Configuration>))]
[SuppressMessage("Major Bug", "S3903:Types should be defined in named namespaces", Justification = "As per standard creation")]
internal class Configuration : Enumeration
{
    public readonly static Configuration Debug = new() { Value = nameof(Debug) };
    public readonly static Configuration Release = new() { Value = nameof(Release) };

    public static implicit operator string(Configuration configuration)
    {
        return configuration.Value;
    }
}
