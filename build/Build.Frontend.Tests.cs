using Nuke.Common;
using Nuke.Common.Tooling;

partial class Build
{
    Target Frontend_Tests => targetDefinition => targetDefinition
        .DependsOn(Frontend_Build)
        .Executes(() =>
        {
            YarnTasks.YarnRun(s => s
                .SetProcessWorkingDirectory(FrontendWorkingDirectory)
                .SetCommand("test")
            );
        });

    Target Frontend_Tests_Ci => targetDefinition => targetDefinition
        .DependsOn(Frontend_Build)
        .ProceedAfterFailure()
        .Executes(() =>
        {
            YarnTasks.YarnRun(s => s
                .SetProcessWorkingDirectory(FrontendWorkingDirectory)
                .SetCommand("test:ci")
            );
        });
}