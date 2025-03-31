using System;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    [NuGetPackage(
        packageId: "dotnet-coverage",
        packageExecutable: "dotnet-coverage.dll"
    )]
    readonly Tool? DotnetCoverage;

    [NuGetPackage(
        packageId: "dotnet-reportgenerator-globaltool",
        packageExecutable: "ReportGenerator.dll"
    )]
    readonly Tool? DotnetReport;

    Target Backend_Test => targetDefinition => targetDefinition
        .DependsOn(Backend_Compile)
        .Requires(() => DockerTasks.DockerInfo(new DockerInfoSettings()).Count != 0)
        .Description("Runs dotnet test")
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetNoBuild(true));
        });

    Target Backend_Test_Ci => targetDefinition => targetDefinition
        .DependsOn(Backend_Compile)
        .Requires(() => DockerTasks.DockerInfo(new DockerInfoSettings()).Count != 0)
        .Description("Runs dotnet-coverage collect, with coverlet coverage")
        .Executes(() =>
        {
            var user = Environment.GetEnvironmentVariable("USER") ?? Environment.GetEnvironmentVariable("USERNAME");

            // due to [ref](https://github.com/Mongo2Go/Mongo2Go/issues/144)
            ProcessTasks
                .StartProcess("sudo", $"chown -R {user}:{user} /home/runneradmin")
                .AssertZeroExitCode();
            // encoded spaces [ref](https://github.com/microsoft/azure-pipelines-tasks/issues/18731#issuecomment-1689118779)
            DotnetCoverage?.Invoke(
                "collect -f xml -o coverage.xml dotnet test --configuration=Release --no-build --collect=XPlat%20Code%20Coverage;Format=cobertura --logger=trx;LogFileName=test-results.trx");
        });

    Target Backend_Report_Ci => targetDefinition => targetDefinition
        .DependsOn(Backend_Test_Ci)
        .Requires(() => DockerTasks.DockerInfo(new DockerInfoSettings()).Count != 0)
        .Description("Runs report-generator Sonarqube reports")
        .Executes(() =>
        {
            DotnetReport?.Invoke("-reports:**/coverage.cobertura.xml -reporttypes:SonarQube -targetdir:coverage");
        });
}