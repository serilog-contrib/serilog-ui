using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.SonarScanner;
using System.Collections.Generic;

[GitHubActions("DotNET-build",
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = true,
    EnableGitHubToken = true,
    ImportSecrets = new[] { nameof(DockerhubUsername), nameof(DockerhubPassword), nameof(SonarToken) },
    InvokedTargets = new[] { nameof(Backend_Reporter) },
    OnPushBranches = new[] { "master", "dev" }
)]
[GitHubActions("JS-build",
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = true,
    EnableGitHubToken = true,
    ImportSecrets = new[] { nameof(SonarTokenUi) },
    InvokedTargets = new[] { nameof(Frontend_Reporter) },
    OnPushBranches = new[] { "master", "dev" },
    OnPullRequestBranches = new[] { "master", "dev" }
)]
partial class Build : NukeBuild
{
    [PathExecutable("dotnet-sonarscanner")]
    readonly Tool SonarScanner;

    [Parameter][Secret] readonly string DockerhubUsername;
    [Parameter][Secret] readonly string DockerhubPassword;
    [Parameter][Secret] readonly string SonarToken;
    [Parameter][Secret] readonly string SonarTokenUi;

    public bool OnGithubActionRun = GitHubActions.Instance != null &&
            !string.IsNullOrWhiteSpace(GitHubActions.Instance.RunId.ToString());

    Target Backend_Setup => _ => _
        .OnlyWhenStatic(() => OnGithubActionRun)
        .Executes(() =>
        {
            DotNetTasks.DotNetToolInstall(new DotNetToolInstallSettings()
                .SetPackageName("dotnet-sonarscanner")
                .SetGlobal(true)
            );
            DotNetTasks.DotNetToolInstall(new DotNetToolInstallSettings()
                .SetPackageName("dotnet-coverage")
                .SetGlobal(true)
            );
        });

    Target Docker_Setup => _ => _
        .DependsOn(Backend_Setup)
        .OnlyWhenStatic(() => OnGithubActionRun)
        .Executes(() =>
        {
            DockerTasks.DockerLogin(new DockerLoginSettings()
                .SetUsername(DockerhubUsername)
                .SetPassword(DockerhubPassword));
        });

    Target Backend_SonarScan_Start => _ => _
        .DependsOn(Backend_Restore)
        .OnlyWhenStatic(() => OnGithubActionRun)
        .Executes(() =>
        {
            SonarScanner(@$"dotnet sonarscanner begin \
                /k:""followynne_serilog-ui\"" \
                /o:""followynne"" \
                /d:sonar.login=""{SonarToken}"" \
                /d:sonar.host.url=""https://sonarcloud.io"" \  
                /d:sonar.sources=src/ \
                /d:sonar.exclusions=src/Serilog.Ui.Web/assets/**/*,src/Serilog.Ui.Web/wwwroot/**/*,src/Serilog.Ui.Web/node_modules/**/*,src/Serilog.Ui.Web/*.js,src/Serilog.Ui.Web/*.json \
                /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml",
                environmentVariables: new Dictionary<string, string> { ["GITHUB_TOKEN"] = GitHubActions.Instance.Token, ["SONAR_TOKEN"] = SonarToken });
        });

    Target Backend_SonarScan_End => _ => _
    .DependsOn(Backend_Test_Ci)
    .OnlyWhenStatic(() => OnGithubActionRun)
    .Executes(() =>
    {
        SonarScanner($"dotnet sonarscanner end /d:sonar.login=\"{SonarToken}\"",
            environmentVariables: new Dictionary<string, string> { ["GITHUB_TOKEN"] = GitHubActions.Instance.Token, ["SONAR_TOKEN"] = SonarToken });
    });

    Target Frontend_SonarScan => _ => _
        .DependsOn(Frontend_Tests_Ci)
        .OnlyWhenStatic(() => OnGithubActionRun)
        .Executes(() =>
        {
            // TODO: Action
        });

    Target Frontend_Reporter => _ => _
        .DependsOn(Frontend_SonarScan)
        .OnlyWhenStatic(() => OnGithubActionRun)
        .Executes(() =>
        {
            // TODO: action
        });

    Target Backend_Reporter => _ => _
        .DependsOn(Backend_SonarScan_End)
        .OnlyWhenStatic(() => OnGithubActionRun)
        .Executes(() =>
        {
            // TODO: action
        });
}