using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.SonarScanner;
using System.Collections.Generic;

/**
 * Interesting ref to make the build script executable on server:
 * https://blog.dangl.me/archive/executing-nuke-build-scripts-on-linux-machines-with-correct-file-permissions/
 * https://stackoverflow.com/a/40979016/15129749
 */
[GitHubActions("DotNET-build",
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = true,
    EnableGitHubToken = true,
    FetchDepth = 0,
    ImportSecrets = new[] { nameof(DockerhubUsername), nameof(DockerhubPassword), nameof(SonarToken) },
    InvokedTargets = new[] { nameof(Backend_Reporter) },
    OnPushBranches = new[] { "master", "dev" },
    OnPullRequestBranches = new[] { "master", "dev" }
)]
[GitHubActions("JS-build",
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = true,
    EnableGitHubToken = true,
    FetchDepth = 0,
    ImportSecrets = new[] { nameof(SonarTokenUi) },
    InvokedTargets = new[] { nameof(Frontend_Reporter) },
    OnPushBranches = new[] { "master", "dev" },
    OnPullRequestBranches = new[] { "master", "dev" }
)]
partial class Build : NukeBuild
{
    //[PackageExecutable(
    //    packageId: "dotnet-sonarscanner",
    //    packageExecutable: "SonarScanner.MSBuild.dll",
    //    // Must be set for tools shipping multiple versions
    //    Framework = "net5.0"
    //)]
    //readonly Tool SonarScanner;

    [Parameter][Secret] readonly string DockerhubUsername;
    [Parameter][Secret] readonly string DockerhubPassword;
    [Parameter][Secret] readonly string SonarToken;
    [Parameter][Secret] readonly string SonarTokenUi;

    public bool OnGithubActionRun = GitHubActions.Instance != null &&
            !string.IsNullOrWhiteSpace(GitHubActions.Instance.RunId.ToString());

    Target Docker_Setup => _ => _
        .OnlyWhenStatic(() => OnGithubActionRun)
        .Executes(() =>
        {
            //DockerTasks.DockerLogin(new DockerLoginSettings()
            //    .SetUsername(DockerhubUsername)
            //    .SetPassword(DockerhubPassword));
        });

    Target Backend_SonarScan_Start => _ => _
        .DependsOn(Backend_Restore)
        .OnlyWhenStatic(() => OnGithubActionRun)
        .Executes(() =>
        {
            SonarScannerTasks.SonarScannerBegin(new SonarScannerBeginSettings()
                .SetFramework("net5.0")
                .SetProjectKey("followynne_serilog-ui")
                .SetOrganization("followynne")
                .SetLogin(SonarToken)
                .SetServer("https://sonarcloud.io")
                .SetVisualStudioCoveragePaths("coverage.xml")
                .SetSourceInclusions("src/")
                .SetExcludeTestProjects(true)
                .SetSourceExclusions(
                    "src/Serilog.Ui.Web/assets/**/*",
                    "src/Serilog.Ui.Web/wwwroot/**/*",
                    "src/Serilog.Ui.Web/node_modules/**/*",
                    "src/Serilog.Ui.Web/*.js",
                    "src/Serilog.Ui.Web/*.json")
                .SetProcessEnvironmentVariable("GITHUB_TOKEN", GitHubActions.Instance.Token)
                .SetProcessEnvironmentVariable("SONAR_TOKEN", SonarToken)
            );
        });

    Target Backend_SonarScan_End => _ => _
        .DependsOn(Backend_Test_Ci)
        .OnlyWhenStatic(() => OnGithubActionRun)
        .Executes(() =>
        {
            SonarScannerTasks.SonarScannerEnd(new SonarScannerEndSettings()
                .SetFramework("net5.0")
                .SetLogin(SonarToken)
                .SetProcessEnvironmentVariable("GITHUB_TOKEN", GitHubActions.Instance.Token)
                .SetProcessEnvironmentVariable("SONAR_TOKEN", SonarToken));            
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