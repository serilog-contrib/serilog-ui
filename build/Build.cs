using System.Diagnostics.CodeAnalysis;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

[SuppressMessage("Major Bug", "S3903:Types should be defined in named namespaces", Justification = "As per standard creation")]
partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Backend_Test, p => p.Frontend_Tests);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    static AbsolutePath FrontendWorkingDirectory => RootDirectory / "src/Serilog.Ui.Web";
    static AbsolutePath SourceDirectory => RootDirectory / "src";
    static AbsolutePath SourceFrontendDirectory => RootDirectory / "src/Serilog.Ui.Web/assets";
    static AbsolutePath TestsDirectory => RootDirectory / "tests";
    static AbsolutePath TestsFrontendDirectory => RootDirectory / "src/Serilog.Ui.Web/__tests__";

    Target Clean => _ => _
        .DependsOn(Backend_Clean, Frontend_Clean)
        .Executes(() => { });
}
