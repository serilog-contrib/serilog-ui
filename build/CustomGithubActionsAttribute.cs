using System.Collections.Generic;
using System.Linq;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.GitHubActions.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;

/// <summary>
/// from: https://github.com/RicoSuter/NSwag/blob/master/build/Build.CI.GitHubActions.cs
/// </summary>
class CustomGithubActionsAttribute(string name, GitHubActionsImage image, params GitHubActionsImage[] images)
    : GitHubActionsAttribute(name, image, images)
{
    public GithubAction[] AddGithubActions { get; set; } = [];

    public enum GithubAction
    {
        Frontend_SonarScan_Task,
        Frontend_Reporter,
        Frontend_Artifact,
        Backend_Reporter,
        Backend_Artifact,
    }

    protected override GitHubActionsJob GetJobs(GitHubActionsImage image, IReadOnlyCollection<ExecutableTarget> relevantTargets)
    {
        var job = base.GetJobs(image, relevantTargets);
        GitHubActionsStep[] backendSteps = AddGithubActions.Any(act => act == GithubAction.Backend_Artifact) ?
            [new GitHubActionSetupDotnet("6.0.x"), new GitHubActionSetupDotnet("8.0.x"),] : [];
        GitHubActionsStep[] setupSteps = [.. backendSteps, new GitHubActionSetupJava17(), .. job.Steps];
        var newSteps = new List<GitHubActionsStep>(setupSteps);

        foreach (var act in AddGithubActions)
        {
            switch (act)
            {
                case GithubAction.Frontend_SonarScan_Task:
                    newSteps.Add(new GithubActionSonarCloud());
                    break;
                case GithubAction.Frontend_Artifact:
                    newSteps.Add(new GithubActionUploadArtifact("'**/test-junit-report.xml'"));
                    break;
                case GithubAction.Frontend_Reporter:
                    newSteps.Add(new GithubActionReporter("JS - Tests", "'**/test-junit-report.xml'", "jest-junit"));
                    break;
                case GithubAction.Backend_Artifact:
                    newSteps.Add(new GithubActionUploadArtifact("'**/test-results.trx'"));
                    break;
                case GithubAction.Backend_Reporter:
                    newSteps.Add(new GithubActionReporter("DotNET - Tests", "'**/test-results.trx'", "dotnet-trx"));
                    break;
            }
        }

        job.Steps = [.. newSteps];
        return job;
    }
}

/// <summary>
/// using: https://github.com/actions/setup-java
/// </summary>
class GitHubActionSetupJava17 : GitHubActionsStep
{
    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine(); // empty line to separate tasks

        writer.WriteLine("- uses: actions/setup-java@v4");

        using (writer.Indent())
        {
            writer.WriteLine("with:");

            using (writer.Indent())
            {
                writer.WriteLine($"distribution: 'temurin'");
                writer.WriteLine($"java-version: '17'");
            }
        }
    }
}

class GitHubActionSetupDotnet(string dotnetV) : GitHubActionsStep
{
    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine(); // empty line to separate tasks

        writer.WriteLine("- uses: actions/setup-dotnet@v4");

        using (writer.Indent())
        {
            writer.WriteLine("with:");

            using (writer.Indent())
            {
                writer.WriteLine($"dotnet-version: '{dotnetV}'");
            }
        }
    }
}

class GithubActionUploadArtifact(string path) : GitHubActionsStep
{
    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine(); // empty line to separate tasks

        writer.WriteLine("- uses: actions/upload-artifact@v4");

        using (writer.Indent())
        {
            writer.WriteLine("if: always()");

            writer.WriteLine("with:");
            using (writer.Indent())
            {
                writer.WriteLine($"name: test-results");
                writer.WriteLine($"path: {path}");
            }
        }
    }
}

/// <summary>
/// using: https://github.com/SonarSource/sonarcloud-github-action
/// </summary>
class GithubActionSonarCloud : GitHubActionsStep
{
    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine(); // empty line to separate tasks

        writer.WriteLine("- name: SonarCloud run");

        using (writer.Indent())
        {
            writer.WriteLine("uses: SonarSource/sonarcloud-github-action@master");

            writer.WriteLine("if: github.event_name != 'pull_request'");

            writer.WriteLine("env:");
            using (writer.Indent())
            {
                writer.WriteLine("GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }} # Needed to get PR information, if any");
                writer.WriteLine("SONAR_TOKEN: ${{ secrets.SONAR_TOKEN_UI }}");
            }

            writer.WriteLine("with:");
            using (writer.Indent())
            {
                writer.WriteLine("args: >");
                using (writer.Indent())
                {
                    writer.WriteLine($"-Dsonar.organization={SonarCloudInfo.Organization}");
                    writer.WriteLine($"-Dsonar.projectKey={SonarCloudInfo.FrontendProjectKey}");
                    writer.WriteLine("-Dsonar.sources=src/Serilog.Ui.Web/src/");
                    writer.WriteLine("-Dsonar.tests=src/Serilog.Ui.Web/src/");
                    writer.WriteLine("-Dsonar.exclusions=src/Serilog.Ui.Web/src/__tests__/**/*,src/Serilog.Ui.Web/src/mockServiceWorker.*,src/Serilog.Ui.Web/src/style/**/*");
                    writer.WriteLine("-Dsonar.test.inclusions=src/Serilog.Ui.Web/src/__tests__/**/*");
                    writer.WriteLine("-Dsonar.javascript.lcov.reportPaths=./src/Serilog.Ui.Web/src/reports/coverage/lcov.info");
                }
            }
        }
    }
}

/// <summary>
/// using: https://github.com/phoenix-actions/test-reporting
/// from dorny/test-reporter => using fork to overcome issue #67
/// </summary>
class GithubActionReporter(string name, string path, string reporter) : GitHubActionsStep
{
    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine(); // empty line to separate tasks

        writer.WriteLine("- uses: dorny/test-reporter@v2");

        using (writer.Indent())
        {
            writer.WriteLine("if: always()");

            writer.WriteLine("with:");
            using (writer.Indent())
            {
                writer.WriteLine($"name: {name}");
                writer.WriteLine($"path: {path}");
                writer.WriteLine($"reporter: {reporter}");
                writer.WriteLine("fail-on-error: false");
            }
        }
    }
}