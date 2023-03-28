using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.GitHubActions.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// https://github.com/RicoSuter/NSwag/blob/master/build/Build.CI.GitHubActions.cs
[SuppressMessage("Major Bug", "S3903:Types should be defined in named namespaces", Justification = "As per standard creation")]
class CustomGithubActionsAttribute : GitHubActionsAttribute
{
    public CustomGithubActionsAttribute(string name, GitHubActionsImage image, params GitHubActionsImage[] images) : base(name, image, images)
    {
    }

    public GithubAction[] AddGithubActions { get; set; } = Array.Empty<GithubAction>();

    public enum GithubAction
    {
        SonarScanTask,
        FrontendReporter,
        BackendReporter
    }

    protected override GitHubActionsJob GetJobs(GitHubActionsImage image, IReadOnlyCollection<ExecutableTarget> relevantTargets)
    {
        var job = base.GetJobs(image, relevantTargets);
        var newSteps = new List<GitHubActionsStep>(job.Steps);

        foreach (var act in AddGithubActions)
        {
            switch (act)
            {
                case GithubAction.SonarScanTask:
                    newSteps.Add(new GithubActionSonarCloud());
                    break;
                case GithubAction.FrontendReporter:
                    newSteps.Add(new GithubActionReporter("JS - Tests", "'**/jest-*.xml'", "jest-junit"));
                    break;
                case GithubAction.BackendReporter:
                    newSteps.Add(new GithubActionReporter("DotNET - Tests", "'**/test-results.trx'", "dotnet-trx"));
                    break;
                default:
                    break;
            }
        }

        job.Steps = newSteps.ToArray();
        return job;
    }
}

[SuppressMessage("Major Bug", "S3903:Types should be defined in named namespaces", Justification = "As per standard creation")]
class GithubActionSonarCloud : GitHubActionsStep
{
    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine(); // empty line to separate tasks

        writer.WriteLine("- name: SonarCloud run");

        using (writer.Indent())
        {
            writer.WriteLine("uses: SonarSource/sonarcloud-github-action@master");
            writer.WriteLine("env:");
            using (writer.Indent())
            {
                writer.WriteLine("GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }} # Needed to get PR information, if any");
                writer.WriteLine("SONAR_TOKEN: ${{ secrets.SONAR_TOKEN_UI }}");
            }

            writer.WriteLine("with:");
            using (writer.Indent())
            {
                writer.WriteLine($"args: >");
                using (writer.Indent())
                {
                    writer.WriteLine($"-Dsonar.organization=followynne");
                    writer.WriteLine("-Dsonar.projectKey=followynne_serilog-ui_assets");
                    writer.WriteLine("-Dsonar.sources=src/Serilog.Ui.Web/assets/");
                    writer.WriteLine($"-Dsonar.tests=src/Serilog.Ui.Web/assets/");
                    writer.WriteLine($"-Dsonar.exclusions=src/Serilog.Ui.Web/assets/__tests__/**/*");
                    writer.WriteLine($"-Dsonar.test.inclusions=src/Serilog.Ui.Web/assets/__tests__/**/*");
                    writer.WriteLine($"-Dsonar.javascript.lcov.reportPaths=./src/Serilog.Ui.Web/coverage/lcov.info");
                }
            }
        }
    }
}

[SuppressMessage("Major Bug", "S3903:Types should be defined in named namespaces", Justification = "As per standard creation")]
class GithubActionReporter : GitHubActionsStep
{
    readonly string name;
    readonly string path;
    readonly string reporter;

    public GithubActionReporter(string name, string path, string reporter)
    {
        this.name = name;
        this.path = path;
        this.reporter = reporter;
    }
    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine(); // empty line to separate tasks

        // dorny/test-reporter@v1.6.0 => using fork to overcome issue #67
        writer.WriteLine("- uses: phoenix-actions/test-reporting@v9");

        using (writer.Indent())
        {
            writer.WriteLine("if: always()");

            writer.WriteLine("with:");
            using (writer.Indent())
            {
                writer.WriteLine($"name: {name}");
                writer.WriteLine($"output-to: step-summary");
                writer.WriteLine($"path: {path}");
                writer.WriteLine($"reporter: {reporter}");
                writer.WriteLine("fail-on-error: false");
            }
        }
    }
}
