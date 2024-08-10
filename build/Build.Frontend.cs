using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;

partial class Build
{
    Target Frontend_Clean => targetDefinition => targetDefinition
        .Executes(() =>
        {
            FrontendWorkingDirectory
                .GlobDirectories("**/node_modules", "**/coverage")
                .ForEach(AbsolutePathExtensions.DeleteDirectory);
        });

    Target Frontend_Restore => targetDefinition => targetDefinition
        .DependsOn(Clean)
        .Before(Backend_Restore)
        .Executes(() =>
        {
            YarnTasks.YarnInstall(s => s
                .SetProcessWorkingDirectory(FrontendWorkingDirectory)
            );
        });

    Target Frontend_Build => targetDefinition => targetDefinition
        .DependsOn(Frontend_Restore)
        .Executes(() =>
        {
            YarnTasks.YarnRun(s => s
                .SetProcessWorkingDirectory(FrontendWorkingDirectory)
                .SetCommand("build")
            );
        });
}