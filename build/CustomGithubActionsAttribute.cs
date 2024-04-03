using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.GitHubActions.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// from: https://github.com/RicoSuter/NSwag/blob/master/build/Build.CI.GitHubActions.cs
/// </summary>
class CustomGithubActionsAttribute(string name, GitHubActionsImage image, params GitHubActionsImage[] images)
    : GitHubActionsAttribute(name, image, images)
{
    public GithubAction[] AddGithubActions { get; set; } = Array.Empty<GithubAction>();

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
        var newSteps = new List<GitHubActionsStep>(new GitHubActionsStep[] { new GitHubActionSetupJava17() }.Concat(job.Steps));

        foreach (var act in AddGithubActions)
        {
            switch (act)
            {
                case GithubAction.Frontend_SonarScan_Task:
                    newSteps.Add(new GithubActionSonarCloud());
                    break;
                case GithubAction.Frontend_Artifact:
                    newSteps.Add(new GithubActionUploadArtifact("'**/test-report.xml'"));
                    break;
                case GithubAction.Frontend_Reporter:
                    newSteps.Add(new GithubActionReporter("JS - Tests", "'**/test-report.xml'", "jest-junit"));
                    break;
                case GithubAction.Backend_Artifact:
                    newSteps.Add(new GithubActionUploadArtifact("'**/test-results.trx'"));
                    break;
                case GithubAction.Backend_Reporter:
                    newSteps.Add(new GithubActionReporter("DotNET - Tests", "'**/test-results.trx'", "dotnet-trx"));
                    break;
            }
        }

        job.Steps = newSteps.ToArray();
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

class GithubActionUploadArtifact(string path) : GitHubActionsStep
{
    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine(); // empty line to separate tasks

        writer.WriteLine("- uses: actions/upload-artifact@v3");

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

            // writer.WriteLine("if: github.event_name != 'pull_request'");

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
                    writer.WriteLine("-Dsonar.exclusions=src/Serilog.Ui.Web/src/__tests__/**/*");
                    writer.WriteLine("-Dsonar.test.inclusions=src/Serilog.Ui.Web/src/__tests__/**/*");
                    writer.WriteLine("-Dsonar.javascript.lcov.reportPaths=./src/Serilog.Ui.Web/src/reports/coverage/lcov.info");
                }
            }
        }
    }
}

/// <summary>
/// using: https://github.com/phoenix-actions/test-reporting
/// from dorny/test-reporter@v1.6.0 => using fork to overcome issue #67
/// </summary>
class GithubActionReporter(string name, string path, string reporter) : GitHubActionsStep
{
    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine(); // empty line to separate tasks

        writer.WriteLine("- uses: phoenix-actions/test-reporting@v14");

        using (writer.Indent())
        {
            writer.WriteLine("if: always()");

            writer.WriteLine("with:");
            using (writer.Indent())
            {
                writer.WriteLine($"name: {name}");
                writer.WriteLine($"output-to: checks");
                writer.WriteLine($"path: {path}");
                writer.WriteLine($"reporter: {reporter}");
                writer.WriteLine("fail-on-error: false");
            }
        }
    }
}