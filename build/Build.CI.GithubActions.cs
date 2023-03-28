using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.SonarScanner;
using static CustomGithubActionsAttribute;

/**
 * Interesting ref to make the build script executable on server:
 * https://blog.dangl.me/archive/executing-nuke-build-scripts-on-linux-machines-with-correct-file-permissions/
 * https://stackoverflow.com/a/40979016/15129749
 */
[CustomGithubActions("DotNET-build",
    GitHubActionsImage.UbuntuLatest,
    AddGithubActions = new[] { GithubAction.BackendReporter },
    AutoGenerate = true,
    EnableGitHubToken = true,
    FetchDepth = 0,
    ImportSecrets = new[] { nameof(SonarToken) },
    InvokedTargets = new[] { nameof(Backend_SonarScan_End) },
    OnPushBranches = new[] { "master", "dev" },
    OnPullRequestBranches = new[] { "master", "dev" }
)]
[CustomGithubActions("JS-build",
    GitHubActionsImage.UbuntuLatest,
    AddGithubActions = new[] { GithubAction.SonarScanTask, GithubAction.FrontendReporter },
    AutoGenerate = true,
    EnableGitHubToken = true,
    FetchDepth = 0,
    ImportSecrets = new[] { nameof(SonarTokenUi) },
    InvokedTargets = new[] { nameof(Frontend_Tests_Ci) },
    OnPushBranches = new[] { "master", "dev" },
    OnPullRequestBranches = new[] { "master", "dev" }
)]
[CustomGithubActions("Release",
    GitHubActionsImage.UbuntuLatest,
    AddGithubActions = new[] { GithubAction.BackendReporter, GithubAction.SonarScanTask, GithubAction.FrontendReporter },
    AutoGenerate = true,
    EnableGitHubToken = true,
    FetchDepth = 0,
    ImportSecrets = new[] { nameof(SonarTokenUi), nameof(SonarToken) },
    InvokedTargets = new[] { nameof(Publish) },
    OnWorkflowDispatchRequiredInputs = new[] { nameof(IncludeMsSql) }
)]
partial class Build : NukeBuild
{
    [Parameter][Secret] readonly string SonarToken;
    [Parameter][Secret] readonly string SonarTokenUi;
    [Parameter] readonly string IncludeMsSql;

    public bool OnGithubActionRun = GitHubActions.Instance != null &&
            !string.IsNullOrWhiteSpace(GitHubActions.Instance.RunId.ToString()); 

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
}