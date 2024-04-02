using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;

partial class Build
{
    Target Frontend_Clean => _ => _
        .Executes(() =>
        {
            FrontendWorkingDirectory
                .GlobDirectories("**/node_modules", "**/coverage")
                .ForEach(AbsolutePathExtensions.DeleteDirectory);
        });

    Target Frontend_Restore => _ => _
        .DependsOn(Clean)
        .Before(Backend_Restore)
        .Executes(() =>
        {
            YarnTasks.YarnInstall(s => s
                .SetProcessWorkingDirectory(FrontendWorkingDirectory)
            );
        });

    Target Frontend_Build => _ => _
        .DependsOn(Frontend_Restore)
        .Executes(() =>
        {
            YarnTasks.YarnRun(s => s
                .SetProcessWorkingDirectory(FrontendWorkingDirectory)
                .SetCommand("build")
            );
        });
}