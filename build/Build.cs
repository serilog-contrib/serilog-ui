using System.Diagnostics.CodeAnalysis;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.ProjectModel;
using Nuke.Common.Git;
using GlobExpressions;
using Nuke.Common.Utilities.Collections;

[SuppressMessage("Major Bug", "S3903:Types should be defined in named namespaces", Justification = "As per standard creation")]
partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Publish);//(x => x.Backend_Test, p => p.Frontend_Tests);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;


    [Solution] readonly Solution Solution;

    [GitRepository] readonly GitRepository Repository;

    static AbsolutePath FrontendWorkingDirectory => RootDirectory / "src/Serilog.Ui.Web";
    static AbsolutePath OutputDirectory => RootDirectory / "artifacts";
    static AbsolutePath SourceDirectory => RootDirectory / "src";
    static AbsolutePath TestsDirectory => RootDirectory / "tests";
    bool IsReleaseOrMasterBranch => Repository.IsOnReleaseBranch() || Repository.IsOnMainOrMasterBranch();

    Target Clean => _ => _
        .DependsOn(Backend_Clean, Frontend_Clean)
        .Executes(() => { });

    Target Pack => _ => _
    .DependsOn(Frontend_Build, Backend_Compile)
        //.DependsOn(Backend_SonarScan_End, Frontend_Tests_Ci)
        //.OnlyWhenStatic(() => IsReleaseOrMasterBranch)
        .Executes(() =>
        {
            DotNetTasks.DotNetPack(new DotNetPackSettings()
                .SetProject(Solution.GetProject("Serilog.Ui.Web"))
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetOutputDirectory(OutputDirectory));
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            OutputDirectory.GlobFiles("*.nupkg")
            .ForEach(x =>
            {
                DotNetTasks.DotNetNuGetPush(s => s
                    .SetTargetPath(x)
                    .SetSource(RootDirectory / "produced-nugets"));
                //.SetSource("https://api.nuget.org/v3/index.json")
                //.SetApiKey("TODO"));
            });
        });
}
