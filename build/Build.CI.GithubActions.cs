using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.SonarScanner;
using Serilog;
using static CustomGithubActionsAttribute;

/**
 * Interesting ref to make the build script executable on server:
 * https://blog.dangl.me/archive/executing-nuke-build-scripts-on-linux-machines-with-correct-file-permissions/
 * https://stackoverflow.com/a/40979016/15129749
 */
[CustomGithubActions("DotNET-build",
    GitHubActionsImage.UbuntuLatest,
    AddGithubActions = new[] { GithubAction.Backend_Artifact },
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
    AddGithubActions = new[] { GithubAction.Frontend_SonarScan_Task, GithubAction.Frontend_Artifact },
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
    AddGithubActions = new[] { GithubAction.Backend_Reporter, GithubAction.Frontend_SonarScan_Task, GithubAction.Frontend_Reporter },
    AutoGenerate = true,
    EnableGitHubToken = true,
    FetchDepth = 0,
    ImportSecrets = new[] { nameof(SonarTokenUi), nameof(SonarToken), nameof(NugetApiKey) },
    InvokedTargets = new[] { nameof(Publish) },
    OnWorkflowDispatchRequiredInputs = new[]
    {
        nameof(ElasticProvider),
        nameof(MongoProvider),
        nameof(MsSqlProvider),
        nameof(MySqlProvider),
        nameof(PostgresProvider),
        nameof(Ui),
    }
)]
partial class Build
{
    [Parameter][Secret] readonly string? SonarToken;

    [Parameter][Secret] readonly string? SonarTokenUi;

    [Parameter][Secret] readonly string? NugetApiKey;

    [Parameter] readonly string ElasticProvider = string.Empty;

    [Parameter] readonly string MongoProvider = string.Empty;

    [Parameter] readonly string MsSqlProvider = string.Empty;

    [Parameter] readonly string MySqlProvider = string.Empty;

    [Parameter] readonly string PostgresProvider = string.Empty;

    [Parameter] readonly string Ui = string.Empty;

    ReleaseParams[] ReleaseInfos() =>
    [
        new(nameof(ElasticProvider), ElasticProvider, "Serilog.Ui.ElasticSearchProvider"),
        new(nameof(MongoProvider), MongoProvider, "Serilog.Ui.MongoDbProvider"),
        new(nameof(MsSqlProvider), MsSqlProvider, "Serilog.Ui.MsSqlServerProvider"),
        new(nameof(MySqlProvider), MySqlProvider, "Serilog.Ui.MySqlProvider"),
        new(nameof(PostgresProvider), PostgresProvider, "Serilog.Ui.PostgreSqlProvider"),
        new(nameof(Ui), Ui, "Serilog.Ui.Web")
    ];

    readonly bool OnGithubActionRun = GitHubActions.Instance != null && !string.IsNullOrWhiteSpace($"{GitHubActions.Instance.RunId}");

    readonly bool IsPr = GitHubActions.Instance != null && GitHubActions.Instance.IsPullRequest;

    private bool RunSonarqube()
        => OnGithubActionRun && !IsPr && !string.IsNullOrWhiteSpace(SonarCloudInfo.Organization) && !string.IsNullOrWhiteSpace(SonarCloudInfo.BackendProjectKey);

    Target Backend_SonarScan_Start => targetDefinition => targetDefinition
        .DependsOn(Backend_Restore)
        .Executes(() =>
        {
            var condition = RunSonarqube();
            if (!condition)
            {
                Log.Information("--- Skipped Sonarqube analysis ---");
                return;
            }

            SonarScannerTasks.SonarScannerBegin(new SonarScannerBeginSettings()
                .SetExcludeTestProjects(true)
                .SetToken(SonarToken)
                .SetOrganization(SonarCloudInfo.Organization)
                .SetProjectKey(SonarCloudInfo.BackendProjectKey)
                .SetServer("https://sonarcloud.io")
                .SetSourceInclusions("src/**/*")
                .SetSourceExclusions(
                    "src/Serilog.Ui.Web/src/**/*",
                    "src/Serilog.Ui.Web/wwwroot/**/*",
                    "src/Serilog.Ui.Web/node_modules/**/*",
                    "src/Serilog.Ui.Web/*.js",
                    "src/Serilog.Ui.Web/*.ts",
                    "src/Serilog.Ui.Web/*.tsx",
                    "src/Serilog.Ui.Web/*.json")
                .SetGenericCoveragePaths("coverage/SonarQube.xml, ./coverage/SonarQube.xml")
                .SetProcessEnvironmentVariable("GITHUB_TOKEN", GitHubActions.Instance.Token)
                .SetProcessEnvironmentVariable("SONAR_TOKEN", SonarToken)
            );
        });

    Target Backend_SonarScan_End => targetDefinition => targetDefinition
        .DependsOn(Backend_Report_Ci)
        .Executes(() =>
        {
            var condition = RunSonarqube();
            if (!condition)
            {
                Log.Information("--- Skipped Sonarqube analysis ---");
                return;
            }

            SonarScannerTasks.SonarScannerEnd(new SonarScannerEndSettings()
                .SetToken(SonarToken)
                .SetProcessEnvironmentVariable("GITHUB_TOKEN", GitHubActions.Instance.Token)
                .SetProcessEnvironmentVariable("SONAR_TOKEN", SonarToken));
        });
}

public readonly record struct ReleaseParams(string Key, string ShouldPublish, string Project)
{
    public bool Publish() => ShouldPublish.Equals("true");
}