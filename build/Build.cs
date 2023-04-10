using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.ProjectModel;
using Nuke.Common.Git;
using GlobExpressions;
using Nuke.Common.Utilities.Collections;
using System.Linq;

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

    [GitRepository] readonly GitRepository Repository;

    static AbsolutePath FrontendWorkingDirectory => RootDirectory / "src/Serilog.Ui.Web";
    static AbsolutePath OutputDirectory => RootDirectory / "artifacts";
    static AbsolutePath SourceDirectory => RootDirectory / "src";
    static AbsolutePath TestsDirectory => RootDirectory / "tests";
    bool IsReleaseOrMasterBranch => Repository.IsOnReleaseBranch() || Repository.IsOnMainOrMasterBranch();

    Target Clean => _ => _
        .DependsOn(Backend_Clean, Frontend_Clean)
        .Executes(() =>
        {
            Serilog.Log.Information("--- Clean operations completed ---");
        });

    Target Pack => _ => _
        .DependsOn(Backend_SonarScan_End, Frontend_Tests_Ci)
        .OnlyWhenStatic(() => IsReleaseOrMasterBranch)
        .Executes(() =>
        {
            Serilog.Log.Information("Received infos: {@Key}", ReleaseInfos());

            foreach (var prj in ReleaseInfos().Where(prj => prj.Publish()))
            {
                Serilog.Log.Information("Packing prj: {@Key}", prj.Key);

                DotNetTasks.DotNetPack(new DotNetPackSettings()
                    .SetDescription(prj.Key)
                    .SetProject(Solution.GetProject(prj.Project))
                    .SetConfiguration(Configuration)
                    .SetConfiguration(Configuration)
                    .EnableNoBuild()
                    .EnableNoRestore()
                    .SetOutputDirectory(OutputDirectory));
            }
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .OnlyWhenStatic(() => IsReleaseOrMasterBranch)
        .Executes(() =>
        {
            var localOutput = RootDirectory / "nugets-output";
            if (IsLocalBuild) FileSystemTasks.EnsureExistingDirectory(localOutput);

            OutputDirectory.GlobFiles("*.nupkg")
            .ForEach(filePath =>
            {
                DotNetTasks.DotNetNuGetPush(settings => settings
                    .SetTargetPath(filePath)
                    .SetSource(IsLocalBuild ?
                        localOutput : "https://api.nuget.org/v3/index.json")
                        .SetApiKey(NugetApiKey)
                    );
            });
        });
}