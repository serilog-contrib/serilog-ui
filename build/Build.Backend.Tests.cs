using System.Linq;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    [PackageExecutable(
        packageId: "dotnet-coverage",
        packageExecutable: "dotnet-coverage.dll"
    )]
    readonly Tool DotnetCoverage;

    Target Backend_Test => _ => _
        .DependsOn(Backend_Compile)
        .Requires(() => DockerTasks.DockerInfo(new DockerInfoSettings()).Any())
        .Description("Runs dotnet test")
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetNoBuild(true));
        });

    Target Backend_Test_Ci => _ => _
        .DependsOn(Backend_Compile)
        .Requires(() => DockerTasks.DockerInfo(new DockerInfoSettings()).Any())
        .Description("Runs dotnet-coverage collect, with coverlet coverage")
        .Executes(() =>
        {
            DotnetCoverage?.Invoke(
                @"collect -f xml -o ""coverage.xml"" dotnet test --configuration Release --no-build --logger=""trx;LogFileName=test-results.trx""");
        });
}