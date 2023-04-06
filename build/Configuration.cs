using System.ComponentModel;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Configuration>))]
internal class Configuration : Enumeration
{
    public readonly static Configuration Debug = new() { Value = nameof(Debug) };
    public readonly static Configuration Release = new() { Value = nameof(Release) };

    public static implicit operator string(Configuration configuration)
    {
        return configuration.Value;
    }
}

/// <summary>
/// TODO: set the 3 values when SonarCloud project is created.
/// </summary>
internal static class SonarCloudInfo
{
    internal const string Organization = "followynne";
    internal const string BackendProjectKey = "followynne_serilog-ui";
    internal const string FrontendProjectKey = "followynne_serilog-ui_assets";
}
