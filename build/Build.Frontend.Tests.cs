using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;

partial class Build : NukeBuild
{
    Target Frontend_Tests => _ => _
        .DependsOn(Frontend_Build)
        .Executes(() =>
        {
            NpmTasks.NpmRun(s => s
                .SetProcessWorkingDirectory(FrontendWorkingDirectory)
                .SetCommand("test:local")
            );
        });

    Target Frontend_Tests_Ci => _ => _
    .DependsOn(Frontend_Build)
    .ProceedAfterFailure()
    .Executes(() =>
    {
        NpmTasks.NpmRun(s => s
            .SetProcessWorkingDirectory(FrontendWorkingDirectory)
            .SetCommand("test:ci")
        );
    });
}