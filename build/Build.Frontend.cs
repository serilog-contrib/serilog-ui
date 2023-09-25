using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.Npm;
using Nuke.Common.Utilities.Collections;

partial class Build : NukeBuild
{
    Target Frontend_Clean => _ => _
        .Executes(() =>
        {
            FrontendWorkingDirectory
                .GlobDirectories("**/node_modules", "**/.parcel-cache", "**/coverage")
                .ForEach(AbsolutePathExtensions.DeleteDirectory);
        });

    Target Frontend_Restore => _ => _
        .DependsOn(Clean)
        .Before(Backend_Restore)
        .Executes(() =>
        {
            NpmTasks.NpmCi(s => s
                .SetProcessWorkingDirectory(FrontendWorkingDirectory)
            );
        });

    Target Frontend_Build => _ => _
    .DependsOn(Frontend_Restore)
    .Executes(() =>
    {
        NpmTasks.NpmRun(s => s
            .SetProcessWorkingDirectory(FrontendWorkingDirectory)
            .SetCommand("build")
        );
    });
}